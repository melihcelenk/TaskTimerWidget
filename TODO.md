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

---

## ⏳ Yapılacak Fazlar

### Faz 0.6: Styling & Polish
- [ ] Smooth animations (task appear/disappear, color transitions)
- [ ] Windows 11 Mica background (opsiyonel)
- [ ] Fluent Design System uygulanması
- [ ] Responsive UI iyileştirmeleri

### Faz 0.7: Error Handling & Logging
- [ ] Try-catch blocks strategic konumlara
- [ ] User-friendly error messages
- [ ] Serilog configuration iyileştirmeleri
- [ ] Log rotation ve cleanup

### Faz 0.8: Testing & QA
- [ ] Unit tests (TaskService, MainViewModel)
- [ ] Integration tests (UI interactions)
- [ ] Performance testing (memory, CPU usage)
- [ ] Manual QA checklist

### Faz 0.9: Windows Store Hazırlığı
- [ ] Package.appxmanifest configuration
- [ ] Application icons ve assets (256x256, 120x120)
- [ ] Store screenshots (1080x1620)
- [ ] Privacy policy yazısı
- [ ] Microsoft Store Certification Kit

### Faz 1.0: Release
- [ ] Final build ve testing
- [ ] Store submission
- [ ] Launch announcement

---

## 📋 Deferred Items

### BUG #7: Window Chrome Removal
- ⏳ Ertelendi - WinUI 3 title bar yönetimi karmaşık
- Mevcut durum: Custom title bar kısmen implemented

### Future Features (v1.1+)
- [ ] Task categories/tags
- [ ] Statistics dashboard
- [ ] Export to CSV/Excel
- [ ] Notifications/reminders
- [ ] Cloud sync (OneDrive)
- [ ] Localization (TR, EN, etc)

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

**Current Version**: 0.5
**Status**: Feature Complete (Basic Functionality)
**Next Phase**: 0.6 (Styling & Polish)
**Last Updated**: October 28, 2025

### Session Summary (Today)
- ✅ Design Revamp (0.3.1, 0.3.2, 0.3.3) tamamlandı
- ✅ Timer & State Management (0.4) verified
- ✅ Data Persistence (0.5) confirmed working
- ✅ Time format updated to h/m/s style
- ✅ Toggle behavior fixed (active task pause)
- ✅ Hover colors corrected
- ✅ UI layout finalized

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

## ✅ Checklist Before Store Submission

- [ ] All features tested and working
- [ ] No memory leaks or performance issues
- [ ] Error handling complete
- [ ] Unit tests passing
- [ ] Icons and assets ready
- [ ] Privacy policy written
- [ ] Package.appxmanifest configured
- [ ] Microsoft Store Certification passes
