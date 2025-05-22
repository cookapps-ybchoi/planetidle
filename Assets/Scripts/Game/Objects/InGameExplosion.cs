using UnityEngine;
using Game.ObjectPool;

public class InGameExplosion : PoolableObject
{
    public override void OnSpawn()
    {
        base.OnSpawn();
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
    }

    protected void OnParticleSystemStopped()
    {
        Finish();
    }   

    private void Finish()
    {
        AddressableManager.Instance.ReturnToPool(this);
    }
}
