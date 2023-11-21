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
    private Color _color = Color.White;

    public event Action clicked;
    public event Action doubleClicked;

    public Vector2 Position => new Vector2(_buttonRectangle.X,_buttonRectangle.Y);

    public Button(Texture2D texture,SpriteFont spriteFont, string text, Rectangle buttonRectangle)
    {
        _texture = texture;
        _spriteFont = spriteFont;
        _text = text;
        _buttonRectangle = buttonRectangle;
    }

    public void SetColor(Color color)
    {
        _color = color;
    }
    public void OnPlayerClickedAtPosition(Vector2 position)
    {
        if (_buttonRectangle.Contains(position.X,position.Y))
            clicked?.Invoke();
    }
    public void OnPlayerDoubleClickedAtPosition(Vector2 position)
    {
        if (_buttonRectangle.Contains(position.X,position.Y))
            doubleClicked?.Invoke();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.Draw(_texture,_buttonRectangle,_color);

        if (_spriteFont!=null)
            spriteBatch.DrawString(_spriteFont,_text,new Vector2(_buttonRectangle.X,_buttonRectangle.Y),Color.White);
        
        spriteBatch.End();
    }
    public void MoveTo(Vector2 position)
    {
        _buttonRectangle.X = (int)position.X;
        _buttonRectangle.Y = (int)position.Y;
    }
}