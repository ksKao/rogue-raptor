using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private HealthBar playerHealthBar;

    public HealthBar PlayerHealthBar => playerHealthBar;
}
