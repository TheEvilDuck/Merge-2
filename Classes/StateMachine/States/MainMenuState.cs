using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class MainMenuState : State
{
    private PlayerInput _playerInput;
    private Button _startButton;
    private SpriteBatch _spriteBatch;
    public MainMenuState(StateMachine stateMachine,SpriteBatch spriteBatch,GraphicsDevice graphicsDevice, PlayerInput playerInput,SpriteFont spriteFont) : base(stateMachine)
    {
        _playerInput = playerInput;
        _spriteBatch = spriteBatch;

        Texture2D buttonTexture = new Texture2D(graphicsDevice,1,1);
        buttonTexture.SetData<Color>(new Color [] { Color.Gray });
        _startButton = new Button(buttonTexture,new Rectangle(10,10,100,20),"Play",spriteFont);
        _startButton.clicked+=OnStartButtonClicked;
    }

    public override void Enter()
    {
        base.Enter();

        _playerInput.mouseClicked+=_startButton.OnPlayerClickedAtPosition;
    }
    public override void Exit()
    {
        base.Exit();

        _playerInput.mouseClicked-=_startButton.OnPlayerClickedAtPosition;
    }
    public override void Draw()
    {
        base.Draw();

        _startButton.Draw(_spriteBatch);
    }
    private void OnStartButtonClicked()
    {
        _stateMachine.ChangeState<CoreGameState>();
    }
}