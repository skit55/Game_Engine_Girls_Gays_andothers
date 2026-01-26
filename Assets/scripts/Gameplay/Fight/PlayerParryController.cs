using System.Collections;
using UnityEngine;

public class PlayerParryController : MonoBehaviour
{
    [SerializeField] ParryZone up, right, left, down;
    [SerializeField] Transform visualRoot;

    [SerializeField] float armedWindow = 0.12f;
    [SerializeField] float cooldown = 0.18f;

    bool onCooldown;
    Coroutine routine;

    void OnEnable()
    {
        if (InputHub.Instance != null)
            InputHub.Instance.ParryPressed += OnParry;
    }

    void OnDisable()
    {
        if (InputHub.Instance != null)
            InputHub.Instance.ParryPressed -= OnParry;
    }

    void OnParry(Direction dir)
    {
        var gsm = GameStateManager.Instance;
        if (gsm != null && gsm.CurrentState != GameState.Fight) return;
        if (onCooldown) return;

        ParryZone z = dir switch
        {
            Direction.Up => up,
            Direction.Right => right,
            Direction.Left => left,
            Direction.Down => down,
            _ => null
        };
        if (!z) return;

        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(ParryRoutine(z, dir));
    }

    IEnumerator ParryRoutine(ParryZone zone, Direction dir)
    {
        onCooldown = true;

        // only one active at a time
        DisableAll();
        zone.SetActive(true);

        // optional: rotate/lean here

        yield return new WaitForSeconds(armedWindow);
        zone.SetActive(false);

        yield return new WaitForSeconds(cooldown);
        onCooldown = false;
        routine = null;
    }

    void DisableAll()
    {
        if (up) up.SetActive(false);
        if (right) right.SetActive(false);
        if (left) left.SetActive(false);
        if (down) down.SetActive(false);
    }
}
