public class KnightAttack : Effect
{
    protected override void OnCollide(Entity entity)
    {
        Monster monster = entity as Monster;

        monster.TakeDamage(GameManager.Instance.Player.AttackDamage, source);
    }
}
