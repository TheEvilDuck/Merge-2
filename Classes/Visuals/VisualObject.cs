using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public abstract class VisualObject
{
    
    protected Color _color = Color.White;

    public abstract Vector2 Position {get; protected set;}
    
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        OnDrawStarted(spriteBatch);
        spriteBatch.End();
    }
    public abstract void MoveTo(Vector2 position);

    public void SetColor(Color color)
    {
        _color = color;
    }
    public abstract void OnDrawStarted(SpriteBatch spriteBatch);
}