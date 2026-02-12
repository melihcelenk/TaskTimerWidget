# [20] Manually Add Time to a Task

## Task Definition

### Mevcut Durum
- Zaman sadece timer start/stop ile takip edilebiliyor
- Timer'i başlatmayı unutan veya PC'den uzakta olan kullanıcılar zaman ekleyemiyor
- About/Help bilgisi yok

### İstenen
- Sağ tık menüsünden "Change Time" seçeneği ile hızlı zaman ekleme/çıkarma
- Inline kart olarak task pozisyonunda açılmalı (Rename mantığıyla aynı)
- Uygulama temasıyla uyumlu olmalı
- About ve Help butonları eklenmeli

### Test Senaryoları
- [x] +5m butonuna basınca 5 dakika eklenmeli
- [x] -5m butonuna basınca 5 dakika çıkarılmalı (0'ın altına düşmemeli)
- [x] +1h / -1h doğru çalışmalı
- [x] Anlık zaman göstergesi her butona basıldığında güncellenmeli
- [x] Change Time inline kartı task pozisyonunda açılmalı
- [x] Task ismi ve süre kart üzerinde görünmeli
- [x] Başka yere tıklayınca Change Time kapanmalı
- [x] Sağ tıklayınca Change Time kapanmalı
- [x] Timer Change Time modunda çalışmaya devam etmeli
- [x] Compact mode'a geçerken Change Time düzgün kapanmalı
- [x] Rename/yeni task eklerken başka yere tıklayınca kaydetmeli (LostFocus)
- [x] About flyout doğru bilgileri göstermeli
- [x] Help flyout ipuçlarını göstermeli

### Error Cases
- [x] Negatif süreye düşme engellenmeli (Math.Max(0, ...))
- [x] Timer çalışırken de zaman ayarlanabilmeli
- [x] Change Time açıkken compact mode'a geçince aktif task doğru görünmeli

## Solution

### Uygulanan Yaklaşım: Inline Card + Quick Duration Buttons
- Sağ tık context menu'ye "Change Time" eklendi
- Flyout yerine **inline kart** kullanıldı (Rename ile aynı pattern)
- Task listeden çıkarılıp yerine edit kartı konuluyor
- `MainViewModel.SetTaskElapsedTime()` ile zaman güncelleniyor
- `PropertyChanged` subscribe ile canlı timer güncellenmesi
- About/Help butonları alt bölüme eklendi

## Phases

### Phase 1: Change Time - Temel İşlevsellik ✅
- [x] Context menu'ye "Change Time" eklendi
- [x] `SetTaskElapsedTime()` metodu ViewModel'e eklendi
- [x] -1h, -5m, +5m, +1h butonları

### Phase 2: Change Time - UI/UX İyileştirmeleri ✅
- [x] Flyout → Inline kart dönüşümü (Rename pattern)
- [x] Task ismi ve süre kartın üzerinde görünüyor
- [x] Font boyutları ve tipleri task kartıyla aynı
- [x] Butonlar küçük ve oval (CornerRadius=12, Height=24)
- [x] Dark tema renkleri (aktif task → Gold bg)
- [x] Başka yere tıklayınca kapanma (MainGrid_Tapped + IsInsideElement)
- [x] Sağ tıklayınca kapanma (ChangeTimeBorder_RightTapped)
- [x] Timer canlı güncellenmesi (PropertyChanged subscribe)
- [x] Compact mode uyumu (CloseChangeTimeCard + delay)

### Phase 3: LostFocus İyileştirmeleri ✅
- [x] Rename'de başka yere tıklayınca kaydetme (NewTaskTextBox_LostFocus)
- [x] Yeni task eklerken başka yere tıklayınca kaydetme

### Phase 4: About & Help ✅
- [x] About butonu (sağ alt, ℹ daire buton)
- [x] Help butonu (sol alt, ? daire buton)
- [x] About flyout: isim, versiyon, tarih, geliştirici, email, website, rate
- [x] Help flyout: tap, right-click, drag, compact mode ipuçları
- [x] Açık yeşil tema (#C8E6C0)
- [x] Flyoutlar + butonundan ortalanarak açılıyor

### Phase 5: Versiyon & Dokümantasyon ✅
- [x] Versiyon 1.0.0.0 → 1.1.0.0
- [x] MAIN_TODO güncellendi
- [x] README güncellendi
