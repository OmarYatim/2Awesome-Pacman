using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [HideInInspector] public static SoundManager Instance;
    // Start is called before the first frame update
    void Awake() => Instance = this;

    public void PlayClip(AudioClip clip)
    {
        this.source.clip = clip;
        this.source.Play();
    }

    public void PlayClipNoReset(AudioClip clip)
    {
        if (!this.source.isPlaying)
            this.PlayClip(clip);
    }
    public void PlayUnpausedClip(AudioClip clip)
    {
        this.source.ignoreListenerPause = true;
        this.PlayClip(clip);
    }

    public void PauseSound() => this.source.Pause();
    public void UnPauseSound() => this.source.UnPause();
}
