using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using TowerDefense.GameObjects.Enemies;
using TowerDefense.GameObjects.Towers;
using TowerDefense.Map;
using TowerDefense.UI;

namespace TowerDefense.Scenes;

public class GameScene : IScene
{
    private readonly ContentManager _content;
    private readonly GraphicsDevice _graphicsDevice;
    private readonly SceneManager _sceneManager;
    
    private MapManager? _mapManager;
    private List<Enemy> _enemies;
    private List<Tower> _towers;
    private Panel? _gameUI;
    private Button? _pauseButton;
    private Button? _menuButton;
    
    // Oyun durumu
    private int _playerHealth = 100;
    private int _playerMoney = 200;
    private int _wave = 1;
    private float _timeSinceLastSpawn = 0f;
    private float _spawnInterval = 2f;
    private int _enemiesSpawnedThisWave = 0;
    private int _enemiesPerWave = 5;
    
    // Input
    private MouseState _previousMouseState;
    private KeyboardState _previousKeyboardState;
    
    // Textures
    private Texture2D? _enemyTexture;
    private Texture2D? _towerTexture;
    private SpriteFont? _gameFont;
    
    public GameScene(ContentManager content, GraphicsDevice graphicsDevice, SceneManager sceneManager)
    {
        _content = content;
        _graphicsDevice = graphicsDevice;
        _sceneManager = sceneManager;
        _enemies = new List<Enemy>();
        _towers = new List<Tower>();
    }
    
    public void Initialize()
    {
        _mapManager = new MapManager(_content, _graphicsDevice);
        _mapManager.LoadMap("level1"); // Varsayılan harita
        
        SetupUI();
        CreateTestTextures();
    }
    
    public void LoadContent()
    {
        try
        {
            _gameFont = _content.Load<SpriteFont>("Fonts/GameFont");
        }
        catch
        {
            // Font bulunamazsa devam et
        }
        
        try
        {
            _enemyTexture = _content.Load<Texture2D>("Sprites/Enemy");
        }
        catch
        {
            // Texture bulunamazsa test texture kullan
        }
        
        try
        {
            _towerTexture = _content.Load<Texture2D>("Sprites/Tower");
        }
        catch
        {
            // Texture bulunamazsa test texture kullan
        }
    }
    
    private void CreateTestTextures()
    {
        // Test için basit renkli texture'lar oluştur
        if (_enemyTexture == null)
        {
            _enemyTexture = new Texture2D(_graphicsDevice, 20, 20);
            var enemyData = new Color[400];
            for (int i = 0; i < enemyData.Length; i++)
                enemyData[i] = Color.Red;
            _enemyTexture.SetData(enemyData);
        }
        
        if (_towerTexture == null)
        {
            _towerTexture = new Texture2D(_graphicsDevice, 30, 30);
            var towerData = new Color[900];
            for (int i = 0; i < towerData.Length; i++)
                towerData[i] = Color.Blue;
            _towerTexture.SetData(towerData);
        }
    }
    
    private void SetupUI()
    {
        var screenWidth = _graphicsDevice.Viewport.Width;
        var screenHeight = _graphicsDevice.Viewport.Height;
        
        _gameUI = new Panel(Color.Black * 0.3f)
        {
            Position = new Vector2(0, screenHeight - 100),
            Size = new Vector2(screenWidth, 100)
        };
        
        _pauseButton = new Button("Duraklat", Color.Orange, Color.Gold, Color.DarkOrange)
        {
            Position = new Vector2(10, screenHeight - 90),
            Size = new Vector2(80, 30)
        };
        _pauseButton.OnClick += () => { /* Pause logic */ };
        
        _menuButton = new Button("Menü", Color.Gray, Color.LightGray, Color.DarkGray)
        {
            Position = new Vector2(100, screenHeight - 90),
            Size = new Vector2(80, 30)
        };
        _menuButton.OnClick += () => _sceneManager.SetScene("MainMenu");
        
        _gameUI.AddChild(_pauseButton);
        _gameUI.AddChild(_menuButton);
    }
    
    public void Update(GameTime gameTime)
    {
        var currentMouseState = Mouse.GetState();
        var currentKeyboardState = Keyboard.GetState();
        
        // Harita güncelle
        _mapManager?.Update(gameTime);
        
        // Düşman spawn
        UpdateEnemySpawning(gameTime);
        
        // Düşmanları güncelle
        UpdateEnemies(gameTime);
        
        // Kuleleri güncelle
        UpdateTowers(gameTime);
        
        // Input işle
        HandleInput(currentMouseState, currentKeyboardState);
        
        // UI güncelle
        _gameUI?.Update(gameTime);
        
        // Oyun durumu kontrol
        CheckGameState();
        
        _previousMouseState = currentMouseState;
        _previousKeyboardState = currentKeyboardState;
    }
    
