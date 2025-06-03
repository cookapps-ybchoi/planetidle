public enum EnemyType
{
    Normal,
    Elite,
    Boss
}

public class EnemyData
{
    public EnemyEntity Entity { get; private set; }
    public int Level { get; private set; }
    public double CurHp { get; private set; }
    public double MaxHp => Entity.hp * (1 + (Level - 1) * Entity.hpIncRate);
    public float MoveSpeed => Entity.moveSpeed;
    public float AttackRange => Entity.attackRange;
    public double AttackPower => Entity.attackPower;
    public float AttackDelay => Entity.attackDelay;
    public int Point => Entity.point;

    public EnemyData(EnemyEntity metaData, int level)
    {
        Entity = metaData;
        Level = level;
        CurHp = MaxHp;
    }

    public EnemyData Copy()
    {
        return new EnemyData(Entity, Level);
    }

    public void ChangeHp(double value)
    {
        CurHp += value;
        if (CurHp < 0)
        {
            CurHp = 0;
        }
    }

    
}
