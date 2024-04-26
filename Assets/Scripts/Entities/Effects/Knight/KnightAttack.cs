public class KnightAttack : Effect
{
    protected override void OnCollide(Entity entity)
    {
        Monster monster = entity as Monster;

        monster.OnHit();
    }
}
