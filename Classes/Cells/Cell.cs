public abstract class Cell
{
    private const int CELL_MAX_LEVEL = 4;

    public int Level {get; private set;}
    public abstract CellColor Color {get;}
    public bool CanGenerate {get; private set;}

    public Cell(int level)
    {
        Level = level;

        if (Level<=0)
            Level = 1;

        if (Level>=CELL_MAX_LEVEL)
        {
            Level = CELL_MAX_LEVEL;
            CanGenerate = true;
        }
    }

    public bool LevelUp()
    {
        if (Level>=CELL_MAX_LEVEL)
            return false;

        Level++;

        if (Level==CELL_MAX_LEVEL)
            CanGenerate = true;

        return true;
    }

    public abstract Cell Generate();
}