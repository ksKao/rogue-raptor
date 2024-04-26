using UnityEngine;

public class Coin : Loot
{
    protected override void OnPickUp(Player player)
    {
        Debug.Log("Picked up coin");
    }
}
