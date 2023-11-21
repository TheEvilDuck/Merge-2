using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Panel : VisualObject
{
    private Texture2D _texture;
    private Rectangle _rectangle;

    protected Rectangle Rectangle => _rectangle;

    public override Vector2 Position 
    { 
        get => new Vector2(Rectangle.X,Rectangle.Y); 
        protected set 
        {
            _rectangle.X = (int)value.X;
            _rectangle.Y = (int)value.Y;
        }
    }

    public Panel(Texture2D texture, Rectangle rectangle) : base()
    {
        _texture = texture;
        _rectangle = rectangle;
    }

    public override void MoveTo(Vector2 position)
    {
        _rectangle.X = (int)position.X;
        _rectangle.Y = (int)position.Y;
    }

    public override void OnDrawStarted(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture,_rectangle,_color);
    }
}