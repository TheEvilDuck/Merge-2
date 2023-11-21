using System;

public class PlayerStats
{
    private int _points = 0;

    public event Action<int>pointsChanged;

    public void AddPoints(int amount)
    {
        if (amount<0)
            return;
        _points+=amount;
        pointsChanged.Invoke(_points);
    }
}