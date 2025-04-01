
using UnityEngine;

public abstract class GameModeBase : MonoBehaviour
{
    protected GameManager gameManager;
    public abstract void Initialize(GameManager gameManager);
    public abstract void StartMode();
    public abstract void OnKickResult(KickData kickData);
    public virtual void EndMode() { }
    
}
