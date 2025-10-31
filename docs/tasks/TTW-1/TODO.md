# TaskTimerWidget - Geliştirme Roadmap

## 🎯 Proje Amacı
Masaüstü'nde çalışan, widget benzeri küçük bir uygulama ile tasklar oluşturup timer sayacı tutabileceğiniz ve taskları yönetebileceğiniz bir Windows Store uygulaması.

---

## ✅ Tamamlanan Fazlar

### Faz 0.1: Proje Kurulumu ✅
- ✅ Klasör yapısı ve proje dosyaları
- ✅ Git repository kurulumu
- ✅ CLAUDE.md, DEVELOPMENT.md, README.md
- ✅ .NET 8 + WinUI 3 kurulumu

### Faz 0.2: Temel Model ve Logic ✅
- ✅ TaskItem model (Id, Name, ElapsedSeconds, IsRunning, CreatedAt)
- ✅ TaskService (CRUD operations)
- ✅ StorageService (JSON persistence)
- ✅ MainViewModel ve TaskViewModel
- ✅ MVVM pattern + INotifyPropertyChanged

### Faz 0.3: Design Revamp ✅

#### 0.3.1: Bug Fixes (6/8) ✅
- ✅ Timer double count hatası düzeltildi
- ✅ Pencere boyutu optimized (220x500)
- ✅ Task renkleri dinamik güncelleme
- ✅ Empty state message görünürlüğü
- ✅ Delete button boyutu ve görünürlüğü

