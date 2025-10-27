# TaskTimerWidget - Proje Planı ve Geliştirme Roadmap

## 🎯 Proje Amacı
Masaüstü'nde çalışan, widget benzeri küçük bir uygulama ile task'lar oluşturup timer sayacı tutabileceğiniz ve taskları yönetebileceğiniz bir Windows Store uygulaması.

## 📋 Faz 1: Proje Kurulumu ve Temelleri (v0.1)

### Geliştirme Ortamı
- [ ] Visual Studio 2022 Community/Professional yüklü
- [ ] .NET 8 SDK yüklü
- [ ] WinUI 3 project templates kurulu
- [ ] Git repository init (opsiyonel)

### Proje Yapısı
- [x] Klasör yapısı oluşturuldu
- [x] CLAUDE.md oluşturuldu
- [x] TODO.md oluşturuldu
- [x] MARKET_RESEARCH.md oluşturuldu
- [ ] .gitignore oluştur
- [ ] README.md oluştur
- [ ] .csproj dosyası oluştur ve konfigure et

## 📋 Faz 2: Temel Model ve Mantık (v0.2)

### Models
- [ ] `Task.cs` - Task modeli oluştur
  - Properties: Id, Name, ElapsedSeconds, IsRunning, CreatedAt, Color
  - ToString(), Equals(), GetHashCode() implement et

### Services
- [ ] `TaskService.cs` - Task yönetim servisi
  - AddTask(string name)
  - RemoveTask(Guid id)
  - GetAllTasks()
  - UpdateTask(Task task)
  - Save/Load tasks (local storage)

### ViewModels
- [ ] `MainViewModel.cs` - Ana ViewModel
  - ObservableCollection<TaskViewModel> Tasks
  - Task add/remove/update commands
  - Property Change notification implement et

- [ ] `TaskViewModel.cs` - Tekil task ViewModel
  - StartTimer() / PauseTimer()
  - Update elapsed time
  - IsRunning property

## 📋 Faz 3: UI ve Kullanıcı Arayüzü (v0.3)

### XAML ve Views
- [ ] `App.xaml` ve `App.xaml.cs` - Uygulama ayarları
  - DispatcherTimer setup
  - Window size (widget)
  - Always on top option

- [ ] `MainWindow.xaml` - Ana pencere
  - Task listesi (ListView/ItemsControl)
  - + Butonu (yeni task ekle)
  - Minimize/close butonları

- [ ] `TaskItemView.xaml` - Task item şablonu
  - Task adı etiketi
  - Timer göstergesi
  - Renk değişimi (normal/sarı-active)
  - Hover efektleri

### Kod-Behind
- [ ] MainWindow.xaml.cs - Window işlemleri
- [ ] Event handling - Task click, add button vb.

## 📋 Faz 4: Timer ve State Yönetimi (v0.4)

### Timer Implementation
- [ ] System.Timers.Timer yada DispatcherTimer kullan
  - 100ms refresh rate
  - UI thread safe

### State Management
- [ ] Single active task at a time
  - Diğer task'ların timer'ı durdur
  - Renk state'ini update et

- [ ] Click Events
  - Task tıklanırsa: active yap, timer başlat
  - Aktif task'a tekrar tıklanırsa: pause yap
  - Başka task'a tıklanırsa: öncekini pause, yeni başlat

- [ ] Input Dialog
  - Yeni task dialog'u (TextBox + OK/Cancel)
  - Özel bir CustomInputDialog veya ContentDialog kullan

## 📋 Faz 5: Veri Kalıcılığı (v0.5)

### Local Storage
- [ ] Tasks JSON olarak localStorage'a kaydet
  - ApplicationData.Current.LocalFolder kullan
  - Serialize/Deserialize (System.Text.Json)

- [ ] Uygulama açılışında tasks yükle
- [ ] Her değişiklikte kaydet

### Settings
- [ ] Always on top preference
- [ ] Widget size preferences
- [ ] Theme (light/dark) seçeneği

## 📋 Faz 6: Styling ve UX İyileştirmeleri (v0.6)

### Visual Design
- [ ] Modern Windows 11 design uygulanacak
  - Mica background (optional)
  - Fluent Design System

- [ ] Color Scheme
  - Normal task: Light gray
  - Active task (sarı): #FFD700 veya similar
  - Accent color: Windows theme color

- [ ] Animations
  - Task item appear/disappear
  - Timer count smooth update
  - Color transition smooth

### Responsive UI
- [ ] Widget minimum size: 300x200
- [ ] Widget maximum size: 500x800
- [ ] Resizable window

## 📋 Faz 7: Hata Yönetimi ve Logging (v0.7)

### Error Handling
- [ ] Try-catch blocks strategik yerlere
- [ ] User-friendly error messages
- [ ] Graceful degradation

### Logging
- [ ] Serilog veya Microsoft.Extensions.Logging kullan
- [ ] Debug/Release configuration
- [ ] Log levels: Debug, Info, Warning, Error

## 📋 Faz 8: Testing ve Kalite Assurance (v0.8)

