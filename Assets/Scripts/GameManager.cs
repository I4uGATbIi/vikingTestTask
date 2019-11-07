using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public delegate void GameStageHandler(GameStage changedStage, bool isGamePaused);

    public static event GameStageHandler StageChanged;

    private GameStage currentStage;

    public GameStage CurrentStage
    {
        get => currentStage;
        private set
        {
            currentStage = value;
            StageChanged?.Invoke(currentStage, IsGamePaused);
        }
    }

    public bool IsGamePaused => CurrentStage != GameStage.Game;

    void Awake()
    {
        
    }
}

public enum GameStage
{
    MainMenu = 1,
    Game,
    EndGameMenu
}