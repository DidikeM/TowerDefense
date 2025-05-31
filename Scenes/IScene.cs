using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefense.Scenes;

public interface IScene
{
    void Initialize();
    void LoadContent();
    void Update(GameTime gameTime);
    void Draw(SpriteBatch spriteBatch);
    void UnloadContent();
} 