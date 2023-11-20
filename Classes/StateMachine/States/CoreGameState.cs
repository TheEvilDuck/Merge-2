using Microsoft.Xna.Framework.Graphics;

public class CoreGameState : State
{
    private Field _field;
    private FieldVisuals _fieldVisuals;
    private SpriteBatch _spriteBatch;
    private GraphicsDevice _graphicsDevice;
    public CoreGameState(StateMachine stateMachine, byte fieldSize,SpriteBatch spriteBatch,GraphicsDevice graphicsDevice,SpriteFont spriteFont,PlayerInput playerInput) : base(stateMachine)
    {
        _field = new Field(fieldSize);

        _spriteBatch = spriteBatch;
        _graphicsDevice = graphicsDevice;

        _fieldVisuals = new FieldVisuals(_spriteBatch,_graphicsDevice,fieldSize,_field,spriteFont,playerInput);

        _field.GenerateField(8);
    }
    public override void Draw()
    {
        base.Draw();

        _fieldVisuals.Draw();
    }
    public override void Enter()
    {
        base.Enter();

        _fieldVisuals.SubToInput();
    }
    public override void Exit()
    {
        base.Exit();

        _fieldVisuals.UnSubToInput();
    }
}