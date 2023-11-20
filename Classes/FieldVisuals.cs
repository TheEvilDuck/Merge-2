using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class FieldVisuals
{
    private const int CELL_VISUAL_SIZE = 30;
    private const float CELL_CONTENT_SIZE_PERCENT = 0.9F;
    private const int CELL_SPACING = 4;

    private SpriteBatch _spriteBatch;
    private Texture2D _cellTexture;
    private Texture2D _buttonTexture;
    private SpriteFont _buttonSpriteFont;
    private byte _fieldSize;
    private PlayerInput _playerInput;

    private Dictionary<Vector2,Button>_visualCells;

    private Button _holdCell;

    public FieldVisuals(SpriteBatch spriteBatch,GraphicsDevice graphicsDevice,byte fieldSize,Field field, SpriteFont spriteFont,PlayerInput playerInput)
    {
        _spriteBatch = spriteBatch;
        _cellTexture = new Texture2D(graphicsDevice,1,1);
        _cellTexture.SetData<Color>(new Color [] { Color.Gray });
        _buttonTexture = new Texture2D(graphicsDevice,1,1);
        _buttonTexture.SetData<Color>(new Color [] { Color.Blue });
        _fieldSize = fieldSize;
        _buttonSpriteFont = spriteFont;
        _playerInput = playerInput;

        _visualCells = new Dictionary<Vector2, Button>();
        field.cellAdded+=OnCellAdded;

        _playerInput.mouseDown+=HoldCell;
        _playerInput.mouseReleased+=DropCell;
    }
    public void Draw()
    {
        _spriteBatch.Begin();
        for (byte x = 0;x<_fieldSize;x++)
        {
            for (byte y = 0;y<_fieldSize;y++)
            {
                Rectangle cellVisual = new Rectangle(x*(CELL_VISUAL_SIZE+CELL_SPACING),y*(CELL_VISUAL_SIZE+CELL_SPACING),CELL_VISUAL_SIZE,CELL_VISUAL_SIZE);
                _spriteBatch.Draw(_cellTexture,cellVisual,Color.White);
            }
        }
        _spriteBatch.End();
        foreach (KeyValuePair<Vector2,Button>button in _visualCells)
        {
            
            if (button.Value!=_holdCell)
                button.Value.Draw(_spriteBatch);
        }

        

    }

    public void SubToInput()
    {
        foreach (KeyValuePair<Vector2,Button>button in _visualCells)
        {
            _playerInput.mouseClicked += button.Value.OnPlayerClickedAtPosition;
        }
    }
    public void UnSubToInput()
    {
        foreach (KeyValuePair<Vector2,Button>button in _visualCells)
        {
            _playerInput.mouseClicked -= button.Value.OnPlayerClickedAtPosition;
        }
    }
    private void HoldCell(Vector2 position)
    {
        if (_holdCell!=null)
        {
            _holdCell.Draw(_spriteBatch);
            Console.WriteLine(position);
        }
        
    }
    private void DropCell(Vector2 position)
    {
        _holdCell = null;
    }
    
    private void OnCellAdded(Vector2 atPosition)
    {
        Rectangle rectangle = new Rectangle((int)atPosition.X*(CELL_VISUAL_SIZE+CELL_SPACING),(int)atPosition.Y*(CELL_VISUAL_SIZE+CELL_SPACING),CELL_VISUAL_SIZE,CELL_VISUAL_SIZE);
        Button button = new Button(_buttonTexture,_buttonSpriteFont,"1",rectangle);
        _visualCells.Add(atPosition,button);

        button.clicked+=() =>
        {
            _holdCell = button;
            Console.WriteLine("A");
        };
    }

}