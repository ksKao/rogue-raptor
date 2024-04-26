public class GameManager : Singleton<GameManager>
{
    private Player player;

    public Player Player => player;

    protected override void Awake()
    {
        base.Awake();

        player = FindObjectOfType<Player>();
    }
}
