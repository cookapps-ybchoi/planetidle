using UnityEngine;
using System;

public partial class InGameManager
{
    private int _totalExp = 0;
    private int _maxExp = 0;
    private int _currentLevel = 1;

    //포인트 레벨 별 획득 포인트
    private int[] _expPerLevel = new int[Constants.INGAME_MAX_LEVEL]
    {
        0, 5, 10, 15, 20, 150, 210, 280, 360, 450, 550, 660, 780, 910, 1050, 1200, 1360, 1530, 1710, 1900
    };

    //포인트가 추기되면 현재 레벨이 증가하는지 확인
    public void AddExp(int exp)
    {
        _totalExp += exp;
        _maxExp = _expPerLevel[_currentLevel];
        int lastMaxExp = _expPerLevel[_currentLevel - 1];
        int currentExp = _totalExp - lastMaxExp;
        InGameEventManager.Instance.InvokeExpChanged(currentExp, _maxExp);

        if (_totalExp >= _maxExp)
        {
            _currentLevel++;
            InGameEventManager.Instance.InvokeLevelChanged(_currentLevel);
            PauseGame();
        }
    }

    private void ResetExp()
    {
        _totalExp = 0;
        _currentLevel = 1;
        _maxExp = _expPerLevel[_currentLevel];
    }
}