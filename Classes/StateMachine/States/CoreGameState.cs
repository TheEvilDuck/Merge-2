using Microsoft.Xna.Framework.Graphics;

public class CoreGameState : State
{
    private Field _field;
    private FieldVisuals _fieldVisuals;
    private SpriteBatch _spriteBatch;
    private GraphicsDevice _graphicsDevice;
    public CoreGameState(StateMachine stateMachine, byte fieldSize,SpriteBatch spriteBatch,GraphicsDevice graphicsDevice) : base(stateMachine)
    {
        _field = new Field(fieldSize);
        _field.GenerateField();

        _spriteBatch = spriteBatch;
        _graphicsDevice = graphicsDevice;

        _fieldVisuals = new FieldVisuals(_spriteBatch,_graphicsDevice,fieldSize);
    }
    public override void Draw()
    {
        base.Draw();

        _fieldVisuals.Draw();
    }
}