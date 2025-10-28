# TaskTimerWidget - Kod Standartları ve Kuralları

## 📋 Genel Prensipler
Bu proje **Clean Code** prensiplerine uygun şekilde yazılacaktır. Tüm geliştirciler aşağıdaki kurallara kesinlikle uymalıdır.

## 🏗️ Mimari Yapı

### Katmanlar (Layers)
- **Models**: Veri modelleri ve entity'ler
- **ViewModels**: Business logic ve UI state yönetimi (MVVM Pattern)
- **Views**: XAML kullanıcı arayüzü
- **Services**: İş mantığı ve veri işlemleri
- **Helpers**: Utility ve extension metodları

### MVVM Pattern
- Her View, karşılık gelen ViewModel ile birleştirilir
- ViewModel, UI state ve command'ları yönetir
- Model, saf veri nesneleridir ve UI knowledge'ı taşımazlar

## 📝 Kodlama Standartları

### Adlandırma Kuralları
- **Classes**: PascalCase (ör: `TaskItemViewModel`)
- **Methods**: PascalCase (ör: `StartTimer()`)
- **Properties**: PascalCase (ör: `IsRunning`)
- **Fields**: _camelCase private, camelCase public (ör: `_timerInterval`)
- **Constants**: UPPER_SNAKE_CASE (ör: `DEFAULT_TIMER_INTERVAL`)
- **Local Variables**: camelCase (ör: `elapsedTime`)

### C# Kuralları
- **Access Modifiers**: Explicit olarak belirtilmeli (public, private, internal)
- **Properties**: Auto-properties tercih edilir
  ```csharp
  public string TaskName { get; set; }
  ```
- **Null Safety**: Null reference types aktif olacak
- **Async**: Uzun işlemler async yapılmalı (async/await)
- **Disposal**: IDisposable implement edilmeli
- **Logging**: Structured logging kullanılmalı

### Kod Kalitesi
- **Functions**: Tek bir sorumluluk taşımalı (SRP - Single Responsibility Principle)
- **Parameters**: Maksimum 3-4 parametre, daha fazlaysa object parameter kullan
- **Comments**: Sadece "neden" açıklanmalı, "ne" kod tarafından anlaşılmalı
- **DRY**: Don't Repeat Yourself - Tekrar eden kodu extract et
- **KISS**: Keep It Simple, Stupid - Karmaşık olmadan çöz
- **Indentation**: 4 boşluk (Tab yerine spaces)

### Exception Handling
```csharp
try
{
    // İş mantığı
}
catch (SpecificException ex)
{
    // Spesifik hata işleme
    LogError(ex);
    throw;
}
catch (Exception ex)
{
    // Generic hata işleme
    LogError(ex);
}
```

## 📦 Dosya Organizasyonu
- Bir dosya, bir class içerir (bazı exceptions dışında)
- File name = Class name
- Region'lar kullanılabilir ama aşırı kullanılmamalı

## 🔄 MVVM Veri Bağlama
- `INotifyPropertyChanged` implement edilmeli
- Property change'ler observable olmalı
- Commands `ICommand` interface'ini implement etmeli

## 🧪 Testing
- Unit tests yazılmalı (xUnit tercih)
- Mockable dependencies kullan
- Test coverage %70+ olmalı

## 🚀 Performance
- UI thread'ini block etme
- Background tasks async olmalı
- Ressource leak'lerini önle (Dispose pattern)

## 🎨 UI Kuralları (XAML)
- MVVM binding kullan, code-behind'a mantık yazma
- Xaml'de sadece UI ilgili kod olmalı
- Magic numbers XAML'e yazma, constant yap

## 📚 Dokümantasyon
- Public API'ler XML documentation comment'lı olmalı
- Complex logic'te açıklama yap
- Git commit message'leri anlamlı olmalı

## ✅ Checklist Önce Commit
- [ ] Kod formatting kontrol edildi
- [ ] Syntax hataları yok
- [ ] Naming conventions uygulandı
- [ ] Commented/debug kodu kaldırıldı
- [ ] Unit tests pass ediyor
- [ ] Performance uygun
- [ ] Exception handling var
- [ ] Documentation güncellenmiş
- WinUI 3 Custom Window Dragging:
    Use SetTitleBar(UIElement) for smooth, system-integrated dragging
    Guide: docs/WINUI3_WINDOW_DRAGGING.md
    Applies to all C# WinUI 3 projects
    Only 2 lines of code needed