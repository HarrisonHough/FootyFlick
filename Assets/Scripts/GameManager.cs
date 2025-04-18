using System;
using UnityEngine;
using UnityEngine.Serialization;

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
    public static Action<GameModeEnum> OnGameModeChanged;
    private GameModeEnum currentGameMode;
    [SerializeField] private GameModeBase currentGameModeObject;
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
                if(currentGameModeObject != null)
                {
                    currentGameModeObject.EndMode();
                    Destroy(currentGameModeObject.gameObject);
                    currentGameModeObject = null;
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
        currentGameMode = modeEnum;
        if(currentGameModeObject != null)
        {
            currentGameModeObject.EndMode();
            Destroy(currentGameModeObject.gameObject);
        }
        var newGameMode = gameModePrefabs.GetGameMode(currentGameMode);
        if (newGameMode == null)
        {
            Debug.LogError($"Game mode {currentGameMode} not found.");
            return;
        }
        newGameMode = Instantiate(newGameMode, transform);
        currentGameModeObject = newGameMode;
        currentGameModeObject.Initialize(this);
        SetSwipeDisabled(false);
        OnGameModeChanged?.Invoke(currentGameMode);
    }

    public void StartGame()
    {
        if (currentGameModeObject == null)
        {
            SetGameMode(currentGameMode);
        }
        SetGameState(GameStateEnum.GameStarted);
        currentGameModeObject.StartMode();
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

