using System.Linq;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField] protected float speed = 3;
    [SerializeField] protected float attackSpeed = 1; // How many times this character can attack in one second
    [SerializeField] protected float attackRange = 1.5f;
    [SerializeField] protected int attackDamage;
    [SerializeField] protected int maxHp;

    // components
    protected HealthBar healthBar;
    protected Animator animator;
    protected Rigidbody rigidBody;

    // states
    private int currentHp;
    protected float lockActionDuration = 0;

    // animation parameters
    protected readonly int IS_MOVING_ANIM_PARAM_BOOL = Animator.StringToHash("IsMoving");
    protected readonly int DIE_ANIM_PARAM_TRIGGER = Animator.StringToHash("Die");
    protected readonly int HIT_ANIM_PARAM_TRIGGER = Animator.StringToHash("Hit");
    protected readonly int ATTACK_ANIM_PARAM_TRIGGER = Animator.StringToHash("Attack");
    protected readonly int ATTACK_RATE_ANIM_PARAM_FLOAT = Animator.StringToHash("AttackRate");
    protected readonly int IDLE_ANIM_PARAM_TRIGGER = Animator.StringToHash("Idle");

    protected bool IsLockAction => lockActionDuration > 0;
    protected string CurrentPlayingAnimation => animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
    protected int CurrentHp
    {
        get => currentHp;
        set
        {
            currentHp = Mathf.Clamp(value, 0, maxHp);
            healthBar.SetPercentage((float) currentHp / maxHp);
        }
    }

    public int AttackDamage => attackDamage;
    public float SecondPerAttack => 1 / attackSpeed;
    public float AttackRate
    {
        get
        {
            AnimationClip attackAnimation = animator.runtimeAnimatorController.animationClips.Where(x => x.name == "Attack").First();
            return attackSpeed * attackAnimation.length;
        }
    }

    protected virtual void Awake()
    {
        healthBar = GetComponentInChildren<HealthBar>();
        animator = GetComponentInChildren<Animator>();
        rigidBody = GetComponent<Rigidbody>();

        currentHp = maxHp;
    }

    protected virtual void Update()
    {
        if (IsLockAction)
        {
            lockActionDuration -= Time.deltaTime;

            // if this frame wants to unlock action, reset to idle state
            if (!IsLockAction)
                animator.SetTrigger(IDLE_ANIM_PARAM_TRIGGER);
        }
    }

    private void StopKnockback()
    {
        rigidBody.isKinematic = true;
    }

    private void Die()
    {
        if (CurrentPlayingAnimation == "Die") return;

        animator.SetTrigger(DIE_ANIM_PARAM_TRIGGER);
        healthBar.gameObject.SetActive(false);

        LockAction(2);
        Destroy(gameObject, 2f);
    }

    protected void LockAction(float duration)
    {
        // if duration is greater than lockActionDuration, means that the lock duration is longer, or some time has passed since last lock, in that case, refresh the lock action to new duration
        if (duration > lockActionDuration)
            lockActionDuration = duration;
    }

    protected virtual void Attack()
    {
        if (IsLockAction) return;

        LockAction(SecondPerAttack);

        // play animation
        animator.SetTrigger(ATTACK_ANIM_PARAM_TRIGGER);
        animator.SetFloat(ATTACK_RATE_ANIM_PARAM_FLOAT, AttackRate);
    }

    public virtual void TakeDamage(int damage, float knockbackDuration = 0.5f, float knockbackForce = 10)
    {
        if (!IsLockAction)
            animator.SetTrigger(HIT_ANIM_PARAM_TRIGGER);

        LockAction(knockbackDuration);

        CurrentHp -= damage;

        if (CurrentHp <= 0)
        {
            Die();
        }
        else if (knockbackForce > 0 && knockbackForce > 0)
        {
            rigidBody.isKinematic = false;
            rigidBody.AddForce(GameManager.Instance.Player.transform.forward * knockbackForce, ForceMode.Impulse);
            Invoke(nameof(StopKnockback), knockbackDuration);
        }

    }
}
