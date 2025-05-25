using UnityEngine;
using System;

public partial class InGameManager
{
    private int _currentPoints = 0;
    public int CurrentPoints => _currentPoints;

    public void AddPoints(int points)
    {
        _currentPoints += points;
        InGameEventManager.Instance.InvokePointsChanged(points, _currentPoints);
    }

    public bool TrySpendPoints(int points)
    {
        if (_currentPoints < points) return false;
        
        _currentPoints -= points;
        InGameEventManager.Instance.InvokePointsChanged(-points, _currentPoints);
        return true;
    }
} 