using UnityEngine;

public class Monster : Entity
{
    // components
    private Rigidbody rigidBody;

    protected override void Awake()
    {
        base.Awake();
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // always face the player
        transform.LookAt(GameManager.Instance.Player.transform.position);

        // dont do anything if lock action is set to true, prevent monster to move while attacking
        if (lockAction) return;

        // check player is in attack range, if yes, attack, else just chase after player
        if (Vector3.Distance(GameManager.Instance.Player.transform.position, transform.position) <= attackRange)
            Attack();
        else
            Move();
    }

    private void Move()
    {
        transform.position += speed * Time.deltaTime * transform.forward;

        animationController.ChangeAnimationState(AnimationController.WALK_ANIMATION);
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
        rigidBody.AddForce(GameManager.Instance.Player.transform.forward * 10, ForceMode.Impulse);

        Invoke(nameof(StopKnockback), knockbackDuration);
    }
}