### Unit Tests
- [ ] TaskService tests
- [ ] MainViewModel tests
- [ ] Timer logic tests
- [ ] Storage tests (mock filesystem)

### Integration Tests
- [ ] UI interaction tests
- [ ] End-to-end scenario tests

### Performance Testing
- [ ] Memory leak check
- [ ] CPU usage monitoring
- [ ] Startup time < 2 seconds

## 📋 Faz 9: Windows Store Hazırlığı (v0.9)

### Proje Konfigurasyonu
- [ ] Package.appxmanifest düzenle
  - Display name: "Task Timer Widget"
  - Publisher info
  - Version: 1.0.0.0
  - Entry point doğru

- [ ] Capabilities ayarla
  - İhtiyaçlı capabilities ekle
  - Privacy policy oluştur

### Assets ve Icons
- [ ] Application icon (256x256, PNG)
- [ ] Store logo (120x120, PNG)
- [ ] Screenshot'lar (3-5 adet, 1080x1620)
- [ ] Feature graphic (1200x628, PNG)
- [ ] Description yazısı

### Certification
- [ ] Microsoft Store Certification Kit'i çalıştır
- [ ] All checks pass olana kadar fix et

### Submission Hazırlığı
- [ ] Privacy Policy URL'si hazırla
- [ ] Description ve keywords write et
- [ ] Category: Productivity
- [ ] Age rating: 3+
- [ ] Requirements belirt

## 📋 Faz 10: Beta Testing ve Release (v1.0)

### Beta Testing
- [ ] 5-10 tester ile beta test
- [ ] Feedback topla
- [ ] Critical bugs fix et

### Performance Optimization
- [ ] Code review
- [ ] Final optimizations
- [ ] Build optimization

### Release
- [ ] Final build ve test
- [ ] Store'a submit et
- [ ] Launch announcement
- [ ] User support setup

## 📋 Post-Launch: Maintenance ve Updates (v1.1+)

### Feature Requests (v1.1)
- [ ] Task category/tags
- [ ] Statistics dashboard
- [ ] Export to CSV/Excel
- [ ] Notifications/reminders
- [ ] Cloud sync (OneDrive/iCloud)

### Improvements
- [ ] Performance optimizations
- [ ] Bug fixes (user feedback)
- [ ] UI/UX improvements
- [ ] Accessibility improvements
- [ ] Localization (TR, EN, vb)

## 🎯 Şu Anki Durum: FAZ 3 - Design Revamp Aşaması (v0.3.2-v0.3.3)

**Nerede olduğumuz:** Bug fix fası tamamlandı (5/6 bug). Şimdi minimal design revampına başlıyoruz.

**Yapı:**
- ✅ Faz 0.1: Proje kuruldu
- ✅ Faz 0.2: Models, Services, ViewModels tamamlandı
- ✅ **Faz 0.3.1: 8 BUG FIX TAMAMLANDI**
- 🎨 **Faz 0.3.2: DESIGN REVAMP A - Colors & Typography (ŞU ANDA BURADADAYIZ)**
- 🎨 **Faz 0.3.3: DESIGN REVAMP B - UX & Layout (SONRASINDA)**
- ⏳ Faz 0.4+: Sonrasında devam edecek

---

## 🔧 Faz 0.3.1: Bug Fix Listesi (Kaçıncı adımda olduğunu görmek için)

### BUG #1: Timer Sayacı 2 Saniye Artıyor
- [x] Dosya: `ViewModels/MainViewModel.cs` - `UpdateActiveTaskTimer()` methodu
- [x] Problem: `UpdateElapsedDisplay()` + `AddElapsedTimeAsync()` double count yapıyor
- [x] Çözüm: `AddElapsedTimeAsync()` çağrısını `UpdateTaskAsync()` ile değiştir
- Status: ✅ TAMAMLANDI

### BUG #2: Pencere Boyutu Çok Büyük (Widget boyuta küçült)
- [x] Dosya: `Views/MainWindow.xaml.cs` - InitializeWindow() methodu
- [x] Problem: Window boyutu ayarlanmamış
- [x] Çözüm: AppWindow.Resize() ile 380x600'e ayarla
- Status: ✅ TAMAMLANDI

### BUG #3: Diğer Taskların Rengi Güncellenmiyor
- [x] Dosya: `Views/MainWindow.xaml.cs` - UpdateTaskItemColors() methodu
- [x] Problem: Background sabit gri renk, dinamik güncelleme yok
- [x] Çözüm: Visual Tree traversal ile tüm items'ın rengini güncelle
- [x] Not: Aktif task sarı, diğerleri açık gri oldu
- Status: ✅ TAMAMLANDI

### BUG #4: "No Tasks Yet" Gösterilmiyor
- [x] Dosya: `Views/MainWindow.xaml.cs` - InitializeViewModel() methodu
- [x] Problem: EmptyStatePanel ilk açılışta görünüyor, yeni task eklenmiş olsa da kapanmıyor
- [x] Çözüm: Tasks.CollectionChanged event'ini subscribe ederek dinamik güncelle
- Status: ✅ TAMAMLANDI

