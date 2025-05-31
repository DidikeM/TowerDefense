using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System.Collections.Generic;

namespace TowerDefense.UI;

public class Panel : UIElement
{
    private readonly List<UIElement> _children;
    private Color _backgroundColor;
    private Texture2D? _backgroundTexture;
    
    public Panel(Color? backgroundColor = null)
    {
        _children = new List<UIElement>();
        _backgroundColor = backgroundColor ?? Color.Transparent;
    }
    
    public void SetBackgroundTexture(Texture2D texture)
    {
        _backgroundTexture = texture;
    }
    
    public void AddChild(UIElement child)
    {
        _children.Add(child);
    }
    
    public void RemoveChild(UIElement child)
    {
        _children.Remove(child);
    }
    
    public override void Update(GameTime gameTime)
    {
        foreach (var child in _children)
        {
            child.Update(gameTime);
        }
    }
    
    public override void Draw(SpriteBatch spriteBatch)
    {
        if (!IsVisible) return;
        
        if (_backgroundTexture != null)
        {
            spriteBatch.Draw(_backgroundTexture, Bounds, Color.White);
        }
        else if (_backgroundColor != Color.Transparent)
        {
            spriteBatch.FillRectangle(Bounds, _backgroundColor);
        }
        
        foreach (var child in _children)
        {
            child.Draw(spriteBatch);
        }
    }
} 