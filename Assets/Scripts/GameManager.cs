using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private PlayerScore playerScore;
    [SerializeField]
    private InputHandler inputHandler;
    [SerializeField]
    private RandomPositionInSector randomPositionInSector;
    [SerializeField]
    private GameModePrefabs gameModePrefabs;
    [SerializeField]
    private WindControl windControl;
    public WindControl WindControl => windControl;
    
    public static Action<GameStateEnum> OnGameStateChanged;
    
    [SerializeField] private GameModeBase currentGameMode;
    public static GameStateEnum CurrentGameState { get; private set; } = GameStateEnum.Home;
    
    private void Start()
    {
        windControl = GetComponent<WindControl>();
        OnGameStateChanged += HandleGameStateChanged;
        MovePlayerToRandomPosition();
    }


    private void OnDestroy()
    {
        
    }
    
    public void SetSwipeDisabled(bool disabled)
    {
        inputHandler.SetSwipeDisabled(disabled);
    }
    
    private void HandleGameStateChanged(GameStateEnum gameState)
    {
        switch (gameState)
        {
            case GameStateEnum.GameStarted:
                break;
            case GameStateEnum.Home:
                if(currentGameMode != null)
                {
                    currentGameMode.EndMode();
                    Destroy(currentGameMode.gameObject);
                    currentGameMode = null;
                }
                break;
            default:
                break;
        }
    }

    public static void SetGameState(GameStateEnum gameState)
    {
        CurrentGameState = gameState;
        OnGameStateChanged?.Invoke(CurrentGameState);
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
        SetSwipeDisabled(false);
    }

    public void StartGame()
    {
        if (currentGameMode == null)
        {
            Debug.Log( "Game mode is null, setting default mode.");
            SetGameMode(GameModeEnum.Practice);
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

