using UnityEngine;

public class HealthPotion : Loot
{
    [SerializeField] public int HpIncrease = 5;

    protected override void OnPickUp(Player player)
    {
        GameManager.Instance.Player.CurrentHp += HpIncrease;
    }
}
