namespace Game.ObjectPool
{
    public interface IPoolableObject
    {
        public void OnCreated();
        public void OnSpawn();
        public void OnDespawn();
    }
}
