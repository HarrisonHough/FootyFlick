using UnityEngine;

public enum AudioId
{
    Kick,
    Score,
    BallHitGoalPost,
    OutOfBounds,
    MusicLoop,
    Fail
}

[CreateAssetMenu(fileName = "AudioLibrary", menuName = "FlickFooty/Audio Library")]
public class AudioLibrary : ScriptableObject
{
    [SerializeField]
    private AudioClip[] kickClips;
    [SerializeField]
    private AudioClip goalClip;
    [SerializeField]
    private AudioClip[] ballHitGoalPostClips;
    [SerializeField]
    private AudioClip musicLoopClip;
    [SerializeField]
    private AudioClip outOfBoundsClip;
    
    public AudioClip GetAudioClip(AudioId id)
    {
        switch (id)
        {
            case AudioId.Kick:
                return GetRandomAudioClip(kickClips);
            case AudioId.Score:
                return goalClip;
            case AudioId.BallHitGoalPost:
                return GetRandomAudioClip(ballHitGoalPostClips);
            case AudioId.MusicLoop:
                return musicLoopClip;
            case AudioId.OutOfBounds:
                 return outOfBoundsClip;
            default:
                Debug.LogWarning($"AudioId {id} not recognized.");
                return null;
        }
    }
    
    AudioClip GetRandomAudioClip(AudioClip[] clips)
    {
        return clips.Length == 0 ? null : clips[Random.Range(0, clips.Length)];
    }
}

