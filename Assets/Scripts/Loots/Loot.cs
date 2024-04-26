using UnityEngine;

public abstract class Loot : MonoBehaviour
{
    private void Start()
    {
        LeanTween.moveY(gameObject, 0.2f, 1).setLoopPingPong();
        LeanTween.rotateAround(gameObject, Vector3.up, 360, 5).setLoopClamp();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            OnPickUp(player);
            Destroy(gameObject);
        }
    }

    protected abstract void OnPickUp(Player player);
}
