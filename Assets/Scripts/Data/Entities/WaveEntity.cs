using System;

[Serializable]
public class WaveEntity
{
    public int id;               //웨이브 아이디
    public int level;            //웨이브 레벨
    public int spawnCount;       //동시 생산 수
    public float spawnInterval;  //생산 간격
    public int[] enemyIds;       //생산 아이디
    public float[] spawnRates;   //생산 확률
}