#### 0.3.2: Colors & Typography ✅
- ✅ Koyu gri kartlar (#2A2A2A) + açık gri background (#D0D0D0)
- ✅ Text rengi binding (beyaz=inactive, siyah=active)
- ✅ Timer font normal weight, başlık bold
- ✅ Hover effect hafif aydınlık (#3A3A3A)
- ✅ Active task hover açık sarı (#FFE050)

#### 0.3.3: UX & Layout ✅
- ✅ "Task Timer" başlığı kaldırılıp inline task creation
- ✅ + Butonu aşağıda ortada (#3A3A3A rengi)
- ✅ Yeni task kartı tasklist içinde, diğerleri gibi
- ✅ Delete buton task name'in sağında (üst satırda)
- ✅ TextBox padding düzeltildi (sağdan başlıyor)
- ✅ Enter tuşu: task ekle, Esc: iptal et
- ✅ Dialog kaldırılıp inline editing

### Faz 0.4: Timer & State Management ✅
- ✅ DispatcherTimer ile real-time sayaç
- ✅ Single active task at a time
- ✅ Task tıklaması: active yap, tekrar tık: pause
- ✅ Başka task tıklaması: öncekini pause, yenisini başlat
- ✅ Toggle sırasında renk güncelleniyor
- ✅ Süre formatı (1h 30m 5s, 23m 12s, 35s)

### Faz 0.5: Veri Kalıcılığı ✅
- ✅ Tasks JSON olarak `%LOCALAPPDATA%\TaskTimerWidget\Data\tasks.json` kaydediliyor
- ✅ Uygulama açılışında önceki tasklar yükleniyor
- ✅ Her değişiklikte otomatik save (create, update, delete)

### Faz 0.6: UI Enhancements ✅
- ✅ Custom font integration (Trebuchet MS embedded)
- ✅ Time percentage display (her task'ın toplam içindeki oranı)
- ✅ Right-click rename functionality
- ✅ Drag-and-drop task reordering
- ✅ Drop indicator line (tasklar arası yeşil çizgi kılavuz)
- ✅ Title bar dragging (SetTitleBar ile native sürükleme)
- ✅ Auto-scroll to bottom on new task

### Faz 0.7: Order Persistence ✅
- ✅ Task order'ı JSON'a kaydetme (Order property eklendi)
- ✅ Drag-drop sonrası UpdateTaskOrdersAsync() çağrılıyor
- ✅ Uygulama açılışında Tasks.OrderBy(t => t.Order) ile sıralama
- ✅ Test: Drag-drop persistence çalışıyor

### Faz 0.8: UI Fixes & Polish ✅
- ✅ Multi-line text wrapping for long task names (max 3 lines)
- ✅ Fix: Rename sonrası active task rengi korunuyor
- ✅ Fix: Add new task butonu rename sonrası çalışıyor
- ✅ Auto-scroll when opening new task card

### Faz 0.9: Basic Testing ✅
- ✅ Uzun task isimleri test edildi
- ✅ Create, rename, delete, drag-drop test edildi
- ✅ Data persistence ve order persistence çalışıyor

---

## ⏳ Yapılacak Fazlar (v1.0 Release)

### Faz 1.0: Windows Store Hazırlığı (Tahmini: 3-4 saat) 📦
- [✅] Application icons (AI ile oluşturuldu ve 4 boyuta resize edildi: 256x256, 150x150, 44x44, 16x16)
  - [✅] Icon'lar Assets klasörüne kopyalandı
  - [✅] .csproj'a asset referansları eklendi
  - [✅] app.ico oluşturuldu (taskbar icon için)
  - [✅] MainWindow.xaml.cs'de runtime icon ayarı yapıldı
  - [✅] Taskbar'da icon görünüyor (kalite iyileştirme gerekli - online converter ile)
- [✅] Store logos (1240x600, 2400x1200)
- [✅] .gitignore'dan Assets/ kaldırıldı
- [✅] README_ASSETS.txt güncellendi
- [ ] Screenshots (min 3 adet, 1920x1080 - mevcut uygulamadan) ⚠️ **Manuel gerekli**
- [✅] Privacy policy (PRIVACY_POLICY.md oluşturuldu)
  - [✅] GitHub username güncelle (melihcelenk)
  - [ ] GitHub Pages'e yükle ve URL al (repo public yapıldıktan sonra)
- [✅] Store listing: Short description (EN & TR) (STORE_LISTING.md oluşturuldu)
  - [✅] GitHub username güncelle (melihcelenk)
  - [✅] Privacy Policy URL'i eklendi (https://melihcelenk.github.io/TaskTimerWidget/PRIVACY_POLICY.html)
  - [✅] Support URL güncellendi (https://github.com/melihcelenk/TaskTimerWidget)
- [✅] Package.appxmanifest configuration (Version 1.0.0.0, Publisher: Melih Celenk)
- [ ] MSIX package creation ve signing ⚠️ **Manuel gerekli** (Signing için sertifika gerekli)
- [ ] WACK (Windows App Certification Kit) testi ⚠️ **Manuel gerekli**

### Faz 1.1: Final Release (Tahmini: 1 saat) 🚀
- [✅] Version number → 1.0.0.0 (Package.appxmanifest'te ayarlandı)
- [ ] Release build test ⚠️ **Manuel gerekli**
- [ ] Store submission ⚠️ **Manuel gerekli** (Microsoft hesabı gerekli)
- [ ] Wait for Microsoft review (1-3 gün)
- [ ] Publish! 🎉

---

## 📋 Version 2.0 Features (Deferred)

### Widget Behavior (v2.0)
- [✅] Always-on-top window (implemented in v1.0)
- [ ] System tray icon (H.NotifyIcon.WinUI kompleks - ertelendi)
- [ ] Minimize to tray (system tray ile birlikte)
- [ ] Windows startup (MSIX için karmaşık - ertelendi)

### Polish & Animations (v2.0)
- [ ] Smooth animations (task appear/disappear, fade in/out)
- [ ] Color transitions (hover, active state)
- [ ] Drop indicator animation (smooth slide)
- [ ] Button hover effects polish

### Error Handling & Logging (v2.0)
- [ ] Comprehensive try-catch blocks
- [ ] User-friendly error messages (toast notifications)
- [ ] Log rotation ve cleanup
- [ ] Crash recovery (corrupted JSON handling)

### Advanced Features (v2.0+)
- [ ] Task categories/tags
- [ ] Statistics dashboard (günlük/haftalık raporlar)
- [ ] Export to CSV/Excel
- [ ] Notifications/reminders
- [ ] Cloud sync (OneDrive)
- [ ] Localization (TR, EN, etc)
- [ ] Dark/Light theme toggle
- [ ] Keyboard shortcuts (Ctrl+N: new task, etc)

---

## 🛠️ Development Commands

```bash
# Build
cd C:\Kodlar\Desktop\TaskTimerWidget\src\TaskTimerWidget
dotnet build --configuration Debug

# Kill app and rebuild
powershell -NoProfile -Command "Get-Process TaskTimerWidget -ErrorAction Ignore | Stop-Process -Force -ErrorAction Ignore; Start-Sleep -Milliseconds 500"

# Launch
cd bin\Debug\net8.0-windows10.0.19041.0
start TaskTimerWidget.exe

# Git
git add -A
git commit -m "Faz X.X: Description"
git log --oneline | head -5
```

---

## 📊 Current Status

**Current Version**: 0.9
**Status**: Development & Testing Complete ✅
**Next Phase**: 1.0 (Store Hazırlığı) 📦
**Last Updated**: October 30, 2025

### Session Summary (October 30, 2025)
- ✅ Faz 0.7 tamamlandı (Order Persistence)
- ✅ Faz 0.8 tamamlandı (UI Fixes & Polish)
- ✅ Faz 0.9 tamamlandı (Basic Testing)
  - Tüm core features test edildi ve çalışıyor

### 🎯 Release Yol Haritası (MVP Approach)
**Toplam Kalan Süre: 3-4 saat aktif + 1-3 gün Microsoft review**

- **Faz 1.0**: 3-4 saat → Store Hazırlığı (icons, screenshots, MSIX, privacy policy, store listing) 📦
- **Faz 1.1**: 1 saat → Final Release & Submission 🚀
- **Microsoft Review**: 1-3 gün (pasif bekleme)

### ❌ Version 2.0'a Ertelenen
- Animasyonlar & Polish
- Comprehensive error handling
- Extensive testing
- Unit tests

---

## 🔗 Key Files

- **MainWindow.xaml**: UI layout
- **MainWindow.xaml.cs**: Event handlers, color management
- **MainViewModel.cs**: Task selection, state management
- **TaskViewModel.cs**: Individual task logic
- **StorageService.cs**: JSON persistence
- **ValueConverters.cs**: XAML converters
- **DEVELOPMENT.md**: Development guide & commands

---

## ✅ Store Submission Checklist

### Technical Requirements
- [ ] All features tested and working
- [ ] No memory leaks or performance issues
- [ ] Error handling complete
- [ ] Smooth animations implemented
- [ ] Task order persistence working
- [ ] Multi-session data integrity verified

### Assets & Documentation
- [ ] App icons (256x256, 150x150, 44x44, 16x16) ✨
- [ ] Store logos (1240x600, 2400x1200) ✨
- [ ] Screenshots (min 3, 1366x768 veya daha yüksek) 📸
- [ ] Privacy policy published (URL) 📄
- [ ] Store description written (EN & TR) 📝
- [ ] Feature list prepared 📋
- [ ] What's New / Release notes ✍️

### Store Configuration
- [ ] Package.appxmanifest configured
- [ ] App name finalized
- [ ] Publisher info correct
- [ ] Capabilities declared (File system access)
- [ ] Age rating selected
- [ ] Category selected (Productivity)
- [ ] Pricing (Free)

### Certification
- [ ] MSIX package created and signed 📦
- [ ] Microsoft Store Certification Kit (WACK) passed ✅
- [ ] Release build tested on clean machine 🖥️
- [ ] Final smoke test completed ✔️
