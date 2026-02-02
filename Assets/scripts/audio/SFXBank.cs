using UnityEngine;

public class SfxBank : MonoBehaviour
{
    public static SfxBank Instance { get; private set; }

    [Header("Fight")]
    public AudioClip fightEnter;
    public AudioClip parry;
    public AudioClip hurt;
    public AudioClip scrollLoop;
    public AudioClip hit;
    public AudioClip miss;
    public AudioClip turnStart;
    public AudioClip win;
    public AudioClip lose;
    public AudioClip projectileSpawn;

    [Header("Environment")]
    public AudioClip door;
    public AudioClip pickupKey;
    public AudioClip dialogue;


    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
