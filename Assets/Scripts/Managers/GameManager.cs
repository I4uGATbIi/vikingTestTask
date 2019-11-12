using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public delegate void GameStageHandler(GameStage changedStage, bool isGamePaused);

    public static event GameStageHandler StageChanged;

    [SerializeField] private GameStage currentStage;

    [SerializeField] private GameObject player;

    [SerializeField] private List<GameObject> monsters;

    [SerializeField] private Camera mainCam;

    [SerializeField] private UIManager UIManager;

    private int score;
    public int Score
    {
        get { return score; }
        private set
        {
            score = value;
            UIManager.UpdateScoreBar(score);
        }
    }

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
        StageChanged += GameManager_StageChanged;

        if (mainCam == null)
        {
            mainCam = Camera.main;
        }

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        player.GetComponent<PlayerControl>().stats.playerHPisZero += OnPlayerDies;
        player.GetComponent<PlayerControl>().stats.playerDamageTaken += OnPlayerDamageTaken;
        player.GetComponent<PlayerControl>().stats.playerHealingTaken += Stats_playerHealingTaken;

        if (UIManager == null)
        {
            UIManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        }

        InitMonsters();
    }

    void InitMonsters()
    {
        monsters.ForEach(monster =>
        {
            monster.GetComponent<MonsterContoll>().monsterStats.monsterHPisZero += MonsterStats_monsterHPisZero;
            monster.transform.position = GetSpawnPosition();
        });
    }

    private void MonsterStats_monsterHPisZero(UnitStats stats)
    {
        Score++;
    }

    private void Stats_playerHealingTaken(UnitStats stats)
    {
        UIManager.OnPlayerDamageTaken((PlayerStats)stats);
    }

    private void GameManager_StageChanged(GameStage changedStage, bool isGamePaused)
    {
        player.GetComponent<PlayerControl>().enabled = !isGamePaused;
        monsters.ForEach(monster => monster.GetComponent<MonsterContoll>().enabled = !isGamePaused);
        mainCam.GetComponent<ThirdPersonCamera>().enabled = !isGamePaused;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        CurrentStage = GameStage.MainMenu;
    }

    public void StartGame()
    {
        CurrentStage = GameStage.Game;
        Score = 0;
    }

    public void RestartGame()
    {
        player.GetComponent<PlayerControl>().stats.ResetStat();
        monsters.ForEach(monster => monster.GetComponent<MonsterContoll>().monsterStats.ResetStat());
        StartGame();
    }

    public void ExitGame()
    {
        Application.Quit(0);
    }

    public void OnPlayerDies(UnitStats stats)
    {
        CurrentStage = GameStage.EndGameMenu;
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

    private void OnPlayerDamageTaken(UnitStats stats)
    {
        UIManager.OnPlayerDamageTaken((PlayerStats)stats);
    }
}

public enum GameStage
{
    MainMenu = 1,
    Game,
    EndGameMenu
}