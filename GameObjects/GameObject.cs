using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefense.GameObjects;

public abstract class GameObject
{
    public Vector2 Position { get; set; }
    public Vector2 Size { get; set; }
    public bool IsActive { get; set; } = true;
    public float Rotation { get; set; }
    
    protected Texture2D? Texture { get; set; }
    
    public Rectangle Bounds => new Rectangle(
        (int)(Position.X - Size.X / 2),
        (int)(Position.Y - Size.Y / 2),
        (int)Size.X,
        (int)Size.Y
    );
    
    public Vector2 Center => Position;
    
    public abstract void Update(GameTime gameTime);
    
    public virtual void Draw(SpriteBatch spriteBatch)
    {
        if (Texture != null && IsActive)
        {
            var origin = new Vector2(Texture.Width / 2f, Texture.Height / 2f);
            spriteBatch.Draw(Texture, Position, null, Color.White, Rotation, origin, 1f, SpriteEffects.None, 0f);
        }
    }
    
    public virtual bool CollidesWith(GameObject other)
    {
        return Bounds.Intersects(other.Bounds);
    }
    
    public float DistanceTo(GameObject other)
    {
        return Vector2.Distance(Position, other.Position);
    }
    
    public float DistanceTo(Vector2 point)
    {
        return Vector2.Distance(Position, point);
    }
} 