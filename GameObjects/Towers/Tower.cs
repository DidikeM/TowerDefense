using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using TowerDefense.GameObjects.Enemies;

namespace TowerDefense.GameObjects.Towers;

public abstract class Tower : GameObject
{
    public float Range { get; protected set; }
    public float Damage { get; protected set; }
    public float FireRate { get; protected set; } // Saniyede ateş sayısı
    public int Cost { get; protected set; }
    public int Level { get; protected set; } = 1;
    
    protected float TimeSinceLastShot { get; set; }
    protected Enemy? Target { get; set; }
    
    public bool CanFire => TimeSinceLastShot >= 1f / FireRate;
    
    protected Tower(float range, float damage, float fireRate, int cost)
    {
        Range = range;
        Damage = damage;
        FireRate = fireRate;
        Cost = cost;
        TimeSinceLastShot = 0f;
    }
    
    public override void Update(GameTime gameTime)
    {
        TimeSinceLastShot += (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        UpdateTarget();
        
        if (Target != null && CanFire)
        {
            Fire();
            TimeSinceLastShot = 0f;
        }
        
        UpdateRotation();
    }
    
    protected virtual void UpdateTarget()
    {
        // Mevcut hedef hala menzilde mi kontrol et
        if (Target != null && (Target.DistanceTo(this) > Range || !Target.IsActive))
        {
            Target = null;
        }
    }
    
    public void FindTarget(List<Enemy> enemies)
    {
        if (Target != null) return; // Zaten hedef var
        
        // Menzil içindeki en yakın düşmanı bul
        Target = enemies
            .Where(e => e.IsActive && e.DistanceTo(this) <= Range)
            .OrderBy(e => e.DistanceTo(this))
            .FirstOrDefault();
    }
    
    protected virtual void Fire()
    {
        if (Target == null) return;
        
        // Temel ateş etme mantığı - türetilen sınıflar override edebilir
        Target.TakeDamage(Damage);
        
        // Ateş efekti, ses vs. burada eklenebilir
        OnFire();
    }
    
    protected virtual void OnFire()
    {
        // Ateş etme efektleri için override edilebilir
    }
    
    protected virtual void UpdateRotation()
    {
        if (Target != null)
        {
            var direction = Target.Position - Position;
            Rotation = (float)Math.Atan2(direction.Y, direction.X);
        }
    }
    
    public virtual void Upgrade()
    {
        Level++;
        // Temel upgrade mantığı - türetilen sınıflar override edebilir
        Damage *= 1.2f;
        Range *= 1.1f;
        FireRate *= 1.1f;
    }
    
    public virtual int GetUpgradeCost()
    {
        return Cost / 2 * Level;
    }
    
    public bool IsInRange(Enemy enemy)
    {
        return enemy.DistanceTo(this) <= Range;
    }
    
    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        
        // Debug: Menzil çemberini çiz (geliştirme aşamasında)
        #if DEBUG
        DrawRange(spriteBatch);
        #endif
    }
    
    protected virtual void DrawRange(SpriteBatch spriteBatch)
    {
        // Menzil çemberi çizimi için basit bir implementasyon
        // Gerçek projede daha gelişmiş bir çember çizimi kullanılabilir
    }
} 