    private void UpdateEnemySpawning(GameTime gameTime)
    {
        if (_enemiesSpawnedThisWave >= _enemiesPerWave)
        {
            // Bu dalga tamamlandı, yeni dalga başlat
            if (_enemies.Count == 0)
            {
                StartNextWave();
            }
            return;
        }
        
        _timeSinceLastSpawn += (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        if (_timeSinceLastSpawn >= _spawnInterval)
        {
            SpawnEnemy();
            _timeSinceLastSpawn = 0f;
            _enemiesSpawnedThisWave++;
        }
    }
    
    private void SpawnEnemy()
    {
        var enemy = new BasicEnemy();
        enemy.SetTexture(_enemyTexture!);
        enemy.SetPath(_mapManager!.EnemyPath);
        _enemies.Add(enemy);
    }
    
    private void StartNextWave()
    {
        _wave++;
        _enemiesSpawnedThisWave = 0;
        _enemiesPerWave += 2; // Her dalgada daha fazla düşman
        _spawnInterval = Math.Max(0.5f, _spawnInterval - 0.1f); // Daha hızlı spawn
    }
    
    private void UpdateEnemies(GameTime gameTime)
    {
        for (int i = _enemies.Count - 1; i >= 0; i--)
        {
            var enemy = _enemies[i];
            enemy.Update(gameTime);
            
            if (!enemy.IsActive)
            {
                // Düşman öldü, para ver
                _playerMoney += enemy.Reward;
                _enemies.RemoveAt(i);
            }
            else if (enemy.ReachedEnd)
            {
                // Düşman hedefe ulaştı, can azalt
                _playerHealth -= (int)enemy.Damage;
                _enemies.RemoveAt(i);
            }
        }
    }
    
    private void UpdateTowers(GameTime gameTime)
    {
        foreach (var tower in _towers)
        {
            tower.FindTarget(_enemies);
            tower.Update(gameTime);
        }
    }
    
    private void HandleInput(MouseState mouseState, KeyboardState keyboardState)
    {
        // Escape tuşu ile ana menüye dön
        if (keyboardState.IsKeyDown(Keys.Escape) && !_previousKeyboardState.IsKeyDown(Keys.Escape))
        {
            _sceneManager.SetScene("MainMenu");
        }
        
        // Sol tık ile kule yerleştir
        if (mouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
        {
            var mousePosition = new Vector2(mouseState.X, mouseState.Y);
            TryPlaceTower(mousePosition);
        }
    }
    
    private void TryPlaceTower(Vector2 mousePosition)
    {
        var towerPosition = _mapManager?.GetNearestTowerPosition(mousePosition);
        if (towerPosition == null) return;
        
        // Bu pozisyonda zaten kule var mı kontrol et
        var existingTower = _towers.FirstOrDefault(t => Vector2.Distance(t.Position, towerPosition.Value) < 30f);
        if (existingTower != null) return;
        
        // Para yeterli mi kontrol et
        var tower = new BasicTower();
        if (_playerMoney < tower.Cost) return;
        
        // Kule yerleştir
        tower.Position = towerPosition.Value;
        tower.SetTexture(_towerTexture!);
        _towers.Add(tower);
        _playerMoney -= tower.Cost;
    }
    
    private void CheckGameState()
    {
        if (_playerHealth <= 0)
        {
            // Oyun bitti - Game Over
            // Game Over sahnesine geç
        }
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        // Haritayı çiz
        _mapManager?.Draw(spriteBatch);
        
        // Düşmanları çiz
        foreach (var enemy in _enemies)
        {
            enemy.Draw(spriteBatch);
        }
        
        // Kuleleri çiz
        foreach (var tower in _towers)
        {
            tower.Draw(spriteBatch);
        }
        
        // UI çiz
        _gameUI?.Draw(spriteBatch);
        
        // Oyun bilgilerini çiz
        DrawGameInfo(spriteBatch);
    }
    
    private void DrawGameInfo(SpriteBatch spriteBatch)
    {
        if (_gameFont == null) return;
        
        var infoText = $"Can: {_playerHealth} | Para: {_playerMoney} | Dalga: {_wave}";
        spriteBatch.DrawString(_gameFont, infoText, new Vector2(10, 10), Color.White);
        
        var instructionText = "Sol tık: Kule yerleştir | ESC: Ana menü";
        spriteBatch.DrawString(_gameFont, instructionText, new Vector2(10, 30), Color.Yellow);
    }
    
    public void UnloadContent()
    {
        _enemyTexture?.Dispose();
        _towerTexture?.Dispose();
    }
} 