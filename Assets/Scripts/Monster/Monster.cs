using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] private float speed = 3;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackSpeed = 1; // How many times this unit can attack in one second

    // components
    private AnimationController animationController;

    // states
    private bool lockAction = false;

    private void Awake()
    {
        animationController = new AnimationController(GetComponentInChildren<Animator>());
    }

    private void Update()
    {
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
        transform.LookAt(Player.Instance.transform.position);

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
}
