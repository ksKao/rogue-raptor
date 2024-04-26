using System.Collections.Generic;
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

    protected override void Die()
    {
        base.Die();

        if (GameManager.Instance.Random.NextDouble() > 0)
        {
            List<Loot> allLoots = GameManager.Instance.AllLoots;
            Loot randomLoot = allLoots[GameManager.Instance.Random.Next(allLoots.Count)];

            randomLoot = Instantiate(randomLoot);
            randomLoot.transform.position = transform.position;
        }
    }
}
