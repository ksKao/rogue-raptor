public class SkeletonMinion : Monster
{
    // effects
    private SkeletonAttack skeletonAttack;

    protected override void Awake()
    {
        base.Awake();

        skeletonAttack = GetComponentInChildren<SkeletonAttack>();
        skeletonAttack.damage = attackDamage;
    }

    protected override void Attack()
    {
        if (IsLockAction) return;

        base.Attack();

        skeletonAttack.Activate(this);
    }
}
