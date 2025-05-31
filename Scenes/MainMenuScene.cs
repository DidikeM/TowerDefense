using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TowerDefense.UI;
using System;

namespace TowerDefense.Scenes;

public class MainMenuScene : IScene
{
    private readonly ContentManager _content;
    private readonly SceneManager _sceneManager;
    private readonly GraphicsDevice _graphicsDevice;
    
    private Panel? _mainPanel;
    private Button? _playButton;
    private Button? _optionsButton;
    private Button? _exitButton;
    private SpriteFont? _titleFont;
    private SpriteFont? _buttonFont;
    private Texture2D? _pixelTexture;
    
    public MainMenuScene(ContentManager content, SceneManager sceneManager, GraphicsDevice graphicsDevice)
    {
        _content = content;
        _sceneManager = sceneManager;
        _graphicsDevice = graphicsDevice;
    }
    
    public void Initialize()
    {
        CreatePixelTexture();
        SetupUI();
    }
    
    public void LoadContent()
    {
        try
        {
            _titleFont = _content.Load<SpriteFont>("Fonts/TitleFont");
        }
        catch
        {
            // Font yüklenemezse default kullan
        }
        
        try
        {
            _buttonFont = _content.Load<SpriteFont>("Fonts/ButtonFont");
        }
        catch
        {
            // Font yüklenemezse default kullan
        }
        
        if (_buttonFont != null)
        {
            _playButton?.SetFont(_buttonFont);
            _optionsButton?.SetFont(_buttonFont);
            _exitButton?.SetFont(_buttonFont);
        }
    }
    
    private void CreatePixelTexture()
    {
        _pixelTexture = new Texture2D(_graphicsDevice, 1, 1);
        _pixelTexture.SetData(new[] { Color.White });
    }
    
    private void SetupUI()
    {
        var screenWidth = _graphicsDevice.Viewport.Width;
        var screenHeight = _graphicsDevice.Viewport.Height;
        
        _mainPanel = new Panel(Color.Black * 0.8f)
        {
            Position = Vector2.Zero,
            Size = new Vector2(screenWidth, screenHeight)
        };
        
        var buttonWidth = 200;
        var buttonHeight = 50;
        var buttonX = (screenWidth - buttonWidth) / 2;
        var startY = screenHeight / 2;
        
        _playButton = new Button("Oyuna Başla", Color.DarkGreen, Color.Green, Color.DarkOliveGreen)
        {
            Position = new Vector2(buttonX, startY),
            Size = new Vector2(buttonWidth, buttonHeight)
        };
        _playButton.OnClick += () => _sceneManager.SetScene("Game");
        
        _optionsButton = new Button("Seçenekler", Color.DarkBlue, Color.Blue, Color.Navy)
        {
            Position = new Vector2(buttonX, startY + 70),
            Size = new Vector2(buttonWidth, buttonHeight)
        };
        _optionsButton.OnClick += () => { /* Options menüsü açılacak */ };
        
        _exitButton = new Button("Çıkış", Color.DarkRed, Color.Red, Color.Maroon)
        {
            Position = new Vector2(buttonX, startY + 140),
            Size = new Vector2(buttonWidth, buttonHeight)
        };
        _exitButton.OnClick += () => Environment.Exit(0);
        
        _mainPanel.AddChild(_playButton);
        _mainPanel.AddChild(_optionsButton);
        _mainPanel.AddChild(_exitButton);
    }
    
    public void Update(GameTime gameTime)
    {
        _mainPanel?.Update(gameTime);
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        _mainPanel?.Draw(spriteBatch);
        
        // Başlık çiz
        if (_titleFont != null)
        {
            var title = "Tower Defense";
            var titleSize = _titleFont.MeasureString(title);
            var titlePosition = new Vector2(
                (_graphicsDevice.Viewport.Width - titleSize.X) / 2,
                100
            );
            spriteBatch.DrawString(_titleFont, title, titlePosition, Color.Gold);
        }
    }
    
    public void UnloadContent()
    {
        _pixelTexture?.Dispose();
    }
} 