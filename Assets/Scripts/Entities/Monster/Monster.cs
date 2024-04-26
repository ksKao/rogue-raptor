using UnityEngine;

public class Monster : Entity
{
    protected override void Update()
    {
        base.Update();

        if (IsLockAction) return;

        // always face the player
        transform.LookAt(GameManager.Instance.Player.transform.position);

        // check player is in attack range, if yes, attack, else just chase after player
        if (Vector3.Distance(GameManager.Instance.Player.transform.position, transform.position) <= attackRange)
            Attack();
        else
            Move();
    }

    private void Move()
    {
        transform.position += speed * Time.deltaTime * transform.forward;

        animator.SetBool(IS_MOVING_ANIM_PARAM_BOOL, true);
    }
}
