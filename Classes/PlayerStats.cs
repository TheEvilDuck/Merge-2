using System;

public class PlayerStats
{   public event Action<int>pointsChanged;

    public int Points {get; private set;}

    public void AddPoints(int amount)
    {
        if (amount<=0)
            return;
        
        Points+=amount;
        pointsChanged.Invoke(Points);
    }
}