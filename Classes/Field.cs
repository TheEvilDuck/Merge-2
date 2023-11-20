using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Field
{
    private int _size;
    private Cell[,] _cells;

    public event Action<int,int,int> cellAdded;
    public event Action<int,int> cellRemoved;

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

    public bool CombineTwoCells(int x1,int y1,int x2,int y2)
    {
        if (x1==x2&&y1==y2)
            return false;

        if (x1<0||x2<0||y1<0||y2<0
        ||x1>=_size||x2>=_size
        ||y1>=_size||y2>=_size)
            return false;
        
        if (_cells[x2,y2]==null&&_cells[x1,y1]!=null)
        {
            _cells[x2,y2] = _cells[x1,y1];
            cellAdded?.Invoke(x2,y2, _cells[x2,y2].Level);
            _cells[x1,y1] = null;
            cellRemoved?.Invoke(x1,y1);
            return true;
        }
        if (_cells[x1,y1]!=null&&_cells[x2,y2]!=null)
        {
            if (_cells[x1,y1].Level==_cells[x2,y2].Level
            && _cells[x1,y1].Color==_cells[x2,y2].Color)
            {
                _cells[x2,y2].LevelUp();
                _cells[x1,y1] = null;

                cellRemoved?.Invoke(x1,y1);
                cellRemoved?.Invoke(x2,y2);

                cellAdded?.Invoke(x2,y2, _cells[x2,y2].Level);

                return true;
            }
            cellRemoved?.Invoke(x1,y1);
            cellRemoved?.Invoke(x2,y2);

            Cell buff = _cells[x2,y2];
            _cells[x2,y2] = _cells[x1,y1];
            _cells[x1,y1] = buff;

            cellAdded?.Invoke(x1,y1, _cells[x1,y1].Level);
            cellAdded?.Invoke(x2,y2, _cells[x2,y2].Level);

            return true;
        }
        return false;
    }

    public void RemoveCellAt(int x,int y)
    {
        if (x<0||y<0||x>=_size||y>=_size)
            return;
        
        _cells[x,y] = null;
        cellRemoved?.Invoke(x,y);
    }
    private void GenerateCellContentAt(int x,int y)
    {
        _cells[x,y] = new Cell(CellColor.Blue,1);
        cellAdded?.Invoke(x,y, 1);
    }
}