using UnityEngine;
using System;

public partial class InGameManager
{
    private int _totalExp = 0;
    private int _maxExp = 0;
    private int _currentLevel = 1;

    //레벨별 경험치 계산
    /*
    레벨별 필요 경험치 (10개씩):
    0-9:    0, 10, 25, 45, 70, 100, 135, 175, 220, 270
    10-19:  325, 385, 450, 520, 595, 675, 760, 850, 945, 1045
    20-29:  1150, 1260, 1375, 1495, 1620, 1750, 1885, 2025, 2170, 2320
    30-39:  2475, 2635, 2800, 2970, 3145, 3325, 3510, 3700, 3895, 4095
    40-49:  4300, 4510, 4725, 4945, 5170, 5400, 5635, 5875, 6120, 6370
    50-60:  6625, 6885, 7150, 7420, 7695, 7975, 8260, 8550, 8845, 9145, 9450
    */
    private static readonly int[] EXP_TABLE = new int[]
    {
        0, 10, 25, 45, 70, 100, 135, 175, 220, 270,
        325, 385, 450, 520, 595, 675, 760, 850, 945, 1045,
        1150, 1260, 1375, 1495, 1620, 1750, 1885, 2025, 2170, 2320,
        2475, 2635, 2800, 2970, 3145, 3325, 3510, 3700, 3895, 4095,
        4300, 4510, 4725, 4945, 5170, 5400, 5635, 5875, 6120, 6370,
        6625, 6885, 7150, 7420, 7695, 7975, 8260, 8550, 8845, 9145, 9450
    };

    /*
    경험치 계산 수식:
    - level 0: 0
    - level 1: 10
    - level 2 이상: 
        int exp = 10;
        int increment = 15;
        for (int i = 2; i <= level; i++) {
            exp += increment;
            increment += 5;
        }
    */

    private int GetExpForLevel(int level)
    {
        if (level < 0 || level >= EXP_TABLE.Length) return 0;
        return EXP_TABLE[level];
    }

    //포인트가 추기되면 현재 레벨이 증가하는지 확인
    public void AddExp(int exp)
    {
        _totalExp += exp;
        _maxExp = GetExpForLevel(_currentLevel);
        int lastMaxExp = GetExpForLevel(_currentLevel - 1);
        InGameEventHandler.InvokeExpChanged(_totalExp - lastMaxExp, _maxExp - lastMaxExp);

        if (_totalExp >= _maxExp)
        {
            _currentLevel++;
            InGameEventHandler.InvokeLevelChanged(_currentLevel);
            PauseGame();
        }
    }

    private void ResetExp()
    {
        _totalExp = 0;
        _currentLevel = 1;
        _maxExp = GetExpForLevel(_currentLevel);
        InGameEventHandler.InvokeLevelChanged(_currentLevel);
    }
}