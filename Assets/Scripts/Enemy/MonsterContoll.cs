using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterContoll : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public MonsterStats monsterStats { get; private set; }
    private Animator animator;

    [SerializeField]
    private GameObject weapon;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        monsterStats = new MonsterStats(gameObject);
        animator = GetComponent<Animator>();
        monsterStats.monsterDamageTaken += MonsterStats_monsterDamageTaken;
        monsterStats.monsterHPisZero += MonsterStats_monsterHPisZero;
        navMeshAgent.speed = monsterStats.WalkSpeed;
        weapon.GetComponent<WeaponAttack>().weaponCollision += OnWeaponCollision;
        ResetAnimVars();
    }

    private void OnWeaponCollision(Collider other)
    {
        if (!other.CompareTag("Player") || !animator.GetBool("isAttacking") )
        {
            return;
        }

        other.gameObject.GetComponent<PlayerControl>().GetDamage(monsterStats.Damage);
        Debug.Log($"{other.name} suffered {monsterStats.Damage} from {name}");
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
        {
            transform.LookAt(GameObject.FindWithTag("Player").transform);
            animator.SetTrigger("isAttacking");
        }
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
    
    public void ResetAnimVars()
    {
        animator.SetBool("isAttacking", false);
        animator.SetBool("isDamaged", false);
        animator.SetBool("isDead", false);
        animator.SetFloat("moveSpeed", 0f);
    }
}