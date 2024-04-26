using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField] protected float speed = 3;
    [SerializeField] protected float attackSpeed = 1; // How many times this character can attack in one second
    [SerializeField] protected float attackRange = 1.5f;
    [SerializeField] protected int attackDamage;
    [SerializeField] protected int maxHp;

    // components
    protected Animator animator;
    protected Rigidbody rigidBody;

    // states
    protected int currentHp;
    protected bool isDead = false;
    protected bool lockAction = false; // if true, character cannot perform any action, used for preventing user spam clicking to perform multiple consecutive action without cooldown

    // animation parameters
    protected readonly int IS_MOVING_ANIM_PARAM_BOOL = Animator.StringToHash("IsMoving");
    protected readonly int DIE_ANIM_PARAM_TRIGGER = Animator.StringToHash("Die");
    protected readonly int HIT_ANIM_PARAM_TRIGGER = Animator.StringToHash("Hit");
    protected readonly int ATTACK_ANIM_PARAM_TRIGGER = Animator.StringToHash("Attack");
    protected readonly int ATTACK_RATE_ANIM_PARAM_FLOAT = Animator.StringToHash("AttackRate");
    protected readonly int IDLE_ANIM_PARAM_TRIGGER = Animator.StringToHash("Idle");

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
        animator = GetComponentInChildren<Animator>();
        rigidBody = GetComponent<Rigidbody>();

        currentHp = maxHp;
    }

    private void StopKnockback()
    {
        UnlockAction();
        rigidBody.isKinematic = true;
        animator.SetTrigger(IDLE_ANIM_PARAM_TRIGGER);
    }

    private void Die()
    {
        lockAction = true;
        isDead = true;

        animator.SetTrigger(DIE_ANIM_PARAM_TRIGGER);

        Invoke(nameof(DestroyThis), 2f);
    }

    private void DestroyThis()
    {
        Destroy(gameObject);
    }

    protected virtual void Attack()
    {
        if (lockAction) return;

        lockAction = true;

        // play animation
        animator.SetTrigger(ATTACK_ANIM_PARAM_TRIGGER);
        animator.SetFloat(ATTACK_RATE_ANIM_PARAM_FLOAT, AttackRate);

        // unlock action after attack animation finished
        Invoke(nameof(UnlockAction), SecondPerAttack);
    }

    protected void UnlockAction()
    {
        lockAction = false;
        animator.SetTrigger(IDLE_ANIM_PARAM_TRIGGER);
    }

    public virtual void TakeDamage(int damage, float knockbackDuration = 0.5f, float knockbackForce = 10)
    {
        lockAction = true;
        animator.SetTrigger(HIT_ANIM_PARAM_TRIGGER);

        currentHp -= damage;

        if (currentHp <= 0)
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
