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
- [✅] Screenshots (3 adet, assets/screenshots/ - normal mode, compact mode, rename feature) 📸
- [✅] Privacy policy (PRIVACY_POLICY.md oluşturuldu)
  - [✅] GitHub username güncelle (melihcelenk)
  - [⏳] GitHub Pages'e yükle ve URL al (~5-10 dakika) 🌐
    - [✅] PRIVACY_POLICY.html oluşturuldu (styled, bilingual EN/TR)
    - [✅] Project structure cleaned for public release (legacy/ folder created)
    - [✅] CLAUDE.md updated (commit guidelines clarified - AI suggests only)
    - [✅] Documentation converted to English (Turkish versions in docs/general/tr/)
    - [✅] Removed outdated SETUP_INSTRUCTIONS.md (duplicate of DEVELOPMENT.md)
    - [✅] Converted README_ASSETS.txt to English
    - [✅] Removed unnecessary 'nul' file from root
    - [✅] Added MIT License to project
    - [✅] Updated README.md with license info, badges, and screenshot
    - [✅] Created GITHUB_GROWTH_STRATEGY.md in global goals folder
    - [✅] Updated global CLAUDE.md with GitHub growth objectives
    - [✅] Repo'yu public yap (GitHub Settings)
    - [✅] Settings → Pages → Enable (master branch, root folder)
    - [✅] Enhanced README for GitHub Pages:
      - Added logo (256x256 icon) at header
      - Added all 3 screenshots in gallery layout
      - Added quick navigation links (Download, Features, Docs, Privacy)
      - Improved visual hierarchy with centered header
    - [✅] Privacy Policy improvements:
      - Added EN/TR language switcher (top-right, fixed position)
      - Default language: English
      - Saves language preference to localStorage
      - Responsive design (mobile-friendly)
    - [✅] GitHub Pages rendering fix:
      - Created index.html as landing page (professional design)
      - Optimized README.md for GitHub (pure Markdown, no HTML divs)
      - Gradient header with logo and CTA buttons
      - Feature grid with icons and hover effects
      - Screenshot gallery with descriptions
      - Responsive design for mobile
    - [✅] Landing page UX improvements:
      - Auto-sliding screenshot carousel (5 seconds interval)
      - Larger screenshots (max-width: 700px) for better visibility
      - Clickable dots for manual navigation
      - Pause on hover functionality
      - Fixed Privacy Policy button (outline style, visible text)
    - [ ] URL'i test et (https://melihcelenk.github.io/TaskTimerWidget/)
- [✅] Store listing: Short description (EN & TR) (STORE_LISTING.md oluşturuldu)
  - [✅] GitHub username güncelle (melihcelenk)
  - [✅] Privacy Policy URL'i eklendi (https://melihcelenk.github.io/TaskTimerWidget/PRIVACY_POLICY.html)
  - [✅] Support URL güncellendi (https://github.com/melihcelenk/TaskTimerWidget)
- [✅] Package.appxmanifest configuration (Version 1.0.0.0, Publisher: Melih Celenk)
- [✅] MSIX package creation ve signing (~30-60 dakika) 📦

  **✅ Single-Project MSIX Packaging (Modern Approach)**
  - [✅] Removed Windows Application Packaging Project (deprecated approach)
  - [✅] Configured single-project MSIX in TaskTimerWidget.csproj:
    - WindowsPackageType=MSIX
    - EnableMsixTooling=true
    - AppxBundle=Always
    - AppxBundlePlatforms=x64
    - RuntimeIdentifiers=win-x64
  - [✅] Created Properties/launchSettings.json with MsixPackage profile
  - [✅] Updated Package.appxmanifest:
    - EntryPoint=Windows.FullTrustApplication (correct for WinUI 3 desktop)
    - Added rescap:Capability runFullTrust
  - [✅] Build & Package creation:
    - Configuration: Release/x64
    - Project → Package and Publish → Create App Packages
    - Output: bin\x64\Release\net8.0-windows10.0.19041.0\AppPackages\
  - [✅] Local installation tested: App launches and works correctly

- [✅] WACK (Windows App Certification Kit) testi (~15-30 dakika) ✅
  - [✅] WACK GUI tool çalıştırıldı (appcert.exe)
  - [✅] Test Result: **PASSED with WARNINGS** (acceptable for Store submission)
  - [✅] Report saved: wack-test.xml
  - [✅] Warnings reviewed:
    - DLL reflection warnings (normal for .NET 8 apps, not blocking)
    - DPI awareness warning (cosmetic, not blocking)
  - ✅ **Ready for Microsoft Store submission**

### Faz 1.1: Final Release (Tahmini: 2-3 saat aktif + 1-3 gün review) 🚀
- [✅] Version number → 1.0.0.0 (Package.appxmanifest'te ayarlandı)
- [ ] Release build test (~10-15 dakika) 🔨
  - Release mode'da build (dotnet build --configuration Release)
  - Temiz makinede veya VM'de test et
  - Tüm features çalıştığından emin ol
- [ ] Store submission (~30-45 dakika) 📤
  - Microsoft Partner Center'a kayıt
  - Store listing bilgilerini gir (STORE_LISTING.md'den)
  - Screenshots, icons, logos yükle
  - MSIX package yükle
  - Submit for review
- [ ] Wait for Microsoft review (1-3 gün pasif bekleme) ⏳
- [ ] Publish! 🎉

**⏱️ Toplam Kalan Aktif Süre: ~2-3 saat**
**⏱️ Toplam Bekleme: 1-3 gün (Microsoft review)**

---

## 📋 Version 2.0 Features (Deferred)

### Widget Behavior (v1.0 & v2.0)
- [✅] Always-on-top window (v1.0)
- [✅] Minimize button in titlebar (v1.0 - taskbar'a minimize eder)
- [✅] Compact mode toggle (v1.0 - sadece aktif task gösterir, 220x120px)
- [ ] System tray icon (H.NotifyIcon.WinUI kompleks - v2.0'a ertelendi)
- [ ] Minimize to tray (system tray ile birlikte - v2.0)
- [ ] Windows startup (MSIX için karmaşık - v2.0'a ertelendi)

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
