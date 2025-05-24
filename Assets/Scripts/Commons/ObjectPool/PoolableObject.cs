using UnityEngine;

namespace Game.ObjectPool
{
    public class PoolableObject : MonoBehaviour, IPoolableObject
    {
        public virtual void OnCreated()
        {
            gameObject.SetActive(false);
        }

        public virtual void OnSpawn()
        {
            gameObject.SetActive(true);
        }

        public virtual void OnDespawn()
        {
            gameObject.SetActive(false);
        }
    }
}
