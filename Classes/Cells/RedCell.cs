using System;

public class RedCell : Cell
{
    private const double BLUE_CELL_GENERATE_CHANCE = 0.8f;
    public RedCell(int level) : base(level){}

    public override CellColor Color => CellColor.Red;

    public override Cell Generate()
    {
        Random random = new Random();

        if (random.NextDouble()<=BLUE_CELL_GENERATE_CHANCE)
            return new BlueCell(1);
        else
            return new GreenCell(1);
    }
}