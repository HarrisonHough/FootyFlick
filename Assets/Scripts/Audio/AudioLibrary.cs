using System.Linq;
using UnityEngine;

public enum AudioId
{
    Kick,
    Goal,
    Point,
    BallHitGround,
    BallHitGoalPost,
    BallHitWall,
    MusicLoop,
    GameOver,
    CrowdAmbience
}

[System.Serializable]
public struct AudioItem
{
    public AudioId Id;
    public AudioClip Clip;
}


[CreateAssetMenu(fileName = "AudioLibrary", menuName = "Scriptable Objects/AudioLibrary")]
public class AudioLibrary : ScriptableObject
{
    [SerializeField]
    private AudioItem[] audioItems;

    public AudioClip GetAudioClip(AudioId id)
    {
        var audioItem = audioItems.FirstOrDefault(item => item.Id == id);
        if (audioItem.Clip != null)
        {
            return audioItem.Clip;
        }
        Debug.LogWarning($"Audio clip for {id} not found in AudioLibrary.");
        return null;
    }
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        var duplicateIds = audioItems
            .GroupBy(item => item.Id)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        if (duplicateIds.Any())
        {
            Debug.LogWarning($"AudioLibrary has duplicate AudioIds: {string.Join(", ", duplicateIds)}");
        }
    }
#endif
}

