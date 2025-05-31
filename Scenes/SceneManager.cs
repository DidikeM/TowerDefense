using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TowerDefense.Scenes;

public class SceneManager
{
    private readonly Dictionary<string, IScene> _scenes;
    private IScene? _currentScene;
    
    public SceneManager()
    {
        _scenes = new Dictionary<string, IScene>();
    }
    
    public void AddScene(string name, IScene scene)
    {
        _scenes[name] = scene;
    }
    
    public void SetScene(string name)
    {
        if (_scenes.TryGetValue(name, out var scene))
        {
            _currentScene?.UnloadContent();
            _currentScene = scene;
            _currentScene.Initialize();
            _currentScene.LoadContent();
        }
    }
    
    public void Update(GameTime gameTime)
    {
        _currentScene?.Update(gameTime);
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        _currentScene?.Draw(spriteBatch);
    }
} 