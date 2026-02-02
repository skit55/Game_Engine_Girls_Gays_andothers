using System;
using System.Collections;
using UnityEngine;

public class EnemyTurnController : MonoBehaviour
{
    [SerializeField] ProjectileSpawner spawner;

    public event Action EnemyTurnFinished;
    public event Action EnemyDied;
    public event Action<int, int> EnemyHpChanged; // current, max

    public EnemyData enemy;
    bool isRunning;

    public bool IsRunning => isRunning;

    public void BeginEnemyTurn(EnemyData data)
    {
        if (isRunning) return;
        enemy = data;
        StartCoroutine(EnemyRoutine());
    }
    public void StopSpawns() {
        StopAllCoroutines();
    }
    IEnumerator EnemyRoutine()
    {
        isRunning = true;

        for (int i = 0; i < enemy.pattern.Length; i++)
        {
            Direction dir = enemy.pattern[i];
            spawner.Spawn(dir, enemy);
            SfxManager.Instance.PlaySFX2D(SfxBank.Instance.projectileSpawn);

            yield return new WaitForSeconds(enemy.chainGap);
        }

        isRunning = false;
        yield return new WaitForSeconds(enemy.projectileTravelTime);
        EnemyTurnFinished?.Invoke();
    }
}
