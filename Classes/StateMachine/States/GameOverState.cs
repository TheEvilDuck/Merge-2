using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class GameOverState : State
{
    private PlayerInput _playerInput;
    private Button _mainMenuButton;
    private TextPanel _FinaleScore;
    private SpriteBatch _spriteBatch;
    public GameOverState(StateMachine stateMachine,SpriteBatch spriteBatch,GraphicsDevice graphicsDevice, PlayerInput playerInput,SpriteFont spriteFont, PlayerStats playerStats) : base(stateMachine)
    {
        _playerInput = playerInput;
        _spriteBatch = spriteBatch;

        Texture2D buttonTexture = new Texture2D(graphicsDevice,1,1);
        buttonTexture.SetData<Color>(new Color [] { Color.Gray });
        _mainMenuButton = new Button(buttonTexture,new Rectangle(10,10,100,20),"Main menu",spriteFont);
        _mainMenuButton.clicked+=OnMenuButtonClicked;
    }

    public override void Enter()
    {
        base.Enter();

        _playerInput.mouseClicked+=_mainMenuButton.OnPlayerClickedAtPosition;
    }

    public override void Draw()
    {
        base.Draw();

        _mainMenuButton.Draw(_spriteBatch);
    }

    public override void Exit()
    {
        base.Exit();

        _playerInput.mouseClicked-=_mainMenuButton.OnPlayerClickedAtPosition;
    }

    public void OnMenuButtonClicked()
    {
        _stateMachine.ChangeState<MainMenuState>();
    }
}