public class SkeletonAttack : Effect
{
    protected override void OnCollide(Entity entity)
    {
        Player player = (Player)entity;
        player.TakeDamage(damage, source);
    }
}
