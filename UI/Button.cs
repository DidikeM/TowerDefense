using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;

namespace TowerDefense.UI;

public class Button : UIElement
{
    private Texture2D? _texture;
    private SpriteFont? _font;
    private string _text;
    private Color _normalColor;
    private Color _hoverColor;
    private Color _pressedColor;
    private Color _currentColor;
    private bool _isPressed;
    private bool _wasPressed;
    
    public event Action? OnClick;
    
    public string Text
    {
        get => _text;
        set => _text = value;
    }
    
    public Button(string text = "", Color? normalColor = null, Color? hoverColor = null, Color? pressedColor = null)
    {
        _text = text;
        _normalColor = normalColor ?? Color.Gray;
        _hoverColor = hoverColor ?? Color.LightGray;
        _pressedColor = pressedColor ?? Color.DarkGray;
        _currentColor = _normalColor;
    }
    
    public void SetTexture(Texture2D texture)
    {
        _texture = texture;
    }
    
    public void SetFont(SpriteFont font)
    {
        _font = font;
    }
    
    public override void Update(GameTime gameTime)
    {
        if (!IsEnabled) return;
        
        var mouseState = Mouse.GetState();
        var mousePosition = new Vector2(mouseState.X, mouseState.Y);
        
        bool isHovered = Contains(mousePosition);
        bool isCurrentlyPressed = isHovered && mouseState.LeftButton == ButtonState.Pressed;
        
        if (isHovered)
        {
            _currentColor = isCurrentlyPressed ? _pressedColor : _hoverColor;
        }
        else
        {
            _currentColor = _normalColor;
        }
        
        if (_wasPressed && !isCurrentlyPressed && isHovered)
        {
            OnClick?.Invoke();
        }
        
        _wasPressed = isCurrentlyPressed;
    }
    
    public override void Draw(SpriteBatch spriteBatch)
    {
        if (!IsVisible) return;
        
        if (_texture != null)
        {
            spriteBatch.Draw(_texture, Bounds, _currentColor);
        }
        else
        {
            spriteBatch.FillRectangle(Bounds, _currentColor);
        }
        
        if (_font != null && !string.IsNullOrEmpty(_text))
        {
            var textSize = _font.MeasureString(_text);
            var textPosition = new Vector2(
                Position.X + (Size.X - textSize.X) / 2,
                Position.Y + (Size.Y - textSize.Y) / 2
            );
            spriteBatch.DrawString(_font, _text, textPosition, Color.White);
        }
    }
} 