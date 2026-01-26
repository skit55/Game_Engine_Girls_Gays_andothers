using UnityEngine;

public class SceneBootstrapper : MonoBehaviour
{
    [SerializeField] public string initialScene = "Street";
    [SerializeField] public string initialSpawnId = "StreetStart";

    void Start()
    {
        SceneLoader.Instance.LoadContentScene(initialScene, initialSpawnId);
    }

}
