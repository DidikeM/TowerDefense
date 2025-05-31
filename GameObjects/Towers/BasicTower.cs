using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefense.GameObjects.Towers;

public class BasicTower : Tower
{
    public BasicTower() : base(range: 100f, damage: 25f, fireRate: 1f, cost: 50)
    {
        Size = new Vector2(30, 30);
    }
    
    public void SetTexture(Texture2D texture)
    {
        Texture = texture;
    }
    
    protected override void OnFire()
    {
        // Temel kule ateş efektleri
        // Ses, parçacık efektleri vs. eklenebilir
    }
} 