public class Cell
{
    private CellColor _color;
    private int _level;

    public int Level => _level;
    public CellColor Color => _color;

    public Cell(CellColor color,int level)
    {
        _color = color;
        _level = level;
    }
}