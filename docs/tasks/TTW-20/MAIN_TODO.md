# [20] Manually Add Time to a Task

## Task Definition

### Mevcut Durum
- Zaman sadece timer start/stop ile takip edilebiliyor
- Timer'i başlatmayı unutan veya PC'den uzakta olan kullanıcılar zaman ekleyemiyor

### İstenen
- Sağ tık menüsünden "Change Time" seçeneği ile hızlı zaman ekleme/çıkarma
- -1h, -15m, +15m, +1h butonları ile kolay ayarlama
- Flyout task kartının üzerinde açılmalı, uygulamanın karanlık temasına uygun olmalı

### Test Senaryoları
- [ ] +15m butonuna basınca 15 dakika eklenmeli
- [ ] -15m butonuna basınca 15 dakika çıkarılmalı (0'ın altına düşmemeli)
- [ ] +1h / -1h doğru çalışmalı
- [ ] Anlık zaman göstergesi her butona basıldığında güncellenmeli
- [ ] Flyout task kartının pozisyonunda açılmalı
- [ ] Flyout renkleri uygulama temasıyla tutarlı olmalı

### Error Cases
- Negatif süreye düşme engellenmeli (Math.Max(0, ...))
- Timer çalışırken de zaman ayarlanabilmeli

## Solution

### Uygulanan Yaklaşım: Quick Duration Buttons (Option 1)
- Sağ tık context menu'ye "Change Time" eklendi
- `Flyout` ile -1h, -15m, +15m, +1h butonları gösteriliyor
- `MainViewModel.SetTaskElapsedTime()` ile zaman güncelleniyor

## Phases

### Phase 1: Change Time Flyout - UI/UX Fix
- [x] Context menu'ye "Change Time" eklendi
- [x] Flyout dark tema renkleriyle uyumlu hale getirildi (#2A2A2A bg, #1A1A1A border)
- [x] Butonlar küçültüldü ve oval yapıldı (CornerRadius=13, Height=26)
- [x] Flyout pozisyonu task kartının altında açılacak şekilde ayarlandı
- [x] "Change Time" başlığı kaldırıldı (gereksiz, yer kaplıyor)
- [ ] Manuel test ile doğrulama
