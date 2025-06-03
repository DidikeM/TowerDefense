# Tower Defense Harita Tasarımı - Level 1

Bu doküman `level1.tmx` harita dosyasının açıklamasını ve kullanılan sprite sheet ile ilgili bilgileri içerir.

## Harita Özellikleri

- **Boyut**: 50x30 tile (800x480 piksel)
- **Tile Boyutu**: 16x16 piksel
- **Sprite Sheet**: `roguelikeSheet_transparent.png`
- **Tile Margin**: 1 piksel
- **Tile Spacing**: 1 piksel

## Katmanlar (Layers)

### 1. Ground (Zemin Katmanı)
- **Tile ID**: 115 (Çim/zemin tile'ı)
- **Açıklama**: Tüm haritanın zemin dokusu

### 2. Path (Yol Katmanı)
- **Tile ID**: 887 (Taş yol tile'ı)
- **Açıklama**: Düşmanların hareket edeceği yol
- **Yol Güzergahı**: 
  - Başlangıç: Sol orta (x:0, y:160)
  - İlk dönüş: Sağa doğru (x:256, y:160)
  - İkinci dönüş: Aşağı doğru (x:256, y:272)
  - Üçüncü dönüş: Sağa doğru (x:656, y:272)
  - Dördüncü dönüş: Aşağı doğru (x:656, y:368)
  - Bitiş: Sağ alt (x:800, y:368)

### 3. Decorations (Dekorasyon Katmanı)
- **Ağaç Tile ID**: 516 (Tek ağaç)
- **Büyük Ağaç Tile ID**: 573-574 (üst), 630-631 (alt)
- **Açıklama**: Haritaya görsel zenginlik katan dekoratif objeler
- **Dağılım**: Yol etrafında ve harita kenarlarında stratejik yerleştirme

## Object Katmanları

### PathPoints (Yol Noktaları)
6 adet yol noktası objesi içerir:
1. **PathPoint1**: (0, 160) - Başlangıç noktası
2. **PathPoint2**: (256, 160) - İlk dönüş
3. **PathPoint3**: (256, 272) - İkinci dönüş  
4. **PathPoint4**: (656, 272) - Üçüncü dönüş
5. **PathPoint5**: (656, 368) - Dördüncü dönüş
6. **PathPoint6**: (800, 368) - Bitiş noktası

### TowerPositions (Kule Pozisyonları)
14 adet stratejik kule yerleştirme noktası:
- **Üst sıra**: (64,64), (160,64), (320,64), (416,64), (512,64), (608,64), (704,64)
- **Orta bölge**: (320,192), (416,192), (512,192)
- **Alt sıra**: (64,320), (160,320)
- **Sağ bölge**: (720,256), (720,320)

## Kullanılan Sprite Sheet Tile'ları

### Zemin Tile'ları
- **115**: Çim zemin (yeşil, düz)

### Yol Tile'ları  
- **887**: Taş yol (gri, döşeme)

### Dekorasyon Tile'ları
- **516**: Küçük ağaç (tek tile)
- **573**: Büyük ağaç sol üst
- **574**: Büyük ağaç sağ üst
- **630**: Büyük ağaç sol alt
- **631**: Büyük ağaç sağ alt

## Oyun Mekanikleri

### Düşman Hareketi
- Düşmanlar PathPoint1'den başlayıp PathPoint6'ya doğru hareket ederler
- Yol toplam 6 segmentten oluşur ve L şeklinde kıvrımlar içerir
- Bu tasarım kulelerin etkili savunma pozisyonları oluşturmasını sağlar

### Kule Yerleştirme
- 14 önceden tanımlanmış kule pozisyonu mevcuttur
- Pozisyonlar yolun etrafında stratejik noktalarda yerleştirilmiştir
- Hem yolun başlangıcını hem de sonunu kontrol edebilecek şekilde dağıtılmıştır

### Görsel Tasarım
- Doğal bir orman ortamı atmosferi
- Ağaçlar yoğunluğu yol etrafında azaltılarak gameplay'e odaklanma sağlanmıştır
- Harita kenarlarında daha yoğun dekorasyon ile çerçeveleme yapılmıştır

## Kullanım

```csharp
// MapManager'da haritayı yüklemek için:
mapManager.LoadMap("level1");
```

Bu harita dosyası MonoGame.Extended'ın Tiled entegrasyonu ile uyumludur ve oyunda doğrudan kullanılabilir. 