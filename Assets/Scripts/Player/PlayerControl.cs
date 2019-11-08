using System.Collections;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private PlayerStats stats;

    public float turnSmoothTime = 0.2f;
    float turnSmoothVelocity;

    public float speedSmoothTime = 0.1f;
    float speedSmoothVelocity;
    float currentSpeed;

    Animator animator;
    Transform cameraT;

    private GameManager GM;

    void Awake()
    {
        GM = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        animator = GetComponent<Animator>();
        cameraT = Camera.main.transform;
        stats = new PlayerStats();

        GameManager.StageChanged += OnStageChanged;
        PlayerStats.playerDamageTaken += OnDamageTaken;
        PlayerStats.playerHPisZero += Die;
    }

    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            StartCoroutine(Attack());
        }
#if UNITY_EDITOR
        if (Input.GetButton("Fire2"))
        {
            StartCoroutine(PlayDamageAnim());
        }
#endif
        Move();
    }

    private void Die(UnitStats stats)
    {
        animator.SetBool("isDead", true);
    }

    private void OnStageChanged(GameStage stage, bool isGamePaused)
    {
        enabled = !isGamePaused;
    }

    IEnumerator Attack()
    {
        animator.SetBool("isAttacking", true);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        animator.SetBool("isAttacking", false);
    }

    void OnDamageTaken(UnitStats stats)
    {
        StartCoroutine(PlayDamageAnim());
    }

    IEnumerator PlayDamageAnim()
    {
        animator.SetBool("isDamageTaken", true);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        animator.SetBool("isDamageTaken", false);
    }

    void Move()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 inputDir = input.normalized;

        if (inputDir != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation,
                                        ref turnSmoothVelocity, turnSmoothTime);
        }

        bool running = Input.GetKey(KeyCode.LeftShift);
        float targetSpeed = ((running) ? stats.RunSpeed : stats.WalkSpeed) * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

        animator.SetFloat("moveSpeed", inputDir == Vector2.zero ? 0 : running ? 1 : 0.5f);
        transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);
    }
}