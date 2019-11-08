using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public delegate void GameStageHandler(GameStage changedStage, bool isGamePaused);

    public static event GameStageHandler StageChanged;

    [SerializeField] private GameStage currentStage;

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

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        CurrentStage = GameStage.MainMenu;
    }

    public void StartGame()
    {
        CurrentStage = GameStage.Game;
        GameObject.FindWithTag("Player").transform.position = GetSpawnPosition();
    }

    public void ExitGame()
    {
        Application.Quit(0);
    }

    private Vector3 GetSpawnPosition()
    {
        Terrain terrain = GameObject.FindWithTag("Terrain").GetComponent<Terrain>();
        var terrainData = terrain.terrainData;
        var position = terrain.transform.position;
        float randX = Random.Range(position.x, position.x + terrainData.size.x);
        float randZ = Random.Range(position.z, position.z + terrainData.size.z);
        float yVal = terrain.SampleHeight(new Vector3(randX, 0, randZ)) + 1;
        return new Vector3(randX, yVal, randZ);
    }
}

public enum GameStage
{
    MainMenu = 1,
    Game,
    EndGameMenu
}