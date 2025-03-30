
public interface IGameMode
{
    void StartMode();
    void OnKickResult(KickResult kickResult); // Called when player kicks
    void Update(float deltaTime);         // Optional: for modes like Time Attack
    bool IsGameOver { get; }
    string GetStatusText();               // For UI (e.g. timer, score, etc.)
    void EndMode();                       // Cleanup if needed
}
