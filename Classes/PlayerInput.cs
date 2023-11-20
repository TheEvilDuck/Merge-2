using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


public class PlayerInput
{
    public event Action<Vector2> mouseClicked;
    public event Action<Vector2> mouseDown;
    public event Action<Vector2> mouseReleased;

    private bool _clicked = false;

    public void HandleInput()
    {
        MouseState currentMouseState = Mouse.GetState();

        if (currentMouseState.LeftButton == ButtonState.Pressed && !_clicked)
        {
            mouseClicked?.Invoke(new Vector2(currentMouseState.X,currentMouseState.Y));
            _clicked = true;
        }
        
        if (currentMouseState.LeftButton == ButtonState.Pressed)
            mouseDown?.Invoke(new Vector2(currentMouseState.X,currentMouseState.Y));

        if (currentMouseState.LeftButton == ButtonState.Released)
        {
            mouseReleased?.Invoke(new Vector2(currentMouseState.X,currentMouseState.Y));
            _clicked = false;
        }

    }
}