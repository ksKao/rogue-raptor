using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private Player player;
    private Camera mainCamera;

    public Player Player => player;
    public Camera MainCamera => mainCamera;

    protected override void Awake()
    {
        base.Awake();

        player = FindObjectOfType<Player>();
        mainCamera = Camera.main;
    }
}
