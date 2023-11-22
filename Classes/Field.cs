using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

public class Field
{
    private int _size;
    private Cell[,] _cells;

    public event Action<int,int,int,CellColor> cellAdded;
    public event Action<int,int,int,int>cellsSwitched;
    public event Action<int,int> cellRemoved;

    public Field(int size)
    {
        _size = size;
        _cells = new Cell[_size,_size];
    }
    public void GenerateField(int startCellContentCount)
    {
        for (int i = 0;i<startCellContentCount;i++)
        {
            int x,y;
            if (TryGetRandomPosition(out x,out y))
            {
                _cells[x,y] = new BlueCell(1);
                cellAdded.Invoke(x,y,_cells[x,y].Level,_cells[x,y].Color);
            }
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
            cellAdded?.Invoke(x2,y2, _cells[x2,y2].Level,_cells[x2,y2].Color);
            _cells[x1,y1] = null;
            cellRemoved?.Invoke(x1,y1);
            return true;
        }
        if (_cells[x1,y1]!=null&&_cells[x2,y2]!=null)
        {
            if (_cells[x1,y1].Level==_cells[x2,y2].Level
            && _cells[x1,y1].Color==_cells[x2,y2].Color)
            {
                if (_cells[x2,y2].LevelUp())
                {
                    _cells[x1,y1] = null;

                    cellRemoved?.Invoke(x1,y1);
                    cellRemoved?.Invoke(x2,y2);

                    cellAdded?.Invoke(x2,y2, _cells[x2,y2].Level,_cells[x2,y2].Color);

                    return true;
                }
            }

            /*
            NO ANIMATION METHOD^
            -----------------------------
            cellRemoved?.Invoke(x1,y1);
            cellRemoved?.Invoke(x2,y2);

            Cell buff = _cells[x2,y2];
            _cells[x2,y2] = _cells[x1,y1];
            _cells[x1,y1] = buff;

            cellAdded?.Invoke(x1,y1, _cells[x1,y1].Level,_cells[x1,y1].Color);
            cellAdded?.Invoke(x2,y2, _cells[x2,y2].Level,_cells[x2,y2].Color);

            */

            Cell buff = _cells[x2,y2];
            _cells[x2,y2] = _cells[x1,y1];
            _cells[x1,y1] = buff;

            cellsSwitched.Invoke(x1,y1,x2,y2);

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

    public Cell GetCellAtPosition(int x, int y)
    {
        if (x<0||y<0||x>=_size||y>=_size)
            return null;
        return _cells[x,y];
    }

    public bool TryGenerateNewCellFrom(int x, int y)
    {
        if (_cells[x,y]==null)
            return false;
        
        if (!_cells[x,y].CanGenerate)
            return false;

        int xPos,yPos;

        if (TryGetRandomPosition(out xPos,out yPos))
        {
            _cells[xPos,yPos] = _cells[x,y].Generate();
            cellAdded.Invoke(xPos,yPos,_cells[xPos,yPos].Level,_cells[xPos,yPos].Color);

            return true;
        }

        return false;
    }

    public bool TryFindCell(int level, CellColor cellColor, out int xFound, out int yFound)
    {
        for (int x = 0;x<_size;x++)
        {
            for (int y = 0;y<_size;y++)
            {
                if (_cells[x,y]!=null)
                {
                    if (_cells[x,y].Level==level&&_cells[x,y].Color==cellColor)
                    {
                        xFound = x;
                        yFound = y;
                        return true;
                    }
                }
            }
        }
        xFound = -1;
        yFound = -1;
        return false;
    }

    private bool TryGetRandomPosition(out int newX, out int newY)
    {
        List<Vector2>freeCells = new List<Vector2>();

        for (int x = 0;x< _size;x++)
        {
            for (int y = 0;y<_size;y++)
            {
                if (_cells[x,y]==null)
                {
                    freeCells.Add(new Vector2(x,y));
                }
            }
        }

        newX = -1;
        newY = -1;

        if (freeCells.Count==0)
            return false;

        Random random = new Random();

        Vector2 randomPos = freeCells[random.Next(0,freeCells.Count-1)];

        newX = (int)randomPos.X;
        newY = (int)randomPos.Y;

        return true;
    }
}