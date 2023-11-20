using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

public class Button
{
    private Texture2D _texture;
    private SpriteFont _spriteFont;
    private string _text;
    private Rectangle _buttonRectangle;

    public event Action clicked;

    public Button(Texture2D texture,SpriteFont spriteFont, string text, Rectangle buttonRectangle)
    {
        _texture = texture;
        _spriteFont = spriteFont;
        _text = text;
        _buttonRectangle = buttonRectangle;
    }

    public void OnPlayerClickedAtPosition(Vector2 position)
    {
        if (_buttonRectangle.Contains(position.X,position.Y))
            clicked?.Invoke();

    }
}