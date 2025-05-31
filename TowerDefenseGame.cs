using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TowerDefense.Scenes;

namespace TowerDefense;

public class TowerDefenseGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private SceneManager _sceneManager;

    public TowerDefenseGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        
        // Ekran boyutunu ayarla
        _graphics.PreferredBackBufferWidth = 1024;
        _graphics.PreferredBackBufferHeight = 768;
    }

    protected override void Initialize()
    {
        _sceneManager = new SceneManager();
        
        // Sahneleri oluştur ve ekle
        var mainMenuScene = new MainMenuScene(Content, _sceneManager, GraphicsDevice);
        var gameScene = new GameScene(Content, GraphicsDevice, _sceneManager);
        
        _sceneManager.AddScene("MainMenu", mainMenuScene);
        _sceneManager.AddScene("Game", gameScene);
        
        // Ana menü ile başla
        _sceneManager.SetScene("MainMenu");

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || 
            Keyboard.GetState().IsKeyDown(Keys.F4) && Keyboard.GetState().IsKeyDown(Keys.LeftAlt))
            Exit();

        _sceneManager.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.DarkGreen);

        _spriteBatch.Begin();
        _sceneManager.Draw(_spriteBatch);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
