using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class FieldVisuals
{
    private const int CELL_VISUAL_SIZE = 30;
    private const float CELL_CONTENT_SIZE_PERCENT = 0.9F;
    private const int CELL_SPACING = 4;

    public Func<int,int,int,int,bool> cellDropped;
    public Func<int,int,bool> cellDoubleClicked;

    private readonly Dictionary<CellColor,Color>_cellColors = new Dictionary<CellColor, Color>
    {
        {CellColor.Green,Color.Green},
        {CellColor.Red,Color.Red},
        {CellColor.Blue,Color.Blue}

    };
    private SpriteBatch _spriteBatch;
    private Texture2D _cellTexture;
    private Texture2D _buttonTexture;
    private SpriteFont _buttonSpriteFont;
    private byte _fieldSize;
    private int _xOffset;
    private int _yOffset;
    private PlayerInput _playerInput;

    private Dictionary<Button,Vector2>_visualCells;
    private Button _holdCell;


    public FieldVisuals(SpriteBatch spriteBatch,GraphicsDevice graphicsDevice,byte fieldSize,Field field, SpriteFont spriteFont,PlayerInput playerInput,int xOffset,int yOffset)
    {
        _spriteBatch = spriteBatch;
        _cellTexture = new Texture2D(graphicsDevice,1,1);
        _cellTexture.SetData<Color>(new Color [] { Color.Gray });
        _buttonTexture = new Texture2D(graphicsDevice,1,1);
        _buttonTexture.SetData<Color>(new Color [] { Color.White });
        _fieldSize = fieldSize;
        _buttonSpriteFont = spriteFont;
        _playerInput = playerInput;
        _yOffset = yOffset-fieldSize*(CELL_VISUAL_SIZE+CELL_SPACING)/2;
        _xOffset = xOffset-fieldSize*(CELL_VISUAL_SIZE+CELL_SPACING)/2;

        _visualCells = new Dictionary<Button,Vector2>();
        field.cellAdded+=OnCellAdded;
        field.cellRemoved+=OnCellRemoved;

        _playerInput.mouseDown+=HoldCell;
        _playerInput.mouseReleased+=DropCell;

        cellDropped+=field.CombineTwoCells;
        cellDoubleClicked+=field.TryGenerateNewCellFrom;
    }
    public void Draw()
    {
        _spriteBatch.Begin();
        
        for (byte x = 0;x<_fieldSize;x++)
        {
            for (byte y = 0;y<_fieldSize;y++)
            {
                Rectangle cellVisual = new Rectangle(x*(CELL_VISUAL_SIZE+CELL_SPACING)+_xOffset,y*(CELL_VISUAL_SIZE+CELL_SPACING)+_yOffset,CELL_VISUAL_SIZE,CELL_VISUAL_SIZE);
                _spriteBatch.Draw(_cellTexture,cellVisual,Color.White);
            }
        }

        _spriteBatch.End();

        foreach (KeyValuePair<Button,Vector2>button in _visualCells)
        {
            
            if (button.Key!=_holdCell)
                button.Key.Draw(_spriteBatch);
        }

        if (_holdCell!=null)
            _holdCell.Draw(_spriteBatch);
    }
    public void UnSubFromInput()
    {
        foreach (KeyValuePair<Button,Vector2>button in _visualCells)
        {
            _playerInput.mouseClicked -= button.Key.OnPlayerClickedAtPosition;
            _playerInput.mouseDoubleClicked-=button.Key.OnPlayerDoubleClickedAtPosition;
        }
    }
    private void HoldCell(Vector2 atPosition)
    {
        if (_holdCell!=null)
        {
            _holdCell.MoveTo(atPosition-new Vector2(CELL_VISUAL_SIZE/2f,CELL_VISUAL_SIZE/2f));
        }
        
    }
    private void DropCell(Vector2 atPosition)
    {
        if (_holdCell==null)
            return;
        Vector2 positionWithoutOffset = atPosition- new Vector2(_xOffset,_yOffset);
        if (_visualCells.TryGetValue(_holdCell,out Vector2 holdCellIndexes))
        {
            int x1 = (int)holdCellIndexes.X;
            int y1 = (int)holdCellIndexes.Y;

            int x2 = (int)(positionWithoutOffset.X/(CELL_VISUAL_SIZE+CELL_SPACING));
            int y2 = (int)(positionWithoutOffset.Y/(CELL_VISUAL_SIZE+CELL_SPACING));
        
            bool success = cellDropped.Invoke(x1,y1,x2,y2);

            if (!success)
            {
                _holdCell.MoveTo(ConvertIndexesToPosition(x1,y1));
            }
        }
        _holdCell = null;
    }
    
    private void OnCellAdded(int xIndex, int yIndex, int level, CellColor cellColor)
    {
        Rectangle rectangle = new Rectangle(
            xIndex*(CELL_VISUAL_SIZE+CELL_SPACING)+_xOffset,
            yIndex*(CELL_VISUAL_SIZE+CELL_SPACING)+_yOffset,
            CELL_VISUAL_SIZE,
            CELL_VISUAL_SIZE);
        
        Button button = new Button(_buttonTexture,_buttonSpriteFont,level.ToString(),rectangle);
        button.SetColor(_cellColors.GetValueOrDefault(cellColor));
        _visualCells.Add(button,new Vector2(xIndex,yIndex));

        _playerInput.mouseClicked += button.OnPlayerClickedAtPosition;
        _playerInput.mouseDoubleClicked+=button.OnPlayerDoubleClickedAtPosition;

        Console.WriteLine($"Added new visuals in {new Vector2(xIndex,yIndex)}");

        button.clicked+=() =>
        {
            _holdCell = button;
        };

        button.doubleClicked+=() =>
        {
            if (_visualCells.ContainsKey(button))
            {
                Vector2 indexes = _visualCells[button];

                bool success = cellDoubleClicked.Invoke((int)indexes.X,(int)indexes.Y);
                Console.WriteLine(success);
            }
        };

    }

    private void OnCellRemoved(int xIndex,int yIndex)
    {
        foreach (KeyValuePair<Button,Vector2>keyValuePair in _visualCells)
        {
            if (keyValuePair.Value.X==xIndex&&keyValuePair.Value.Y==yIndex)
            {
                _playerInput.mouseClicked -= keyValuePair.Key.OnPlayerClickedAtPosition;
                _playerInput.mouseDoubleClicked -= keyValuePair.Key.OnPlayerDoubleClickedAtPosition;
                _visualCells.Remove(keyValuePair.Key);
                Console.WriteLine($"Removed at {keyValuePair.Value} in visuals");
            }
        }
    }

    private Vector2 ConvertIndexesToPosition(int xIndex,int yIndex)
    {
        return new Vector2(
            xIndex*(CELL_VISUAL_SIZE+CELL_SPACING)+_xOffset,
            yIndex*(CELL_VISUAL_SIZE+CELL_SPACING)+_yOffset
        );
    }

}