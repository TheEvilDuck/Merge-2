using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class TextPanel : VisualObject
{
    private SpriteFont _spriteFont;
    private string _text;

    public override Vector2 Position { get; protected set; }

    public TextPanel(SpriteFont spriteFont,string text, Vector2 position)
    {
        _spriteFont = spriteFont;
        _text = text;
        Position = position;
    }

    public override void MoveTo(Vector2 position)
    {
        Position = position;
    }

    public override void OnDrawStarted(SpriteBatch spriteBatch)
    {
        spriteBatch.DrawString(_spriteFont,_text,Position,Color.White);
    }
}