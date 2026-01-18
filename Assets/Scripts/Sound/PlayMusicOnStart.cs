using UnityEngine;

public class PlayMusicOnStart : MonoBehaviour
{
    public AudioClip music;

    void Start()
    {
        SoundManager.Instance.PlayMusic(music);
    }
}