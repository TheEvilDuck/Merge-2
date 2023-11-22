using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class OrderPanel: VisualObject
{
    private const int ORDER_VISUAL_SIZE = 30;
    private const int ORDER_SPACING = 5;

    public event Action<int>certainOrderClicked;
    
    private Button[]_orderButtons;
    private Texture2D _buttonTexture;
    private SpriteFont _buttonSpriteFont;
    private PlayerInput _playerInput;

    public OrderPanel(int ordersAvailableInMoment, Texture2D buttonTexture, SpriteFont buttonSpriteFont, PlayerInput playerInput)
    {
        _orderButtons = new Button[ordersAvailableInMoment];

        _buttonTexture = buttonTexture;
        _buttonSpriteFont = buttonSpriteFont;
        _playerInput = playerInput;
    }

    public override Vector2 Position { get; protected set; }

    public override void MoveTo(Vector2 position)
    {
        Position = position;

        foreach (Button button in _orderButtons)
        {

            if (button!=null)
            {
                button.MoveTo(button.Position+position);
            }

        }

    }

    public override void OnDrawStarted(SpriteBatch spriteBatch)
    {
        foreach (Button button in _orderButtons)
        {
            button?.OnDrawStarted(spriteBatch);
        }
    }

    public void OnOrderGenerated(Order order)
    {
        for (int i = 0;i<_orderButtons.Length;i++)
        {

            if (_orderButtons[i]==null)
            {
                Rectangle rectangle = new Rectangle(
                    (int)Position.X+i*(ORDER_VISUAL_SIZE+ORDER_SPACING),
                    (int)Position.Y,
                    ORDER_VISUAL_SIZE,
                    ORDER_VISUAL_SIZE);
                
                Button button = new Button(_buttonTexture,rectangle,order.Level.ToString(),_buttonSpriteFont);
                button.SetColor(FieldVisuals.CellColors.GetValueOrDefault(order.Color));

                int iBuff = i;

                button.clicked+=() =>
                {
                    certainOrderClicked.Invoke(iBuff);
                };

                _orderButtons[i] = button;

                _playerInput.mouseClicked+=button.OnPlayerClickedAtPosition;

                return;
            }

        }
    }

    public void UnSubFromInput()
    {
        foreach(Button button in _orderButtons)
        {
            if (button!=null)
                _playerInput.mouseClicked-=button.OnPlayerClickedAtPosition;
                
        }
    }
    public void OnOrderRemovedAt(int i)
    {
        if (i<0||i>=_orderButtons.Length)
            return;

        if (_orderButtons[i]!=null)
            _playerInput.mouseClicked-=_orderButtons[i].OnPlayerClickedAtPosition;
            
        _orderButtons[i] = null;
    }
}