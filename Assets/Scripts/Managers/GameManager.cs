using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private List<Loot> allLoots;

    private Player player;
    private Camera mainCamera;
    private System.Random random = new();

    public Player Player => player;
    public Camera MainCamera => mainCamera;
    public List<Loot> AllLoots => allLoots;
    public System.Random Random => random;
    

    protected override void Awake()
    {
        base.Awake();

        player = FindObjectOfType<Player>();
        mainCamera = Camera.main;
    }
}
