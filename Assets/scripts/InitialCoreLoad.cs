using UnityEngine;
using UnityEngine.SceneManagement;

public class InitialCoreLoad : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void LoadCore()
    {
        SceneManager.LoadScene("Core");
    }
}
