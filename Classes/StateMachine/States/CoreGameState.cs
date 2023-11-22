using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class CoreGameState : State
{
    private const byte FIELD_SIZE = 8;
    private const int ORDERS_TO_GAME_OVER = 10;
    private const int ORDERS_AVAILABLE_IN_MOMENT = 2;

    private Field _field;
    private FieldVisuals _fieldVisuals;
    private SpriteBatch _spriteBatch;
    private GraphicsDevice _graphicsDevice;
    private OrderPanel _orderPanel;
    private TextPanel _pointsText;
    private TextPanel _ordersLeftPanel;
    private Button _backButton;

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
        _pointsText = new TextPanel(_spriteFont,"Points: 0",new Vector2(_graphicsDevice.Viewport.Width/2,0));
        _ordersLeftPanel = new TextPanel(_spriteFont,$"Orders left: {ORDERS_TO_GAME_OVER}",new Vector2(0,50));

        Rectangle backButtonRectangle = new Rectangle(_graphicsDevice.Viewport.Width-50,0,50,20);
        _backButton = new Button(buttonTexture,backButtonRectangle,"Back",_spriteFont);
        _backButton.SetColor(Color.Gray);
        _backButton.clicked+=OnBackButtonClicked;

        PlayerStats.pointsChanged+=(int amount)=>
        {
            _pointsText.ChangeText($"Points: {amount}");
        };

        _ordersHandler.orderGenerated+=_orderPanel.OnOrderGenerated;
        _ordersHandler.allOrdersCompleted+=OnAllOrdersCompleted;
        _playerInput.mouseClicked+=_backButton.OnPlayerClickedAtPosition;

        _orderPanel.certainOrderClicked+=(int i) =>
        {
            int points = _ordersHandler.CheckOrderCompleteAndReturnPoints(i,_field);
            PlayerStats.AddPoints(points);
        };

        _ordersHandler.orderRemovedAt+=_orderPanel.OnOrderRemovedAt;
        _ordersHandler.orderRemovedAt+=(int index) =>
        {
            _ordersLeftPanel.ChangeText($"Orders left: {_ordersHandler.OrdersLeft}");
        };

        _ordersHandler.GenerateOrder();
        _ordersHandler.GenerateOrder();

        _field.GenerateField(8);

    }
    public override void Draw()
    {
        base.Draw();

        _fieldVisuals.Draw();
        _orderPanel.Draw(_spriteBatch);
        _pointsText.Draw(_spriteBatch);
        _backButton.Draw(_spriteBatch);
        _ordersLeftPanel.Draw(_spriteBatch);
    }
    public override void Exit()
    {
        base.Exit();

        _fieldVisuals.UnSubFromInput();
        _orderPanel.UnSubFromInput();
        _playerInput.mouseClicked-=_backButton.OnPlayerClickedAtPosition;
    }
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (_fieldVisuals!=null)
        {
            _fieldVisuals?.Animator.Update(gameTime);
        }
    }
    private void OnAllOrdersCompleted()
    {
        _stateMachine.ChangeState<GameOverState>();
    }

    private void OnBackButtonClicked()
    {
        _stateMachine.ChangeState<MainMenuState>();
    }
}