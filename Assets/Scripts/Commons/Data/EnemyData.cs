public enum EnemyType
{
    Normal,
    Boss
}

public class EnemyMetaData
{
    public int EnemyId { get; private set; }
    public EnemyType EnemyType { get; private set; }
    public double Hp { get; private set; }
    public float MoveSpeed { get; private set; }
    public float AttackRange { get; private set; }
    public double AttackPower { get; private set; }
    public float AttackDelay { get; private set; }
    public int Point { get; private set; }
    public int PointPerLevel { get; private set; }

    public EnemyMetaData(int enemyId, EnemyType enemyType, double hp, float moveSpeed, float attackRange, double attackPower, float attackDelay, int point, int pointPerLevel)
    {
        EnemyId = enemyId;
        EnemyType = enemyType;
        Hp = hp;
        MoveSpeed = moveSpeed;
        AttackRange = attackRange;
        AttackPower = attackPower;
        AttackDelay = attackDelay;
        Point = point;
        PointPerLevel = pointPerLevel;
    }
}

public class EnemyData
{
    public EnemyMetaData MetaData { get; private set; }
    public int Level { get; private set; }
    public double Hp { get; private set; }
    public double MaxHp => MetaData.Hp * (1 + (Level - 1) * 0.1);
    public float MoveSpeed => MetaData.MoveSpeed;
    public float AttackRange => MetaData.AttackRange;
    public double AttackPower => MetaData.AttackPower;
    public float AttackDelay => MetaData.AttackDelay;
    public int Point => MetaData.Point + (Level - 1) * MetaData.PointPerLevel;

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
