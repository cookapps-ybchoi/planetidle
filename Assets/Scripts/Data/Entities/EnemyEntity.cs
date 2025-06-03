using System;

[Serializable]
public class EnemyEntity
{
    public int id;
    public EnemyType type;
    public double hp;
    public double hpIncRate;
    public float moveSpeed;
    public float attackRange;
    public double attackPower;
    public float attackDelay;
    public int point;

    // public EnemyEntity(int enemyId, EnemyType enemyType, double hp, double hpIncRate, float moveSpeed, float attackRange, double attackPower, float attackDelay, int point)
    // {
    //     id = enemyId;
    //     type = enemyType;
    //     this.hp = hp;
    //     this.hpIncRate = hpIncRate;
    //     this.moveSpeed = moveSpeed;
    //     this.attackRange = attackRange;
    //     this.attackPower = attackPower;
    //     this.attackDelay = attackDelay;
    //     this.point = point;
    // }
}