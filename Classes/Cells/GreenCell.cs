using System;

public class GreenCell : Cell
{
    private const double RED_CELL_GENERATE_CHANCE = 0.8f;
    public GreenCell(int level) : base(level){}

    public override CellColor Color => CellColor.Green;

    public override Cell Generate()
    {
        Random random = new Random();

        if (random.NextDouble()<=RED_CELL_GENERATE_CHANCE)
            return new RedCell(1);
        else
            return new BlueCell(1);
    }
}