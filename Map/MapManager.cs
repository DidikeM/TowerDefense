using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TowerDefense.Map;

public class MapManager
{
    private TiledMap? _tiledMap;
    private TiledMapRenderer? _tiledMapRenderer;
    private readonly ContentManager _content;
    private readonly GraphicsDevice _graphicsDevice;
    
    public List<Vector2> EnemyPath { get; private set; }
    public List<Vector2> TowerPositions { get; private set; }
    
    public MapManager(ContentManager content, GraphicsDevice graphicsDevice)
    {
        _content = content;
        _graphicsDevice = graphicsDevice;
        EnemyPath = new List<Vector2>();
        TowerPositions = new List<Vector2>();
    }
    
    public void LoadMap(string mapName)
    {
        try
        {
            _tiledMap = _content.Load<TiledMap>($"Maps/{mapName}");
            _tiledMapRenderer = new TiledMapRenderer(_graphicsDevice, _tiledMap);
            
            ExtractPathFromMap();
            ExtractTowerPositionsFromMap();
        }
        catch
        {
            // Harita yüklenemezse varsayılan yol oluştur
            CreateDefaultPath();
            CreateDefaultTowerPositions();
        }
    }
    
    private void ExtractPathFromMap()
    {
        EnemyPath.Clear();
        
        if (_tiledMap == null) return;
        
        // "PathPoints" katmanından yol bilgilerini çıkar
        var pathLayer = _tiledMap.ObjectLayers.FirstOrDefault(layer => layer.Name == "PathPoints");
        if (pathLayer != null)
        {
            var pathPoints = pathLayer.Objects
                .Where(obj => obj.Name == "PathPoint")
                .OrderBy(obj => 
                {
                    if (obj.Properties.TryGetValue("Order", out var orderValue))
                        return int.Parse(orderValue);
                    return 0;
                })
                .Select(obj => new Vector2(obj.Position.X, obj.Position.Y))
                .ToList();
                
            EnemyPath.AddRange(pathPoints);
        }
        
        if (EnemyPath.Count == 0)
        {
            CreateDefaultPath();
        }
    }
    
    private void ExtractTowerPositionsFromMap()
    {
        TowerPositions.Clear();
        
        if (_tiledMap == null) return;
        
        // "TowerPositions" katmanından kule yerleştirme noktalarını çıkar
        var towerLayer = _tiledMap.ObjectLayers.FirstOrDefault(layer => layer.Name == "TowerPositions");
        if (towerLayer != null)
        {
            var positions = towerLayer.Objects
                .Where(obj => obj.Name == "TowerPosition")
                .Select(obj => new Vector2(obj.Position.X, obj.Position.Y))
                .ToList();
                
            TowerPositions.AddRange(positions);
        }
        
        if (TowerPositions.Count == 0)
        {
            CreateDefaultTowerPositions();
        }
    }
    
    private void CreateDefaultPath()
    {
        // Varsayılan yol (sol üstten sağ alta)
        EnemyPath.Clear();
        EnemyPath.Add(new Vector2(0, 200));
        EnemyPath.Add(new Vector2(200, 200));
        EnemyPath.Add(new Vector2(200, 400));
        EnemyPath.Add(new Vector2(600, 400));
        EnemyPath.Add(new Vector2(600, 200));
        EnemyPath.Add(new Vector2(800, 200));
    }
    
    private void CreateDefaultTowerPositions()
    {
        // Varsayılan kule pozisyonları
        TowerPositions.Clear();
        for (int x = 100; x < 700; x += 100)
        {
            for (int y = 100; y < 500; y += 100)
            {
                // Yol üzerinde olmayan pozisyonları ekle
                var position = new Vector2(x, y);
                if (!IsOnPath(position, 50f))
                {
                    TowerPositions.Add(position);
                }
            }
        }
    }
    
    public bool IsOnPath(Vector2 position, float threshold = 30f)
    {
        for (int i = 0; i < EnemyPath.Count - 1; i++)
        {
            var start = EnemyPath[i];
            var end = EnemyPath[i + 1];
            
            var distance = DistancePointToLineSegment(position, start, end);
            if (distance < threshold)
            {
                return true;
            }
        }
        return false;
    }
    
    private float DistancePointToLineSegment(Vector2 point, Vector2 lineStart, Vector2 lineEnd)
    {
        var line = lineEnd - lineStart;
        var lineLength = line.Length();
        
        if (lineLength == 0) return Vector2.Distance(point, lineStart);
        
        var t = Math.Max(0, Math.Min(1, Vector2.Dot(point - lineStart, line) / (lineLength * lineLength)));
        var projection = lineStart + t * line;
        
        return Vector2.Distance(point, projection);
    }
    
    public Vector2? GetNearestTowerPosition(Vector2 mousePosition, float maxDistance = 50f)
    {
        return TowerPositions
            .Where(pos => Vector2.Distance(pos, mousePosition) <= maxDistance)
            .OrderBy(pos => Vector2.Distance(pos, mousePosition))
            .FirstOrDefault();
    }
    
    public void Update(GameTime gameTime)
    {
        _tiledMapRenderer?.Update(gameTime);
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        _tiledMapRenderer?.Draw();
        
        // Debug: Yol ve kule pozisyonlarını göster
        #if DEBUG
        DrawDebugInfo(spriteBatch);
        #endif
    }
    
    private void DrawDebugInfo(SpriteBatch spriteBatch)
    {
        var pixelTexture = CreatePixelTexture();
        
        // Yolu çiz
        for (int i = 0; i < EnemyPath.Count - 1; i++)
        {
            var start = EnemyPath[i];
            var end = EnemyPath[i + 1];
            DrawLine(spriteBatch, pixelTexture, start, end, Color.Red, 3);
        }
        
        // Kule pozisyonlarını çiz
        foreach (var position in TowerPositions)
        {
            var rect = new Rectangle((int)position.X - 5, (int)position.Y - 5, 10, 10);
            spriteBatch.Draw(pixelTexture, rect, Color.Green);
        }
    }
    
    private void DrawLine(SpriteBatch spriteBatch, Texture2D pixel, Vector2 start, Vector2 end, Color color, int thickness)
    {
        var distance = Vector2.Distance(start, end);
        var angle = (float)Math.Atan2(end.Y - start.Y, end.X - start.X);
        
        spriteBatch.Draw(pixel, start, null, color, angle, Vector2.Zero, new Vector2(distance, thickness), SpriteEffects.None, 0);
    }
    
    private Texture2D CreatePixelTexture()
    {
        var texture = new Texture2D(_graphicsDevice, 1, 1);
        texture.SetData(new[] { Color.White });
        return texture;
    }
} 