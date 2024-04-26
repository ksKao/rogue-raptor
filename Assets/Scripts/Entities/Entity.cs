using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField] protected float speed = 3;
    [SerializeField] protected float attackSpeed = 1; // How many times this character can attack in one second
    [SerializeField] protected float attackRange = 1.5f;

    protected AnimationController animationController;
    protected bool lockAction = false; // if true, character cannot perform any action, used for preventing user spam clicking to perform multiple consecutive action without cooldown

    protected virtual void Awake()
    {
        animationController = new AnimationController(GetComponentInChildren<Animator>());
    }

    protected virtual void Attack()
    {
        if (lockAction) return;

        lockAction = true;

        // calculate animation speed, use 1 / x because thats how many attack the player can perform in a second
        float secondPerAttack = 1 / attackSpeed;

        // play animation
        animationController.ChangeAnimationState(AnimationController.ATTACK_ANIMATION, secondPerAttack);

        // unlock action after attack animation finished
        Invoke(nameof(UnlockAction), secondPerAttack);
    }

    protected void UnlockAction()
    {
        lockAction = false;
    }
}
