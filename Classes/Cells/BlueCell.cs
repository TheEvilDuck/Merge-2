using System;

public class BlueCell : Cell
{
    private const double GREEN_CELL_GENERATE_CHANCE = 0.8f;
    public BlueCell(int level) : base( level){}

    public override CellColor Color => CellColor.Blue;

    public override Cell Generate()
    {
        Random random = new Random();

        if (random.NextDouble()<=GREEN_CELL_GENERATE_CHANCE)
            return new GreenCell(1);
        else
            return new RedCell(1);
    }
}