using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class MainMenuState : State
{
    private PlayerInput _playerInput;
    public MainMenuState(StateMachine stateMachine,SpriteBatch spriteBatch,GraphicsDevice graphicsDevice, PlayerInput playerInput) : base(stateMachine)
    {
        _playerInput = playerInput;  
    }

    public override void Enter()
    {
        base.Enter();

        _playerInput.mouseClicked+=OnMouseClicked;
    }
    public override void Exit()
    {
        base.Exit();

        _playerInput.mouseClicked-=OnMouseClicked;
    }
    private void OnMouseClicked(Vector2 atPosition)
    {
        _stateMachine.ChangeState<CoreGameState>();
    }
}