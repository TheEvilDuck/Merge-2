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
    private const float ANIMATIONS_TIME_SECONDS = 2f;
    private const int GENERATED_ELEMENT_Y_OFFEST = 25;

    public Func<int,int,int,int,bool> cellDropped;
    public Func<int,int,bool> cellDoubleClicked;

    public static readonly Dictionary<CellColor,Color>CellColors = new Dictionary<CellColor, Color>
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

    public Animator Animator {get;private set;}


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
        field.cellsSwitched+=OnToCellsSwitched;
        field.cellGenerated+=OnCellGenerated;

        _playerInput.mouseDown+=HoldCell;
        _playerInput.mouseReleased+=DropCell;

        cellDropped+=field.CombineTwoCells;
        cellDoubleClicked+=field.TryGenerateNewCellFrom;

        Animator = new Animator();
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
        
        Button button = new Button(_buttonTexture,rectangle,level.ToString(),_buttonSpriteFont);
        button.SetColor(CellColors.GetValueOrDefault(cellColor));
        _visualCells.Add(button,new Vector2(xIndex,yIndex));

        _playerInput.mouseClicked += button.OnPlayerClickedAtPosition;
        _playerInput.mouseDoubleClicked+=button.OnPlayerDoubleClickedAtPosition;

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
            }
        };

    }

    private void OnToCellsSwitched(int x1,int y1,int x2,int y2)
    {
        Button button_1 = null;
        Button button_2 = null;
        foreach (KeyValuePair<Button,Vector2>keyValuePair in _visualCells)
        {
            if (keyValuePair.Value.X==x1&&keyValuePair.Value.Y==y1)
                button_1 = keyValuePair.Key;

            if (keyValuePair.Value.X==x2&&keyValuePair.Value.Y==y2)
                button_2 = keyValuePair.Key;
        }

        if (button_1!=null&&button_2!=null&&button_1!=button_2)
        {

            MovingAnimation animation = new MovingAnimation(button_2,ConvertIndexesToPosition(x1,y1),ANIMATIONS_TIME_SECONDS);

            button_1.MoveTo(ConvertIndexesToPosition(x2,y2));

            Animator.AddAnimation(animation);

            animation.Play();

            _playerInput.mouseClicked-=button_2.OnPlayerClickedAtPosition;

            animation.completed+= () =>
            {
                _playerInput.mouseClicked+=button_2.OnPlayerClickedAtPosition;
            };



            _visualCells.Remove(button_1);
            _visualCells.Remove(button_2);

            _visualCells.Add(button_1, new Vector2(x2,y2));
            _visualCells.Add(button_2, new Vector2(x1,y1));
        }
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

    private void OnCellGenerated(int xFrom, int yFrom, int xTo, int yTo, int level, CellColor color)
    {
        Rectangle rectangle = new Rectangle(
            xFrom*(CELL_VISUAL_SIZE+CELL_SPACING)+_xOffset,
            yFrom*(CELL_VISUAL_SIZE+CELL_SPACING)+_yOffset+GENERATED_ELEMENT_Y_OFFEST,
            CELL_VISUAL_SIZE,
            CELL_VISUAL_SIZE);
        
        Button button = new Button(_buttonTexture,rectangle,level.ToString(),_buttonSpriteFont);
        button.SetColor(CellColors.GetValueOrDefault(color));

        MovingAnimation animation = new MovingAnimation(button,ConvertIndexesToPosition(xTo,yTo),ANIMATIONS_TIME_SECONDS);
        Animator.AddAnimation(animation);
        animation.Play();

        _visualCells.Add(button,new Vector2(xTo,yTo));

        animation.completed+= () =>
        {
            _playerInput.mouseClicked += button.OnPlayerClickedAtPosition;
            _playerInput.mouseDoubleClicked+=button.OnPlayerDoubleClickedAtPosition;
        };

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
            }
        };
    }

}