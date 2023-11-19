using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Merge_3;

public class Game1 : Game
{
    private const byte FIELD_SIZE = 8;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Field _field;
    private Texture2D _cellTexture;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        _field = new Field(FIELD_SIZE);
        _field.GenerateField();
        Window.Title = "Merge-3 game for GameForest";


        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _cellTexture = new Texture2D(GraphicsDevice,1,1);
        _cellTexture.SetData<Color>(new Color [] { Color.Gray });
        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
        _spriteBatch.Draw(_cellTexture, new Rectangle(10,10,10,10), Color.White);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
