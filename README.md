# Tower Defense Game

MonoGame ve MonoGame.Extended kullanılarak geliştirilmiş 2D top-down tower defense oyunu.

## Özellikler

- **Sahne Yönetimi**: Ana menü ve oyun sahneleri arası geçiş
- **UI Sistemi**: Button, Panel gibi temel UI bileşenleri
- **Tiled Entegrasyonu**: Tiled Map Editor ile oluşturulan haritaları okuma
- **Düşman Sistemi**: Yol boyunca hareket eden düşmanlar
- **Kule Sistemi**: Düşmanları otomatik hedef alan kuleler
- **Dalga Sistemi**: Artan zorlukla düşman dalgaları

## Proje Yapısı

```
TowerDefense/
├── Scenes/                 # Sahne yönetimi
│   ├── IScene.cs          # Sahne interface'i
│   ├── SceneManager.cs    # Sahne yöneticisi
│   ├── MainMenuScene.cs   # Ana menü sahnesi
│   └── GameScene.cs       # Oyun sahnesi
├── UI/                    # Kullanıcı arayüzü
│   ├── UIElement.cs       # UI base class
│   ├── Button.cs          # Buton bileşeni
│   └── Panel.cs           # Panel bileşeni
├── GameObjects/           # Oyun nesneleri
│   ├── GameObject.cs      # Base oyun nesnesi
│   ├── Enemies/           # Düşman sınıfları
│   │   ├── Enemy.cs       # Düşman base class
│   │   └── BasicEnemy.cs  # Temel düşman
│   └── Towers/            # Kule sınıfları
│       ├── Tower.cs       # Kule base class
│       └── BasicTower.cs  # Temel kule
├── Map/                   # Harita yönetimi
│   └── MapManager.cs      # Tiled harita yöneticisi
└── Content/               # Oyun varlıkları
    ├── Maps/              # Tiled harita dosyaları
    ├── Fonts/             # Font dosyaları
    └── Sprites/           # Görsel varlıklar
```

## Kurulum

### Gereksinimler

- .NET 8.0 SDK
- MonoGame Framework
- MonoGame.Extended
- Visual Studio veya VS Code

### Adımlar

1. Repoyu klonlayın:
```bash
git clone <repo-url>
cd TowerDefense
```

2. Paketleri yükleyin:
```bash
dotnet restore
```

3. Oyunu çalıştırın:
```bash
dotnet run
```

## Content Pipeline

### Font Ekleme

1. `Content` klasöründe `Fonts` klasörü oluşturun
2. `.spritefont` dosyalarını ekleyin:
   - `TitleFont.spritefont` - Ana menü başlığı için
   - `ButtonFont.spritefont` - Butonlar için
   - `GameFont.spritefont` - Oyun içi metin için

### Sprite Ekleme

1. `Content` klasöründe `Sprites` klasörü oluşturun
2. Texture dosyalarını ekleyin:
   - `Enemy.png` - Düşman sprite'ı
   - `Tower.png` - Kule sprite'ı

### Tiled Harita Ekleme

1. `Content` klasöründe `Maps` klasörü oluşturun
2. Tiled ile oluşturulan `.tmx` dosyalarını ekleyin
3. Content.mgcb dosyasında haritaları ekleyin

#### Tiled Harita Katmanları

Haritalarınız şu katmanları içermelidir:

- **Path**: Düşman yolu
  - Objeler: "PathPoint" tipinde
  - Properties: "order" (sıra numarası)
  
- **TowerPositions**: Kule yerleştirme noktaları
  - Objeler: "TowerPosition" tipinde

## Oyun Mekanikleri

### Düşmanlar

- Belirlenen yol boyunca hareket ederler
- Can ve hız özellikleri vardır
- Hedefe ulaştıklarında oyuncuya hasar verirler
- Öldürüldüklerinde para ödülü verirler

### Kuleler

- Belirli pozisyonlara yerleştirilebilir
- Menzilleri içindeki düşmanları otomatik hedef alırlar
- Upgrade edilebilir (gelecek özellik)
- Para karşılığında satın alınırlar

### Dalga Sistemi

- Her dalga bittiğinde yeni dalga başlar
- Her dalga daha fazla düşman içerir
- Düşman spawn süresi kısalır

## Geliştirme

### Yeni Düşman Türü Ekleme

```csharp
public class FastEnemy : Enemy
{
    public FastEnemy() : base(health: 50f, speed: 100f, reward: 15, damage: 1f)
    {
        Size = new Vector2(15, 15);
    }
}
```

### Yeni Kule Türü Ekleme

```csharp
public class SniperTower : Tower
{
    public SniperTower() : base(range: 200f, damage: 100f, fireRate: 0.5f, cost: 150)
    {
        Size = new Vector2(35, 35);
    }
}
```

### Yeni Sahne Ekleme

```csharp
public class GameOverScene : IScene
{
    // IScene interface'ini implement edin
}
```

## Klavye Kontrolleri

- **ESC**: Ana menüye dön (oyun içindeyken)
- **Sol Tık**: Kule yerleştir
- **Alt + F4**: Oyundan çık

## Debug Özellikleri

Debug modunda ek özellikler aktiftir:

- Düşman yolu kırmızı çizgi ile gösterilir
- Kule pozisyonları yeşil kareler ile gösterilir
- Kule menzilleri görünür olur

## Katkıda Bulunma

1. Fork edin
2. Feature branch oluşturun (`git checkout -b feature/YeniOzellik`)
3. Değişikliklerinizi commit edin (`git commit -am 'Yeni özellik eklendi'`)
4. Branch'e push edin (`git push origin feature/YeniOzellik`)
5. Pull Request oluşturun

## Lisans

Bu proje MIT lisansı altında lisanslanmıştır. 