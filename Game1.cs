﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Merge_3;

public class Game1 : Game
{
    private const byte FIELD_SIZE = 8;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private StateMachine _gameStateMachine;
    private PlayerInput _playerInput;
    

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        Window.Title = "Merge-2 game for GameForest";
        _playerInput = new PlayerInput();
        _gameStateMachine = new StateMachine();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        
        SpriteFont spriteFont = Content.Load<SpriteFont>("Font");
        _gameStateMachine.AddState(new MainMenuState(_gameStateMachine,_spriteBatch,GraphicsDevice,_playerInput,spriteFont));
        _gameStateMachine.AddState(new CoreGameState(_gameStateMachine, FIELD_SIZE,_spriteBatch,GraphicsDevice,spriteFont,_playerInput));
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        _playerInput.HandleInput();
        _gameStateMachine.Update();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _gameStateMachine.Draw();

        base.Draw(gameTime);
    }
}