### BUG #5: Task Statüsü "Running/Paused" Gösterilmiyor
- [x] ATILDI - Running/Paused satırı kaldırılacak, renk ile ifade edilecek
- Status: ✅ ATILDI (Design Revamp'ta ele alınacak)

### BUG #6: Sil Butonundaki X Karakteri Kesiliyor
- [x] Dosya: `Views/MainWindow.xaml` - Delete Button (line ~113)
- [x] Problem: Width="32" Height="32" çok dar, X karakteri kesiliyor
- [x] Çözüm: Width="36" Height="36", FontSize="16", Padding="0" ekle
- Status: ✅ TAMAMLANDI

### BUG #7: Pencere Chrome'u Kaldırılmadı (Title bar vs)
- [ ] Dosya: `Views/MainWindow.xaml` ve `MainWindow.xaml.cs`
- [ ] Problem: Penceredede minimize/maximize/close butonları var, title bar var
- [ ] Çözüm: ExtendsContentIntoTitleBar ve custom title bar yapması gerekiyor
- [ ] Not: WinUI 3 title bar management karmaşık, sonraya alındı
- Status: ⏳ ERTELENDI (Son yapılacak, karmaşık)

### BUG #8: (Yeni gerekli mi?)
- Status: ⏳ PLANLANACAK

---

## 📊 İlerleme Durumu

**Tamamlanan:** 5 / 6 Bug
- ✅ BUG #1: Timer Sayacı 2 Saniye Artıyor
- ✅ BUG #2: Pencere Boyutu Çok Büyük
- ✅ BUG #3: Diğer Taskların Rengi Güncellenmiyor
- ✅ BUG #4: "No Tasks Yet" Gösterilmiyor
- ✅ BUG #6: Sil Butonundaki X Kesiliyor
- ❌ BUG #5: ATILDI (Design Revamp'ta ele alınacak)

**Ertelenen:**
- ⏳ BUG #7: Pencere Chrome'u Kaldırılmadı (WinUI 3 title bar karmaşık, sonra yapılacak)

Her bug'ı tamamladıktan sonra:
1. Uygulamayı test et
2. Sorun yoksa commit et
3. Sonraki bug'a geç

---

## 🎯 Milestone Timeline

| Faz | Adı | Hedef Tarih | Status |
|-----|-----|-----------|--------|
| 0.1 | Kurulum | - | ✅ Tamamlandı |
| 0.2 | Model & Logic | - | ✅ Tamamlandı |
| **0.3.1** | **Bug Fix (ŞU ANDA)** | - | **🔧 Devam Ediyor** |
| 0.3 | UI Design | - | ⏳ Beklemede |
| 0.4 | Timer Impl. | - | ⏳ Beklemede |
| 0.5 | Data Persist. | - | ⏳ Beklemede |
| 0.6 | Styling | - | ⏳ Beklemede |
| 0.7 | Error Handling | - | ⏳ Beklemede |
| 0.8 | Testing | - | ⏳ Beklemede |
| 0.9 | Store Ready | - | ⏳ Beklemede |
| 1.0 | Release | - | ⏳ Beklemede |

---

## 🎨 Faz 0.3.2: Minimal Design Revamp A - Colors & Typography

### UI/UX İyileştirmeleri (A)
- [x] Koyu gri kartlar (Dark Gray background #2A2A2A)
- [x] Kalın beyaz başlık (Bold white text for task name)
- [x] Minimal timer gösterimi (sadece "3m 6s" şeklinde, saat olmadıkça saat gösterme)
- [x] Running/Paused satırını kaldır (renk ile halledecek - sarı=running, siyah=paused)
- [x] XAML Resources syntax'i fix et (ResourceDictionary.MergedDictionaries kullan)
- [x] RunningTextColorConverter ekle ve uygulamaya bağla
- [x] Text rengi binding'i IsActive'e çevir (IsRunning yerine)
- [x] Hover effect'i hafif aydınlık (#3A3A3A) yap
- [x] Timer font normal (bold değil) ve IsActive binding
- [x] Yeni task ekleme sonrası auto-select kaldır (non-active başlanması için)

**Status:** ✅ TAMAMLANDI - Build başarılı, renk sistemi aktif, tüm text renkleri tutarlı

---

## 🎨 Faz 0.3.3: Minimal Design Revamp B - UX & Layout

### UI/UX İyileştirmeleri (B)
- [x] "Task Timer" başlığı kaldırıldı (gereksiz kalabalık)
- [x] + Butonu: Direkt inline textbox aç, ikinci tıklaşta task ekle
- [x] Enter tuşu: Task adını kaydet
- [x] Esc tuşu: Input'u iptal et
- [x] Minimum efor, maksimum kullanılabilirlik
- [x] Dialog kaldırıldı, inline editing ile değiştirildi

### Tasarım Referansı
- Python versiyonundan esinlenildi
- Göze çarpan minimal tasarım
- Temiz ve sade UI

**Status:** ✅ TAMAMLANDI

## 📝 Notlar
- Her faz tamamlanırsa version bump et
- Clean code prensiplerine uy (CLAUDE.md)
- Regular commit et (meaningful messages)
- Code review'den geçir (if team)
- Tests yazılmadan feature complete sayma