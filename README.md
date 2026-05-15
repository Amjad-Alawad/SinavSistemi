
### YBS — Web Programlama / Yazılım Geliştirme Projesi
# 🎓 Sınav Sistemi (Exam Management System)

<p align="center">
  <img src="https://img.shields.io/badge/.NET-8.0-blue?style=for-the-badge"/>
  <img src="https://img.shields.io/badge/ASP.NET%20Core-MVC-red?style=for-the-badge"/>
  <img src="https://img.shields.io/badge/Web%20API-RESTful-green?style=for-the-badge"/>
  <img src="https://img.shields.io/badge/Entity%20Framework-Core-purple?style=for-the-badge"/>
  <img src="https://img.shields.io/badge/SQL%20Server-orange?style=for-the-badge"/>
</p>
---

## 📌 Proje Hakkında

**Sınav Sistemi**, eğitim kurumları için geliştirilmiş kapsamlı bir sınav yönetim platformudur. Sistem; öğrenci, hoca, ders, salon, sınav ve soru yönetimini merkezi bir yapı üzerinden dijital olarak yönetmeyi sağlar.

Proje, **ASP.NET Core MVC + ASP.NET Core Web API + Entity Framework Core + SQL Server** kullanılarak geliştirilmiş olup, modern yazılım mimarisi prensiplerine uygun şekilde tasarlanmıştır. MVC ve API yapısı ayrılarak RESTful bir mimari oluşturulmuştur.

---

## 🎯 Projenin Amacı

- Eğitim kurumlarında sınav süreçlerini dijitalleştirmek  
- MVC + Web API mimarisini birlikte kullanmak  
- Gerçek dünya yazılım mimarisi deneyimi kazanmak  
- Katmanlı mimari (Layered Architecture) uygulamak  
- CRUD operasyonlarını profesyonel şekilde geliştirmek  
- Veritabanı ilişkilerini doğru şekilde modellemek (1-N, N-N)  
- DTO ve REST API kullanımını öğrenmek  
- Authentication ve yetkilendirme mantığını uygulamak  

---

## ⚙️ Kullanılan Teknolojiler

- ASP.NET Core MVC  
- ASP.NET Core Web API  
- Entity Framework Core  
- SQL Server  
- Bootstrap 5  
- C#  
- Session Authentication  
- RESTful API  
- DTO Pattern  
- Layered Architecture  

---

## 🧠 Sistem Mimarisi

Kullanıcı (Browser) → MVC (Frontend) → Web API (Backend) → Entity Framework Core → SQL Server

Bu yapı sayesinde:
- Frontend ve backend ayrılmıştır  
- API yeniden kullanılabilir hale getirilmiştir  
- Kod bakımı kolaylaştırılmıştır  
- Ölçeklenebilir bir sistem oluşturulmuştur  

---

## ✨ Sistem Özellikleri

### 🔐 Kimlik Doğrulama
- Session tabanlı giriş sistemi  
- Rol bazlı yetkilendirme (Admin / Hoca / Öğrenci)  
- Yetkisiz erişim kontrolü  

### 👨‍🏫 Hoca Yönetimi
- Hoca ekleme, silme, güncelleme  
- Ders atama  
- Sınav oluşturma ve yönetme  

### 👨‍🎓 Öğrenci Yönetimi
- Öğrenci ekleme ve güncelleme  
- Ders ve sınav görüntüleme  
- Sınavlara katılım  
- Not ve performans takibi  

### 📚 Ders Yönetimi
- Ders oluşturma ve düzenleme  
- Öğrenci ile ilişkilendirme  

### 🏫 Salon Yönetimi
- Salon oluşturma  
- Kapasite yönetimi  

### 📝 Sınav Yönetimi
- Sınav oluşturma  
- Öğrenci atama  
- Soru ekleme  
- Not girme  
- Sonuç görüntüleme  

### ❓ Soru Sistemi
- Çoktan seçmeli soru yapısı  
- Sınav bazlı soru yönetimi  
- Soru bankası  

### 📊 Raporlama
- Öğrenci başarı analizi  
- Sınav istatistikleri  
- Ders bazlı performans raporları  

---

## 🗄️ Veritabanı Yapısı

- Kullanicilar  
- Hocalar  
- Ogrenciler  
- Dersler  
- Salonlar  
- Sinavlar  
- Sorular  
- OgrenciSinavi  
- OgrenciDersi  

İlişkiler:
- Öğrenci ↔ Ders (N-N)  
- Öğrenci ↔ Sınav (N-N)  
- Ders ↔ Sınav (1-N)  
- Sınav ↔ Soru (1-N)  

---

## 📡 API ENDPOINTLERİ

### Authentication
POST /api/auth/login  

### Öğrenciler
GET /api/ogrenciler  
GET /api/ogrenciler/{id}  
POST /api/ogrenciler  
PUT /api/ogrenciler/{id}  
DELETE /api/ogrenciler/{id}  

### Hocalar
GET /api/hocalar  
POST /api/hocalar  
PUT /api/hocalar/{id}  
DELETE /api/hocalar/{id}  

### Dersler
GET /api/dersler  
POST /api/dersler  
PUT /api/dersler/{id}  
DELETE /api/dersler/{id}  

### Salonlar
GET /api/salonlar  
POST /api/salonlar  
PUT /api/salonlar/{id}  
DELETE /api/salonlar/{id}  

### Sınavlar
GET /api/sinavlar  
POST /api/sinavlar  
PUT /api/sinavlar/{id}  
DELETE /api/sinavlar/{id}  

---

## 📁 PROJE YAPISI

SinavSistemi/  
├── SinavSistemi.API/  
│   ├── Controllers  
│   ├── Models  
│   ├── DTOs  
│   ├── Data  
│   ├── Migrations  
│   └── Program.cs  
│  
├── SinavSistemi.MVC/  
│   ├── Controllers  
│   ├── Views  
│   ├── Models  
│   ├── Services  
│   ├── Filters  
│   ├── wwwroot  
│   └── Program.cs  
│  
└── SinavSistemi_VeriTabani.sql  

---

## 🚀 KURULUM

git clone https://github.com/Amjad-Alawad/SinavSistemi.git  
cd SinavSistemi  

cd SinavSistemi.API  
dotnet restore  
dotnet run  

cd SinavSistemi.MVC  
dotnet restore  
dotnet run  

SQL Server’da:  
SinavSistemi_VeriTabani.sql  

---

## 🔐 GÜVENLİK

- Session tabanlı authentication  
- Rol bazlı yetkilendirme  
- Admin / Hoca / Öğrenci ayrımı  
- Yetkisiz erişim engelleme  

---

## 🧩 MİMARİ

- MVC Pattern  
- RESTful API  
- Layered Architecture  
- DTO Pattern  
- Separation of Concerns  

---

## 📊 AVANTAJLAR

- Gerçek dünya yazılım mimarisi  
- Modüler ve genişletilebilir yapı  
- Temiz ve okunabilir kod  
- API + MVC ayrımı  
- Kurumsal seviyeye uygun sistem  

---

## 🖼️ EKRAN GÖRSELLERİ

- Login  
- Dashboard  
- Öğrenci Paneli  
- Hoca Paneli  
- Sınav Yönetimi  
- Raporlama  

---

## 👨‍💻 GELİŞTİRİCİ

**Amjd Alawad**  
YBS — Web Programlama / Yazılım Geliştirme Projesi  
2026  

---

## 📌 NOT

Bu proje eğitim amaçlı geliştirilmiştir ve ASP.NET Core MVC + Web API mimarisini göstermek için hazırlanmıştır.
