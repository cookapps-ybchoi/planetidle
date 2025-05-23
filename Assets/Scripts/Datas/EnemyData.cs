
public enum EnemyType
{
    Normal,
    Boss
}

public class EnemyData
{
    public int EnemyId { get; private set; }
    public EnemyType EnemyType { get; private set; }
    public int EnemyLevel { get; private set; }
    public double Hp { get; private set; }
    public float MoveSpeed { get; private set; }
    public float AttackRange { get; private set; }
    public double AttackPower { get; private set; }
    public float AttackDelay { get; private set; }

    private double _baseHp;  // 기본 Hp 값을 저장할 필드 추가

    /// <summary>
    /// 적 데이터 생성자
    /// </summary>
    /// <param name="enemyId">적 아이디</param>
    /// <param name="enemyLevel">적 레벨</param>
    /// <param name="hp">적 체력</param>
    /// <param name="moveSpeed">적 이동속도</param>
    /// <param name="attackRange">적 공격범위</param>
    /// <param name="attackPower">적 공격력</param>
    /// <param name="attackDelay">적 공격속도</param>
    public EnemyData(int enemyId, EnemyType enemyType, int enemyLevel, double hp, float moveSpeed, float attackRange, double attackPower, float attackDelay)
    {
        EnemyId = enemyId;
        EnemyType = enemyType;
        MoveSpeed = moveSpeed;
        AttackRange = attackRange;
        AttackPower = attackPower;
        AttackDelay = attackDelay;

        _baseHp = hp;
        SetLevel(enemyLevel);
    }

    public EnemyData Copy()
    {
        return new EnemyData(EnemyId, EnemyType, EnemyLevel, Hp, MoveSpeed, AttackRange, AttackPower, AttackDelay);
    }

    //레벨당 체력 10% 증가
    public void SetLevel(int level)
    {
        EnemyLevel = level;
        Hp = _baseHp * (1 + (level - 1) * 0.1);  // 기본 Hp 값에서 시작
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
