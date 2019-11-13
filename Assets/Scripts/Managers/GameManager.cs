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

    [SerializeField] private float monsterRevivalTime = 3f;
    [SerializeField] private float monsterRevivalRadius = 30f;

    private int score;

    public int Score
    {
        get => score;
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
        StartCoroutine(ReviveMonster(stats.GameObjectBind));
    }

    private void Stats_playerHealingTaken(UnitStats stats)
    {
        UIManager.OnPlayerDamageTaken((PlayerStats) stats);
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
        player.GetComponent<PlayerControl>().ResetAnimVars();
        monsters.ForEach(monster =>
        {
            monster.GetComponent<MonsterContoll>().monsterStats.ResetStat();
            monster.GetComponent<MonsterContoll>().ResetAnimVars();
            monster.transform.position = GetSpawnPosition();
        });
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
        UIManager.OnPlayerDamageTaken((PlayerStats) stats);
    }

    IEnumerator ReviveMonster(GameObject monster)
    {
        yield return new WaitForSeconds(monster.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length +
                                        monsterRevivalTime);
        monster.transform.position = GetMonsterRevivalPosition();
        var monsterContol = monster.GetComponent<MonsterContoll>();
        monsterContol.monsterStats.ReviveStats();
        monsterContol.ResetAnimVars();
        monsterContol.enabled = true;
    }

    private Vector3 GetMonsterRevivalPosition()
    {
        Terrain terrain = GameObject.FindWithTag("Terrain").GetComponent<Terrain>();
        float randX = Random.Range(player.transform.position.x - monsterRevivalRadius,
            player.transform.position.x + monsterRevivalRadius);
        float randZ = Random.Range(player.transform.position.z - monsterRevivalRadius,
            player.transform.position.z + monsterRevivalRadius);
        return new Vector3(randX, terrain.SampleHeight(new Vector3(randX, 0, randZ)) + 1, randZ);
    }
}

public enum GameStage
{
    MainMenu = 1,
    Game,
    EndGameMenu
}