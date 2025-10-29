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

---

## ⏳ Yapılacak Fazlar (v1.0 Release)

### Faz 0.7: Order Persistence (Tahmini: 1-2 saat) 🔥 KRİTİK
- [ ] Task order'ı JSON'a kaydetme (Order property ekle)
- [ ] Drag-drop sonrası UpdateTaskOrdersAsync() çağır
- [ ] Uygulama açılışında Tasks.OrderBy(t => t.Order) ile sırala
- [ ] Test: Drag-drop yap, uygulamayı kapat-aç, sıralama korunuyor mu?

### Faz 0.8: Basic Testing & Polish (Tahmini: 30-60 dakika) ✅
- [ ] Uzun task isimleri test (TextTrimming çalışıyor mu?)
- [ ] 10-15 task ekle, hepsini test (create, rename, delete, drag-drop)
- [ ] Aç-kapat testi (data persistence)
- [ ] Memory leak basic check (Task Manager'da 1 saat açık bırak)

### Faz 0.9: Windows Store Hazırlığı (Tahmini: 3-4 saat) 📦
- [ ] Application icons (AI/Canva ile 256x256, 150x150, 44x44, 16x16)
- [ ] Store logos (1240x600, 2400x1200)
- [ ] Screenshots (min 3 adet, 1920x1080 - mevcut uygulamadan)
- [ ] Privacy policy (template kullan, GitHub Pages'e koy)
- [ ] Store listing: Short description (EN & TR)
- [ ] Package.appxmanifest configuration
- [ ] MSIX package creation ve signing
- [ ] WACK (Windows App Certification Kit) testi

### Faz 1.0: Final Release (Tahmini: 1 saat) 🚀
- [ ] Version number → 1.0.0.0
- [ ] Release build test
- [ ] Store submission
- [ ] Wait for Microsoft review (1-3 gün)
- [ ] Publish! 🎉

---

## 📋 Version 2.0 Features (Deferred)

### BUG #7: Window Chrome Removal
- ⏳ Ertelendi - WinUI 3 title bar yönetimi karmaşık
- Mevcut durum: Custom title bar kısmen implemented

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

**Current Version**: 0.6
**Status**: UI Complete - Core Features Done ✅
**Next Phase**: 0.7 (Order Persistence) 🔥
**Last Updated**: October 30, 2025

### Session Summary (October 30, 2025)
- ✅ Faz 0.6 tamamlandı (UI Enhancements)
- ✅ Custom font (Trebuchet MS) embedded
- ✅ Time percentage display eklendi
- ✅ Right-click rename functionality
- ✅ Drag-and-drop task reordering
- ✅ Green drop indicator line (tasklar arası kılavuz)
- ✅ Title bar dragging (SetTitleBar)
- ✅ Auto-scroll on new task

### 🎯 Release Yol Haritası (MVP Approach)
**Toplam Kalan Süre: 5-7 saat aktif + 1-3 gün Microsoft review**

- **Faz 0.7**: 1-2 saat → Order Persistence (KRİTİK) 🔥
- **Faz 0.8**: 30-60 dakika → Basic Testing ✅
- **Faz 0.9**: 3-4 saat → Store Hazırlığı (icons, screenshots, MSIX) 📦
- **Faz 1.0**: 1 saat → Final Release & Submission 🚀
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
