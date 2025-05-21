

public class EnemyData
{
    public int EnemyId { get; private set; }
    public int EnemyLevel { get; private set; }
    public int Hp { get; private set; }
    public float MoveSpeed { get; private set; }
    public float AttackRange { get; private set; }
    public float AttackPower { get; private set; }
    public float AttackSpeed { get; private set; }

    /// <summary>
    /// 적 데이터 생성자
    /// </summary>
    /// <param name="enemyId">적 아이디</param>
    /// <param name="enemyLevel">적 레벨</param>
    /// <param name="hp">적 체력</param>
    /// <param name="moveSpeed">적 이동속도</param>
    /// <param name="attackRange">적 공격범위</param>
    /// <param name="attackPower">적 공격력</param>
    /// <param name="attackSpeed">적 공격속도</param>
    public EnemyData(int enemyId, int enemyLevel, int hp, float moveSpeed, float attackRange, float attackPower, float attackSpeed)
    {
        EnemyId = enemyId;
        EnemyLevel = enemyLevel;
        Hp = hp;
        MoveSpeed = moveSpeed;
        AttackRange = attackRange;
        AttackPower = attackPower;
        AttackSpeed = attackSpeed;
    }
}

