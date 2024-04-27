using System;
using UnityEngine;
using UnityEngine.VFX;

public abstract class Effect : MonoBehaviour
{
    private enum TriggerTarget
    {
        PLAYER,
        MONSTER
    }

    [SerializeField] private TriggerTarget triggerTarget = TriggerTarget.MONSTER;

    private VisualEffect vfx;
    private new Collider collider;

    protected Entity source;

    [NonSerialized] public int damage;

    protected abstract void OnCollide(Entity entity);

    private void Awake()
    {
        vfx = GetComponentInChildren<VisualEffect>();
        collider = GetComponent<Collider>();

        collider.enabled = false;
    }

    private void Deactivate()
    {
        collider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggerTarget == TriggerTarget.PLAYER)
        {
            if (other.TryGetComponent(out Player player))
                OnCollide(player);
        }
        else
        {
            if (other.TryGetComponent(out Monster monster))
                OnCollide(monster);
        }
    }

    public void Activate(Entity source, float playRate = 1, float activeTime = 1)
    {
        collider.enabled = true;
        this.source = source;

        if (vfx != null)
        {
            vfx.Play();
            vfx.playRate = playRate;
        }

        Invoke(nameof(Deactivate), activeTime);
    }
}
