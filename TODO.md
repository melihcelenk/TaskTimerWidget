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

## 🎯 Şu Anki Durum: FAZ 3 - Bug Fix Aşaması (v0.3.1)

**Nerede olduğumuz:** Uygulama build oluyor ve çalışıyor. Temel UI ve işlevsellik var. Ama 8 adet bug tespit edildi.

**Yapı:**
- ✅ Faz 0.1: Proje kuruldu
- ✅ Faz 0.2: Models, Services, ViewModels tamamlandı
- 🔧 **Faz 0.3.1: 8 BUG FIX (ŞU ANDA BURADADAYIZ)**
- ⏳ Faz 0.4+: Sonrasında devam edecek

---

## 🔧 Faz 0.3.1: Bug Fix Listesi (Kaçıncı adımda olduğunu görmek için)

### BUG #1: Timer Sayacı 2 Saniye Artıyor
- [ ] Dosya: `ViewModels/MainViewModel.cs` - `UpdateActiveTaskTimer()` methodu
- [ ] Problem: `UpdateElapsedDisplay()` + `AddElapsedTimeAsync()` double count yapıyor
- [ ] Çözüm: `AddElapsedTimeAsync()` çağrısını `UpdateTaskAsync()` ile değiştir
- Status: ⏳ BAŞLANMADI

### BUG #2: Pencere Boyutu Çok Büyük (Widget boyuta küçült)
- [ ] Dosya: `Views/MainWindow.xaml` - Window tag'ı
- [ ] Problem: Width ve Height attribute'ları yok
- [ ] Çözüm: `Width="380" Height="600"` ekle
- Status: ⏳ BAŞLANMADI

### BUG #3: Sil Butonundaki X Karakteri Kesiliyor
- [ ] Dosya: `Views/MainWindow.xaml` - Delete Button (line ~113)
- [ ] Problem: Width="32" Height="32" çok dar
- [ ] Çözüm: Width="36" Height="36" yap, Padding="0" ekle
- Status: ⏳ BAŞLANMADI

### BUG #4: Taska Tıklandığında Sarı Olmuyoruz
- [ ] Dosya: `Views/MainWindow.xaml` - Border tag'ı (line ~62)
- [ ] Problem: Background sabit gri renk, binding yok
- [ ] Çözüm: Background binding ekle (IsActive property'e bağla)
- [ ] Not: Converter yazılmış, sadece binding yapılacak
- Status: ⏳ BAŞLANMADI

### BUG #5: Task Statüsü "Running/Paused" Gösterilmiyor
- [ ] Dosya: `Views/MainWindow.xaml` - StatusText TextBlock (line ~105)
- [ ] Problem: Sabit "Status" text, binding yok
- [ ] Çözüm: Text binding ekle (IsRunning property'e bağla converter ile)
- Status: ⏳ BAŞLANMADI

### BUG #6: "No Tasks Yet" Gösterilmiyor
- [ ] Dosya: `Views/MainWindow.xaml` - EmptyStatePanel (line ~135)
- [ ] Problem: Sabit Visibility="Visible", dinamik değil
- [ ] Çözüm: Visibility binding ekle (TaskCount'a bağla converter ile)
- Status: ⏳ BAŞLANMADI

### BUG #7: Diğer Taskların Sarı Rengi Güncellenmiyor
- [ ] Dosya: `Views/MainWindow.xaml.cs` - UpdateTaskItemColors() (line ~124)
- [ ] Problem: Method boş, işlemi yapmuyor
- [ ] Çözüm: Binding'den handle edilecek (BUG #4 ile beraber çözülecek)
- Status: ⏳ BAŞLANMADI

### BUG #8: Pencere Chrome'u Kaldırılmadı (Title bar vs)
- [ ] Dosya: `Views/MainWindow.xaml` ve `MainWindow.xaml.cs`
- [ ] Problem: Penceredede minimize/maximize/close butonları var, title bar var
- [ ] Çözüm: ExtendsContentIntoTitleBar ve custom title bar yapması gerekiyor
- [ ] Not: Bunu son yapacağız (en karmaşık)
- Status: ⏳ BAŞLANMADI

---

## 📊 İlerleme Durumu

**Toplam 8 Bug:**
- ✅ Tamamlanan: 0
- 🔄 Yapılıyor: 0
- ⏳ Başlanmamış: 8

**Sıra:** Bug #1 → Bug #2 → Bug #3 → Bug #4 → Bug #5 → Bug #6 → Bug #7 → Bug #8

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

## 📝 Notlar
- Her faz tamamlanırsa version bump et
- Clean code prensiplerine uy (CLAUDE.md)
- Regular commit et (meaningful messages)
- Code review'den geçir (if team)
- Tests yazılmadan feature complete sayma