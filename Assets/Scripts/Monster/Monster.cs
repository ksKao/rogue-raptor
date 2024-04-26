using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] private float speed = 3;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackSpeed = 1; // How many times this unit can attack in one second

    // components
    private AnimationController animationController;
    private Rigidbody rigidBody;

    // states
    private bool lockAction = false;

    private void Awake()
    {
        animationController = new AnimationController(GetComponentInChildren<Animator>());
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // always face the player
        transform.LookAt(Player.Instance.transform.position);

        // dont do anything if lock action is set to true, prevent monster to move while attacking
        if (lockAction) return;

        // check player is in attack range, if yes, attack, else just chase after player
        if (Vector3.Distance(Player.Instance.transform.position, transform.position) <= attackRange)
            Attack();
        else
            Move();
    }

    private void Move()
    {
        transform.position += speed * Time.deltaTime * transform.forward;

        animationController.ChangeAnimationState(AnimationController.WALK_ANIMATION);
    }

    private void Attack()
    {
        if (lockAction) return;

        lockAction = true;
        animationController.ChangeAnimationState(AnimationController.ATTACK_ANIMATION, 1 / attackSpeed);

        Invoke(nameof(UnlockAction), attackSpeed);
    }

    private void UnlockAction()
    {
        lockAction = false;
    }

    private void StopKnockback()
    {
        UnlockAction();
        rigidBody.isKinematic = true;
        animationController.ChangeAnimationState(AnimationController.WALK_ANIMATION);
    }

    public void OnHit()
    {
        if (animationController.CurrentAnimationState == AnimationController.HIT_ANIMATION) return;

        lockAction = true;
        float knockbackDuration = 0.5f;
        animationController.ChangeAnimationState(AnimationController.HIT_ANIMATION, knockbackDuration);

        rigidBody.isKinematic = false;
        rigidBody.AddForce(Player.Instance.transform.forward * 10, ForceMode.Impulse);

        Invoke(nameof(StopKnockback), knockbackDuration);
    }
}
