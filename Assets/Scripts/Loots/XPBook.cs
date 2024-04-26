using UnityEngine;

public class XPBook : Loot
{
    protected override void OnPickUp(Player player)
    {
        Debug.Log("Pick up XP book");
    }
}
