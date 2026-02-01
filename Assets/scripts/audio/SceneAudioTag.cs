using UnityEngine;

public enum MusicZone { Street1, Street2, Indoor, Fight }

public class SceneAudioTag : MonoBehaviour
{
    public MusicZone zone;

    void Start()
    {
        // Sobald die Scene aktiv ist und das Objekt existiert, setzt es die Musik.
        if (MusicManager.Instance != null)
            MusicManager.Instance.SetZone(zone);
    }
}
