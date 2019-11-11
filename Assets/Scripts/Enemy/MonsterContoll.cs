using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterContoll : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private MonsterStats monsterStats;
    private Animator animator;

    [SerializeField]
    private GameObject weapon;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        monsterStats = new MonsterStats();
        animator = GetComponent<Animator>();
        monsterStats.monsterDamageTaken += MonsterStats_monsterDamageTaken;
        monsterStats.monsterHPisZero += MonsterStats_monsterHPisZero;
        GameManager.StageChanged += GameManager_StageChanged;
        navMeshAgent.speed = monsterStats.WalkSpeed;
        weapon.GetComponent<WeaponAttack>().weaponCollision += OnWeaponCollision;
    }

    private void OnWeaponCollision(Collider other)
    {
        if (!other.CompareTag("Player") || !animator.GetBool("isAttacking"))
        {
            return;
        }

        other.gameObject.GetComponent<PlayerControl>().GetDamage(monsterStats.Damage);
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

    public void GetDamage(float damage)
    {
        monsterStats.TakeDamage(damage);
    }

    void AttackPlayer()
    {
        if (Vector3.Distance(transform.position, GameObject.FindWithTag("Player").transform.position) <=
            navMeshAgent.stoppingDistance)
            animator.SetTrigger("isAttacking");
    }

    private void GameManager_StageChanged(GameStage changedStage, bool isGamePaused)
    {
        enabled = !isGamePaused;
    }

    private void MonsterStats_monsterHPisZero(UnitStats stats)
    {
        enabled = false;
        animator.SetBool("isDead", true);
    }

    private void MonsterStats_monsterDamageTaken(UnitStats stats)
    {
        StartCoroutine(PlayDamageTaken());
    }

    IEnumerator PlayDamageTaken()
    {
        animator.SetBool("isDamaged", true);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        animator.SetBool("isDamaged", false);
    }
}