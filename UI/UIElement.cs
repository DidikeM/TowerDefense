using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace TowerDefense.UI;

public abstract class UIElement
{
    public Vector2 Position { get; set; }
    public Vector2 Size { get; set; }
    public bool IsVisible { get; set; } = true;
    public bool IsEnabled { get; set; } = true;
    
    public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
    
    public abstract void Update(GameTime gameTime);
    public abstract void Draw(SpriteBatch spriteBatch);
    
    public virtual bool Contains(Vector2 point)
    {
        return Bounds.Contains(point);
    }
} 