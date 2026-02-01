using UnityEngine;
using UnityEngine.Audio;

public class SfxManager : MonoBehaviour
{
    public static SfxManager Instance { get; private set; }

    [Header("Mixer")]
    public AudioMixerGroup sfxGroup;   // optional

    [Header("2D Loop (Scroll etc.)")]
    AudioSource loop2DSource;
    GameObject loop2DObject;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // -------------------------
    // One-shot 2D SFX
    // -------------------------
    public void PlaySFX2D(AudioClip clip, float volume = 1f, float pitch = 1f)
    {
        if (clip == null) return;

        var go = new GameObject("SFX2D_" + clip.name);
        go.transform.SetParent(transform, worldPositionStays: false);

        var src = go.AddComponent<AudioSource>();
        src.clip = clip;
        src.volume = Mathf.Clamp01(volume);
        src.pitch = Mathf.Max(0.01f, pitch);
        src.spatialBlend = 0f; // 2D

        if (sfxGroup != null)
            src.outputAudioMixerGroup = sfxGroup;

        src.Play();

        Destroy(go, clip.length / src.pitch + 0.1f);
    }

    // -------------------------
    // 2D Loop SFX (ScrollSound etc.)
    // -------------------------
    public void StartLoop2D(AudioClip clip, float volume = 1f, float pitch = 1f)
    {
        if (clip == null) return;

        if (loop2DSource != null && loop2DSource.isPlaying && loop2DSource.clip == clip)
            return;

        StopLoop2D();

        loop2DObject = new GameObject("SFX_LOOP2D_" + clip.name);
        loop2DObject.transform.SetParent(transform, worldPositionStays: false);

        loop2DSource = loop2DObject.AddComponent<AudioSource>();
        loop2DSource.clip = clip;
        loop2DSource.loop = true;
        loop2DSource.volume = Mathf.Clamp01(volume);
        loop2DSource.pitch = Mathf.Max(0.01f, pitch);
        loop2DSource.spatialBlend = 0f; // 2D

        if (sfxGroup != null)
            loop2DSource.outputAudioMixerGroup = sfxGroup;

        loop2DSource.Play();
    }

    public void StopLoop2D()
    {
        if (loop2DSource == null) return;

        loop2DSource.Stop();

        if (loop2DObject != null)
            Destroy(loop2DObject);

        loop2DSource = null;
        loop2DObject = null;
    }
}
