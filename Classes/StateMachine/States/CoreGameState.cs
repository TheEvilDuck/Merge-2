using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class CoreGameState : State
{
    private const byte FIELD_SIZE = 8;
    private const int ORDERS_TO_GAME_OVER = 1;
    private const int ORDERS_AVAILABLE_IN_MOMENT = 2;

    private Field _field;
    private FieldVisuals _fieldVisuals;
    private SpriteBatch _spriteBatch;
    private GraphicsDevice _graphicsDevice;
    private OrderPanel _orderPanel;

    private SpriteFont _spriteFont;
    private PlayerInput _playerInput;
    private OrdersHandler _ordersHandler;

    public PlayerStats PlayerStats {get; private set;}

    public CoreGameState(
        StateMachine stateMachine, 
        SpriteBatch spriteBatch,
        GraphicsDevice graphicsDevice,
        SpriteFont spriteFont,
        PlayerInput playerInput) : base(stateMachine)
    {
        _spriteBatch = spriteBatch;
        _graphicsDevice = graphicsDevice;
        _spriteFont = spriteFont;
        _playerInput = playerInput;

        

    }
    public override void Enter()
    {
        base.Enter();

        _field = new Field(FIELD_SIZE);

        _fieldVisuals = new FieldVisuals(
            _spriteBatch,
            _graphicsDevice,
            FIELD_SIZE,
            _field,
            _spriteFont,
            _playerInput,
            _graphicsDevice.Viewport.Width/2,
            _graphicsDevice.Viewport.Height/2);

        PlayerStats = new PlayerStats();
        _ordersHandler = new OrdersHandler(ORDERS_AVAILABLE_IN_MOMENT,ORDERS_TO_GAME_OVER);

        Texture2D buttonTexture = new Texture2D(_graphicsDevice,1,1);
        buttonTexture.SetData<Color>(new Color [] { Color.White });

        _orderPanel = new OrderPanel(ORDERS_AVAILABLE_IN_MOMENT,buttonTexture,_spriteFont,_playerInput);

        _ordersHandler.orderGenerated+=_orderPanel.OnOrderGenerated;
        _ordersHandler.allOrdersCompleted+=OnAllOrdersCompleted;

        _orderPanel.certainOrderClicked+=(int i) =>
        {
            _ordersHandler.CheckOrderComplete(i,_field);
        };

        _ordersHandler.orderRemovedAt+=_orderPanel.OnOrderRemovedAt;

        _ordersHandler.GenerateOrder();
        _ordersHandler.GenerateOrder();

        _field.GenerateField(8);

    }
    public override void Draw()
    {
        base.Draw();

        _fieldVisuals.Draw();
        _orderPanel.Draw(_spriteBatch);
    }
    public override void Exit()
    {
        base.Exit();

        _fieldVisuals.UnSubFromInput();
        _orderPanel.UnSubFromInput();
    }

    private void OnAllOrdersCompleted()
    {
        _stateMachine.ChangeState<GameOverState>();
    }
}