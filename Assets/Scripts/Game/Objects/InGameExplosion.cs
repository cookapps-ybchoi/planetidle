using UnityEngine;

public class InGameExplosion : MonoBehaviour, InGameObject  
{
    public void Initialize()
    {
        
    }

    protected void OnParticleSystemStopped()
    {
        Finish();
    }   

    private void Finish()
    {
        ObjectPoolManager.Instance.ReturnToPool(gameObject.name, gameObject);
    }
}
