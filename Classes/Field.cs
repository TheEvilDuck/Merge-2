using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Field
{
    private int _size;
    private Cell[,] _cells;

    public event Action<Vector2> cellAdded;

    public Field(int size)
    {
        _size = size;
        _cells = new Cell[_size,_size];
    }
    public void GenerateField(int startCellContentCount)
    {
        List<int>xNotUsedYet = new List<int>();
        List<int>yNotUsedYet = new List<int>();

        for (int i =0;i<_size;i++)
        {
            xNotUsedYet.Add(i);
            yNotUsedYet.Add(i);
        }

        Random random = new Random();

        for (int cellCount = 0;cellCount<startCellContentCount;cellCount++)
        {
            int xId = random.Next(0,xNotUsedYet.Count-1);
            int yId = random.Next(0,yNotUsedYet.Count-1);

            GenerateCellContentAt(xNotUsedYet[xId],yNotUsedYet[yId]);

            xNotUsedYet.RemoveAt(xId);
            yNotUsedYet.RemoveAt(yId);
        }
    }
    private void GenerateCellContentAt(int x,int y)
    {
        _cells[x,y] = new Cell(CellColor.Blue,1);
        cellAdded?.Invoke(new Vector2(x,y));
    }
}