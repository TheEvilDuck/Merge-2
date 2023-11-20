using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


public class PlayerInput
{
    public event Action<Vector2> mouseClicked;
    public event Action<Vector2> mouseDown;
    public event Action<Vector2> mouseReleased;

    private MouseState _lastMouseState;

    public void HandleInput()
    {
        MouseState currentMouseState = Mouse.GetState();

        if (currentMouseState.LeftButton == ButtonState.Released && _lastMouseState.LeftButton == ButtonState.Pressed)
            mouseClicked?.Invoke(new Vector2(currentMouseState.X,currentMouseState.Y));

        _lastMouseState = currentMouseState;
    }
}