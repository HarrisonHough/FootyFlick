using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private PlayerScore playerScore;
    [SerializeField]
    private RandomPositionInSector randomPositionInSector;
    [SerializeField]
    private GameCanvas gameCanvas;
    [SerializeField]
    private GameModePrefabs gameModePrefabs;
    [SerializeField]
    private WindControl windControl;
    public WindControl WindControl => windControl;
    
    public static Action<GameStateEnum> OnGameStateChanged;
    
    [SerializeField] private GameModeBase currentGameMode;
    
    
    private void Start()
    {
        windControl = GetComponent<WindControl>();
    }

    private void OnDestroy()
    {
        
    }

    public static void SetGameState(GameStateEnum gameState)
    {
        OnGameStateChanged?.Invoke(gameState);
    }

    public GameCanvas GetGameCanvas()
    {
        return gameCanvas;
    }
    
    public PlayerScore GetPlayerScore()
    {
        return playerScore;
    }
    
    public void SetGameMode(GameModeEnum modeEnum)
    {
        if(currentGameMode != null)
        {
            currentGameMode.EndMode();
            Destroy(currentGameMode.gameObject);
        }
        var newGameMode = gameModePrefabs.GetGameMode(modeEnum);
        if (newGameMode == null)
        {
            Debug.LogError($"Game mode {modeEnum} not found.");
            return;
        }
        newGameMode = Instantiate(newGameMode, transform);
        currentGameMode = newGameMode;
        currentGameMode.Initialize(this);
    }

    public void StartGame()
    {
        if (currentGameMode == null)
        {
            Debug.Log( "Game mode is null, setting default mode.");
            SetGameMode(GameModeEnum.GoalOrNothing);
        }
        OnGameStateChanged?.Invoke(GameStateEnum.GameStarted);
        currentGameMode.StartMode();
    }
    
    public void MovePlayerToRandomPosition()
    {
        MovePlayerToPosition(randomPositionInSector.GetRandomPositionInSector());
    }
    
    public void MovePlayerToPosition( Vector3 position)
    {
        player.MoveToPosition(position);
    }


}

