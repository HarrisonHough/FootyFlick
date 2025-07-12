using UnityEngine;

public enum AudioId
{
    Kick,
    Score,
    BallHitGoalPost,
    MusicLoop,
    Fail
}

[CreateAssetMenu(fileName = "AudioLibrary", menuName = "FlickFooty/Audio Library")]
public class AudioLibrary : ScriptableObject
{
    [SerializeField]
    private AudioClip kickClip;
    [SerializeField]
    private AudioClip goalClip;
    [SerializeField]
    private AudioClip ballHitGoalPostClip;
    [SerializeField]
    private AudioClip musicLoopClip;
    
    public AudioClip GetAudioClip(AudioId id)
    {
        switch (id)
        {
            case AudioId.Kick:
                return kickClip;
            case AudioId.Score:
                return goalClip;
            case AudioId.BallHitGoalPost:
                return ballHitGoalPostClip;
            case AudioId.MusicLoop:
                return musicLoopClip;
            default:
                Debug.LogWarning($"AudioId {id} not recognized.");
                return null;
        }
    }
}

