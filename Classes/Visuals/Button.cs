using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

public class Button: Panel
{
    private TextPanel _textPanel;

    public event Action clicked;
    public event Action doubleClicked;

    public Button(Texture2D texture, Rectangle rectangle, string text, SpriteFont spriteFont) : base(texture, rectangle)
    {
        _textPanel = new TextPanel(spriteFont,text, Position);
    }

    public void OnPlayerClickedAtPosition(Vector2 position)
    {
        if (Rectangle.Contains(position.X,position.Y))
            clicked?.Invoke();
    }
    public void OnPlayerDoubleClickedAtPosition(Vector2 position)
    {
        if (Rectangle.Contains(position.X,position.Y))
            doubleClicked?.Invoke();
    }

    public override void OnDrawStarted(SpriteBatch spriteBatch)
    {
        base.OnDrawStarted(spriteBatch);
        _textPanel.OnDrawStarted(spriteBatch);
    }
    public override void MoveTo(Vector2 position)
    {
        base.MoveTo(position);
        _textPanel.MoveTo(position);
    }
}