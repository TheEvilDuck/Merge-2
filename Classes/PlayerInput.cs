using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


public class PlayerInput
{
    private const int MILISECONDS_GAP_FOR_DOUBLE_CLICK = 200;
    public event Action<Vector2> mouseClicked;
    public event Action<Vector2> mouseDown;
    public event Action<Vector2> mouseReleased;
    public event Action<Vector2> mouseDoubleClicked;

    private bool _clicked = false;
    private bool _doubleClicked = false;
    private int _mouseClickReleasedTime = 0;

    public void HandleInput(GameTime gameTime)
    {
        MouseState currentMouseState = Mouse.GetState();
        Vector2 mousePosition = new Vector2(currentMouseState.X,currentMouseState.Y);

        if (currentMouseState.LeftButton == ButtonState.Pressed)
        {
            mouseDown?.Invoke(mousePosition);

            if (!_clicked)
            {
                _clicked = true;

                if (_mouseClickReleasedTime>0)
                {
                    _clicked = false;
                    _doubleClicked = true;
                    _mouseClickReleasedTime = 0;
                }
                else
                    mouseClicked?.Invoke(mousePosition);
            }

            if (_mouseClickReleasedTime<=0)
                _mouseClickReleasedTime = MILISECONDS_GAP_FOR_DOUBLE_CLICK;
        }

        if (currentMouseState.LeftButton == ButtonState.Released)
        {
            mouseReleased?.Invoke(mousePosition);

            if (!_clicked&&_doubleClicked)
            {
                mouseDoubleClicked?.Invoke(mousePosition);
                Console.WriteLine("DOUBLE CLICK");
            }
            
            _clicked = false;
            _doubleClicked = false;
        }

        if (_mouseClickReleasedTime>0)
        {
            _mouseClickReleasedTime-=gameTime.ElapsedGameTime.Milliseconds;

            if (_mouseClickReleasedTime<=0)
            {
                _mouseClickReleasedTime = 0;
            }
        }
    }
}