using Microsoft.Xna.Framework.Graphics;

public class CoreGameState : State
{
    private Field _field;
    private FieldVisuals _fieldVisuals;
    private SpriteBatch _spriteBatch;
    private GraphicsDevice _graphicsDevice;

    private byte _fieldSize;
    private SpriteFont _spriteFont;
    private PlayerInput _playerInput;

    public CoreGameState(StateMachine stateMachine, byte fieldSize,SpriteBatch spriteBatch,GraphicsDevice graphicsDevice,SpriteFont spriteFont,PlayerInput playerInput) : base(stateMachine)
    {
        _spriteBatch = spriteBatch;
        _graphicsDevice = graphicsDevice;
        _fieldSize = fieldSize;
        _spriteFont = spriteFont;
        _playerInput = playerInput;

    }
    public override void Enter()
    {
        base.Enter();

        _field = new Field(_fieldSize);
        _fieldVisuals = new FieldVisuals(_spriteBatch,_graphicsDevice,_fieldSize,_field,_spriteFont,_playerInput,_graphicsDevice.Viewport.Width/2,_graphicsDevice.Viewport.Height/2);

        _field.GenerateField(8);
    }
    public override void Draw()
    {
        base.Draw();

        _fieldVisuals.Draw();
    }
    public override void Exit()
    {
        base.Exit();

        _fieldVisuals.UnSubFromInput();
    }
}