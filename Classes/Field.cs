using Microsoft.Xna.Framework.Graphics;

public class Field
{
    private byte _size;
    private Cell[,] _cells;

    public Field(byte size)
    {
        _size = size;
        _cells = new Cell[_size,_size];
    }
    public void GenerateField()
    {
        for (int x = 0;x<_size;x++)
        {
            for (int y = 0;y<_size;y++)
            {
                _cells[x,y] = new Cell(CellColor.Blue,1);
            }
        }
    }
}