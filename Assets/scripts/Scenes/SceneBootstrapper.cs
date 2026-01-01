using UnityEngine;

public class SceneBootstrapper : MonoBehaviour
{
    [SerializeField] string initialScene = "Street";
    [SerializeField] string initialSpawnId = "StreetStart";

    void Start()
    {
        SceneLoader.Instance.LoadContentScene(initialScene, initialSpawnId);
    }

}
