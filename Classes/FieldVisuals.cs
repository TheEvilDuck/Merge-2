using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class FieldVisuals
{
    private const int CELL_VISUAL_SIZE = 30;
    private const float CELL_CONTENT_SIZE_PERCENT = 0.9F;
    private const int CELL_SPACING = 4;

    private SpriteBatch _spriteBatch;
    private Texture2D _cellTexture;
    private byte _fieldSize;

    public FieldVisuals(SpriteBatch spriteBatch,GraphicsDevice graphicsDevice,byte fieldSize)
    {
        _spriteBatch = spriteBatch;
        _cellTexture = new Texture2D(graphicsDevice,1,1);
        _cellTexture.SetData<Color>(new Color [] { Color.Gray });
        _fieldSize = fieldSize;
    }
    public void Draw()
    {
        for (byte x = 0;x<_fieldSize;x++)
        {
            for (byte y = 0;y<_fieldSize;y++)
            {
                Rectangle cellVisual = new Rectangle(x*(CELL_VISUAL_SIZE+CELL_SPACING),y*(CELL_VISUAL_SIZE+CELL_SPACING),CELL_VISUAL_SIZE,CELL_VISUAL_SIZE);
                _spriteBatch.Draw(_cellTexture,cellVisual,Color.White);
            }
        }
    }


}