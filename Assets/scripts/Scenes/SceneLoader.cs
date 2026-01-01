using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    [SerializeField] Transform playerRoot;
    [SerializeField] string currentContentScene;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void LoadContentScene(string newScene, string spawnId)
    {
        StartCoroutine(LoadRoutine(newScene, spawnId));
    }

    IEnumerator LoadRoutine(string newScene, string spawnId)
    {
        if (!IsLoaded(newScene))
            yield return SceneManager.LoadSceneAsync(newScene, LoadSceneMode.Additive);

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(newScene));

        PlacePlayerAtSpawn(spawnId);

        if (!string.IsNullOrEmpty(currentContentScene) &&
            currentContentScene != newScene &&
            IsLoaded(currentContentScene))
        {
            yield return SceneManager.UnloadSceneAsync(currentContentScene);
        }

        currentContentScene = newScene;
    }

    void PlacePlayerAtSpawn(string spawnId)
    {
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
