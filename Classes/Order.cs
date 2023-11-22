using System;

public class Order
{
    public int Level {get; private set;}
    public CellColor Color {get; private set;}
    public Order()
    {
        var colors = Enum.GetValues<CellColor>();
        Random random = new Random();
        Level = random.Next(1,5);
        Color = colors[random.Next(0,colors.Length)];
    }
}