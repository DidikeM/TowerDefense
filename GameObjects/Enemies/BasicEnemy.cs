using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefense.GameObjects.Enemies;

public class BasicEnemy : Enemy
{
    public BasicEnemy() : base(health: 100f, speed: 50f, reward: 10, damage: 1f)
    {
        Size = new Vector2(20, 20);
    }
    
    public void SetTexture(Texture2D texture)
    {
        Texture = texture;
    }
} 