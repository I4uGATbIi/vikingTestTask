using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterContoll : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private MonsterStats monsterStats;
    private Animator animator;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        monsterStats = new MonsterStats();
        animator = GetComponent<Animator>();
        monsterStats.monsterDamageTaken += MonsterStats_monsterDamageTaken;
        monsterStats.monsterHPisZero += MonsterStats_monsterHPisZero;
        GameManager.StageChanged += GameManager_StageChanged;
        navMeshAgent.speed = monsterStats.WalkSpeed;
    }

    void Update()
    {
        Move();
        AttackPlayer();
    }

    void Move()
    {
        navMeshAgent.SetDestination(GameObject.FindWithTag("Player").transform.position);
        animator.SetFloat("moveSpeed", navMeshAgent.velocity.magnitude);
    }

    void AttackPlayer()
    {
        animator.SetBool("isAttacking", navMeshAgent.remainingDistance <= 1.5);
    }

    private void GameManager_StageChanged(GameStage changedStage, bool isGamePaused)
    {
        enabled = !isGamePaused;
    }

    private void MonsterStats_monsterHPisZero(UnitStats stats)
    {
        throw new System.NotImplementedException();
    }

    private void MonsterStats_monsterDamageTaken(UnitStats stats)
    {
        throw new System.NotImplementedException();
    }
}
