using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    [SerializeField] Transform playerRoot;
    [SerializeField] string currentContentScene;

    public string CurrentContentSceneName => currentContentScene;

    bool isLoading;

    string pendingScene;
    string pendingSpawn;
    bool hasPending;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void LoadContentScene(string newScene, string spawnId)
    {
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

        // Wenn die Scene schon geladen ist: nur aktiv setzen + Player umsetzen
        if (IsLoaded(newScene))
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(newScene));
            currentContentScene = newScene;

            PlacePlayerAtSpawn(spawnId);

            // Musik kommt automatisch über SceneAudioTag.Start()

            yield return new WaitForSeconds(0.15f);

            isLoading = false;
            yield return HandlePendingIfAny();
            yield break;
        }

        // alte merken, bevor wir überschreiben
        string oldScene = currentContentScene;

        // neue Content-Scene laden
        yield return SceneManager.LoadSceneAsync(newScene, LoadSceneMode.Additive);

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(newScene));
        currentContentScene = newScene;

        PlacePlayerAtSpawn(spawnId);

        // Musik kommt automatisch über SceneAudioTag.Start() in der neu geladenen Scene

        // alte Content-Scene unloaden (wenn vorhanden)
        if (!string.IsNullOrEmpty(oldScene) &&
            oldScene != newScene &&
            IsLoaded(oldScene))
        {
            yield return SceneManager.UnloadSceneAsync(oldScene);
        }

        yield return new WaitForSeconds(0.15f);

        isLoading = false;
        yield return HandlePendingIfAny();
    }

    IEnumerator HandlePendingIfAny()
    {
        if (!hasPending) yield break;

        var s = pendingScene;
        var sp = pendingSpawn;
        hasPending = false;
        pendingScene = null;
        pendingSpawn = null;

        yield return LoadRoutine(s, sp);
    }

    void PlacePlayerAtSpawn(string spawnId)
    {
        if (!playerRoot)
        {
            Debug.LogError("SceneLoader: playerRoot is not assigned!");
            return;
        }

        foreach (var sp in Object.FindObjectsByType<SpawnPoint>(FindObjectsInactive.Include, FindObjectsSortMode.None))
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
