public enum EnemyType
{
    Normal,
    Elite,
    Boss
}

public class EnemyMetaData
{
    public int EnemyId { get; private set; }
    public EnemyType EnemyType { get; private set; }
    public double Hp { get; private set; }
    public double HpIncRate { get; private set; }
    public float MoveSpeed { get; private set; }
    public float AttackRange { get; private set; }
    public double AttackPower { get; private set; }
    public float AttackDelay { get; private set; }
    public int Point { get; private set; }

    public EnemyMetaData(int enemyId, EnemyType enemyType, double hp, double hpIncRate, float moveSpeed, float attackRange, double attackPower, float attackDelay, int point)
    {
        EnemyId = enemyId;
        EnemyType = enemyType;
        Hp = hp;
        HpIncRate = hpIncRate;
        MoveSpeed = moveSpeed;
        AttackRange = attackRange;
        AttackPower = attackPower;
        AttackDelay = attackDelay;
        Point = point;
    }
}

public class EnemyData
{
    public EnemyMetaData MetaData { get; private set; }
    public int Level { get; private set; }
    public double Hp { get; private set; }
    public double MaxHp => MetaData.Hp * (1 + (Level - 1) * MetaData.HpIncRate);
    public float MoveSpeed => MetaData.MoveSpeed;
    public float AttackRange => MetaData.AttackRange;
    public double AttackPower => MetaData.AttackPower;
    public float AttackDelay => MetaData.AttackDelay;
    public int Point => MetaData.Point;

    public EnemyData(EnemyMetaData metaData, int level)
    {
        MetaData = metaData;
        Level = level;
        Hp = MaxHp;
    }

    public EnemyData Copy()
    {
        return new EnemyData(MetaData, Level);
    }

    public void ChangeHp(double value)
    {
        Hp += value;
        if (Hp < 0)
        {
            Hp = 0;
        }
    }

    
}
