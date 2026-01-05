using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    [SerializeField] Transform playerRoot;
    [SerializeField] string currentContentScene;

    bool isLoading;

    // optional: "last request wins" während loading
    string pendingScene;
    string pendingSpawn;
    bool hasPending;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;

        // Wenn dein Core über Szenen hinweg leben soll:
        DontDestroyOnLoad(gameObject);
    }

    public void LoadContentScene(string newScene, string spawnId)
    {
        // falls während loading nochmal gedrückt wird: merken, aber nicht parallel starten
        if (isLoading)
        {
            pendingScene = newScene;
            pendingSpawn = spawnId;
            hasPending = true;
            return;
        }

        StartCoroutine(LoadRoutine(newScene, spawnId));
    }

    IEnumerator LoadRoutine(string newScene, string spawnId)
    {
        isLoading = true;

        // falls es schon aktiv ist + Spieler nur umsetzen
        if (IsLoaded(newScene))
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(newScene));
            PlacePlayerAtSpawn(spawnId);

            // kleiner Puffer gegen Spam-Input
            yield return new WaitForSeconds(0.15f);

            isLoading = false;
            yield return HandlePendingIfAny();
            yield break;
        }

        // neue Content-Scene laden
        yield return SceneManager.LoadSceneAsync(newScene, LoadSceneMode.Additive);

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(newScene));

        PlacePlayerAtSpawn(spawnId);

        // alte Content-Scene unloaden (wenn vorhanden)
        if (!string.IsNullOrEmpty(currentContentScene) &&
            currentContentScene != newScene &&
            IsLoaded(currentContentScene))
        {
            yield return SceneManager.UnloadSceneAsync(currentContentScene);
        }

        currentContentScene = newScene;

        // kleiner Puffer gegen Spam-Input
        yield return new WaitForSeconds(0.15f);

        isLoading = false;

        // wenn währenddessen noch Requests kamen: einmal die letzte ausführen
        yield return HandlePendingIfAny();
    }

    IEnumerator HandlePendingIfAny()
    {
        if (!hasPending) yield break;

        // pending abholen
        var s = pendingScene;
        var sp = pendingSpawn;
        hasPending = false;
        pendingScene = null;
        pendingSpawn = null;

        // direkt nächste Ladung starten
        yield return LoadRoutine(s, sp);
    }

    void PlacePlayerAtSpawn(string spawnId)
    {
        if (!playerRoot)
        {
            Debug.LogError("SceneLoader: playerRoot is not assigned!");
            return;
        }

        foreach (var sp in FindObjectsOfType<SpawnPoint>(true))
        {
            if (sp.id == spawnId)
            {
                var cc = playerRoot.GetComponent<CharacterController>();
                if (cc) cc.enabled = false;

                playerRoot.position = sp.transform.position;

                if (cc) cc.enabled = true;
                return;
            }
        }

        Debug.LogError($"SpawnPoint '{spawnId}' not found");
    }

    bool IsLoaded(string name)
    {
        var s = SceneManager.GetSceneByName(name);
        return s.IsValid() && s.isLoaded;
    }
}
