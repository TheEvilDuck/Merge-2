using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class GameOverState : State
{
    private PlayerInput _playerInput;
    private Button _mainMenuButton;
    private TextPanel _finaleScoreText;
    private SpriteBatch _spriteBatch;
    private CoreGameState _coreGameState;
    public GameOverState(StateMachine stateMachine,SpriteBatch spriteBatch,GraphicsDevice graphicsDevice, PlayerInput playerInput,SpriteFont spriteFont, CoreGameState coreGameState) : base(stateMachine)
    {
        _playerInput = playerInput;
        _spriteBatch = spriteBatch;
        _coreGameState = coreGameState;

        Texture2D buttonTexture = new Texture2D(graphicsDevice,1,1);
        buttonTexture.SetData<Color>(new Color [] { Color.Gray });
        _mainMenuButton = new Button(buttonTexture,new Rectangle(10,10,100,20),"Main menu",spriteFont);
        _mainMenuButton.clicked+=OnMenuButtonClicked;

        _finaleScoreText = new TextPanel(spriteFont,"Points: ",new Vector2(graphicsDevice.Viewport.Width/2,graphicsDevice.Viewport.Height/2));
    }

    public override void Enter()
    {
        base.Enter();

        _playerInput.mouseClicked+=_mainMenuButton.OnPlayerClickedAtPosition;
        OnPointsChanged(_coreGameState.PlayerStats.Points);
        _coreGameState.PlayerStats.pointsChanged+=OnPointsChanged;
    }

    public override void Draw()
    {
        base.Draw();

        _mainMenuButton.Draw(_spriteBatch);
        _finaleScoreText.Draw(_spriteBatch);
    }

    public override void Exit()
    {
        base.Exit();

        _playerInput.mouseClicked-=_mainMenuButton.OnPlayerClickedAtPosition;

        _coreGameState.PlayerStats.pointsChanged-=OnPointsChanged;
    }

    public void OnMenuButtonClicked()
    {
        _stateMachine.ChangeState<MainMenuState>();
    }

    public void OnPointsChanged(int amount)
    {
        _finaleScoreText.ChangeText($"Finale score: {amount} points");
    }
}