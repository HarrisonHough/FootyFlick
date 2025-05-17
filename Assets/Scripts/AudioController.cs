using UnityEngine;
using UnityEngine.Audio;

public class AudioController : GenericSingleton<AudioController>
{
    private AudioMixer audioMixer;
    private AudioSource musicSource;
    private AudioSource sfxSource;
    private const string SFX_GROUP = "SFX";
    private const string MUSIC_GROUP = "Music";

    public override void Awake()
    {
        destroyOnLoad = false;
        base.Awake();
        Init();
    }

    public void Init()
    {
        if(audioMixer == null)
        {
            audioMixer = Resources.Load<AudioMixer>("Audio/AudioMixer");
        }
        if(sfxSource == null)
        {
            var sfxSourcePrefab = Resources.Load<GameObject>("Audio/AudioSourceSFX");
            sfxSource = Instantiate(sfxSourcePrefab).GetComponent<AudioSource>();
            sfxSource.transform.SetParent(transform);
        }
        if(musicSource == null)
        {
            var musicSourcePrefab = Resources.Load<GameObject>("Audio/AudioSourceSFX");
            musicSource = Instantiate(musicSourcePrefab).GetComponent<AudioSource>();
            musicSource.transform.SetParent(transform);
        }
    }
    
    public void SetMusicMuted(bool muted)
    {
        Init();
        musicSource.outputAudioMixerGroup.audioMixer.SetFloat(MUSIC_GROUP, muted ? -80 : 0);
    }

    public void SetSFXMuted(bool muted)
    {
        Init();
        sfxSource.outputAudioMixerGroup.audioMixer.SetFloat(SFX_GROUP, muted ? -80 : 0);
    }
    
    public void PlaySFX(AudioClip clip)
    {
        Init();
        sfxSource.clip = clip;
        sfxSource.Play();
    }
    
    public void PlayMusic(AudioClip clip)
    {
        Init();
        musicSource.clip = clip;
        musicSource.Play();
    }
}
