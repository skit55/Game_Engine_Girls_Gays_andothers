using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [Header("Clips")]
    [SerializeField] AudioClip street1Loop;
    [SerializeField] AudioClip street2Loop;
    [SerializeField] AudioClip indoorLoop;
    [SerializeField] AudioClip fightLoop;

    [Header("Mixer")]
    [SerializeField] AudioMixerGroup musicGroup; // <- assign in Inspector (e.g. "Music")

    [Header("Settings")]
    [SerializeField] float volume = 0.9f;
    [SerializeField] float fadeTime = 0.25f;

    AudioSource src;
    AudioClip current;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        src = GetComponent<AudioSource>();
        if (!src) src = gameObject.AddComponent<AudioSource>();

        src.loop = true;
        src.playOnAwake = false;
        src.spatialBlend = 0f; // 2D
        src.volume = volume;

        // Route through mixer group
        if (musicGroup != null)
            src.outputAudioMixerGroup = musicGroup;
    }

    // Called by SceneAudioTag.Start()
    public void SetZone(MusicZone zone)
    {
        AudioClip target = zone switch
        {
            MusicZone.Street1 => street1Loop,
            MusicZone.Street2 => street2Loop,
            MusicZone.Indoor => indoorLoop,
            MusicZone.Fight => fightLoop,
            _ => null
        };

        if (!target) return;
        if (target == current && src.isPlaying) return;

        StopAllCoroutines();
        StartCoroutine(FadeTo(target));
    }

    IEnumerator FadeTo(AudioClip next)
    {
        current = next;

        float startVol = src.volume;

        // Fade out
        for (float t = 0; t < fadeTime; t += Time.unscaledDeltaTime)
        {
            src.volume = Mathf.Lerp(startVol, 0f, t / fadeTime);
            yield return null;
        }

        src.Stop();
        src.clip = next;
        src.Play();

        // Fade in
        for (float t = 0; t < fadeTime; t += Time.unscaledDeltaTime)
        {
            src.volume = Mathf.Lerp(0f, volume, t / fadeTime);
            yield return null;
        }

        src.volume = volume;
    }
}
