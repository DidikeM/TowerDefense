using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace TowerDefense.GameObjects.Enemies;

public abstract class Enemy : GameObject
{
    public float Health { get; protected set; }
    public float MaxHealth { get; protected set; }
    public float Speed { get; protected set; }
    public int Reward { get; protected set; }
    public float Damage { get; protected set; }
    
    protected List<Vector2> Path { get; set; }
    protected int CurrentPathIndex { get; set; }
    
    public bool IsAlive => Health > 0;
    public bool ReachedEnd => CurrentPathIndex >= Path.Count;
    
    protected Enemy(float health, float speed, int reward, float damage)
    {
        Health = health;
        MaxHealth = health;
        Speed = speed;
        Reward = reward;
        Damage = damage;
        Path = new List<Vector2>();
        CurrentPathIndex = 0;
    }
    
    public void SetPath(List<Vector2> path)
    {
        Path = new List<Vector2>(path);
        CurrentPathIndex = 0;
        if (Path.Count > 0)
        {
            Position = Path[0];
        }
    }
    
    public virtual void TakeDamage(float damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Health = 0;
            IsActive = false;
        }
    }
    
    public override void Update(GameTime gameTime)
    {
        if (!IsActive || ReachedEnd) return;
        
        MoveAlongPath(gameTime);
    }
    
    protected virtual void MoveAlongPath(GameTime gameTime)
    {
        if (CurrentPathIndex >= Path.Count) return;
        
        var target = Path[CurrentPathIndex];
        var direction = target - Position;
        
        if (direction.Length() < 5f) // Hedefe yakın
        {
            CurrentPathIndex++;
            if (CurrentPathIndex >= Path.Count)
            {
                // Son noktaya ulaştı
                return;
            }
            target = Path[CurrentPathIndex];
            direction = target - Position;
        }
        
        if (direction.Length() > 0)
        {
            direction.Normalize();
            Position += direction * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            // Rotasyonu hareket yönüne göre ayarla
            Rotation = (float)Math.Atan2(direction.Y, direction.X);
        }
    }
    
    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        
        // Sağlık barı çiz
        DrawHealthBar(spriteBatch);
    }
    
    protected virtual void DrawHealthBar(SpriteBatch spriteBatch)
    {
        if (Health >= MaxHealth) return;
        
        var barWidth = Size.X;
        var barHeight = 4;
        var barPosition = new Vector2(Position.X - barWidth / 2, Position.Y - Size.Y / 2 - 10);
        
        // Arka plan (kırmızı)
        var backgroundRect = new Rectangle((int)barPosition.X, (int)barPosition.Y, (int)barWidth, barHeight);
        spriteBatch.Draw(CreatePixelTexture(spriteBatch.GraphicsDevice), backgroundRect, Color.Red);
        
        // Sağlık (yeşil)
        var healthWidth = barWidth * (Health / MaxHealth);
        var healthRect = new Rectangle((int)barPosition.X, (int)barPosition.Y, (int)healthWidth, barHeight);
        spriteBatch.Draw(CreatePixelTexture(spriteBatch.GraphicsDevice), healthRect, Color.Green);
    }
    
    private Texture2D CreatePixelTexture(GraphicsDevice graphicsDevice)
    {
        var texture = new Texture2D(graphicsDevice, 1, 1);
        texture.SetData(new[] { Color.White });
        return texture;
    }
} 