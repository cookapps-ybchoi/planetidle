using UnityEngine;
using System;

public partial class InGameManager
{
    private int _totalPoints = 0;
    private int _maxPoints = 0;
    private int _currentLevel = 1;

    //포인트 레벨 별 획득 포인트
    private int[] _pointPerLevel = new int[Constants.INGAME_MAX_LEVEL]
    {
        0, 10, 30, 60, 100, 150, 210, 280, 360, 450, 550, 660, 780, 910, 1050, 1200, 1360, 1530, 1710, 1900
    };

    //포인트가 추기되면 현재 레벨이 증가하는지 확인
    public void AddPoints(int points)
    {
        _totalPoints += points;
        _maxPoints = _pointPerLevel[_currentLevel];
        int lastMaxPoints = _pointPerLevel[_currentLevel - 1];
        int _currentPoints = _totalPoints - lastMaxPoints;
        InGameEventManager.Instance.InvokePointsChanged(_currentPoints, _maxPoints);

        if (_totalPoints >= _maxPoints)
        {
            _currentLevel++;
            InGameEventManager.Instance.InvokeLevelChanged(_currentLevel);
        }
    }
}