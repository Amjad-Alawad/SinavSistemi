using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SinavSistemi.API.Data;
using SinavSistemi.API.Models;

namespace SinavSistemi.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class SinavlarController : ControllerBase
{
    private readonly AppDbContext _db;
    public SinavlarController(AppDbContext db) { _db = db; }

    // 📌 LIST
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var liste = await _db.Sinavlar
            .Select(s => new
            {
                s.SinavId,
                DersAdi = s.Ders.DersAdi,
                HocaAdi = s.Ders.Hoca.Ad + " " + s.Ders.Hoca.Soyad,
                SalonAdi = s.Salon.SalonAdi,
                s.SinavTarihi,
                s.BaslangicSaati,
                s.BitisSaati,
                OgrenciSayisi = s.OgrenciSinavlari.Count
            })
            .ToListAsync();

        return Ok(liste);
    }

    // 📌 GET BY ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var sinav = await _db.Sinavlar
            .Where(s => s.SinavId == id)
            .Select(s => new
            {
                s.SinavId,
                s.DersId,
                s.SalonId,
                DersAdi = s.Ders.DersAdi,
                HocaAdi = s.Ders.Hoca.Ad + " " + s.Ders.Hoca.Soyad,
                SalonAdi = s.Salon.SalonAdi,
                s.SinavTarihi,
                s.BaslangicSaati,
                s.BitisSaati,
                OgrenciSayisi = s.OgrenciSinavlari.Count
            })
            .FirstOrDefaultAsync();

        if (sinav == null)
            return NotFound();

        return Ok(sinav);
    }

    // 📌 CREATE (FIXED)
    [HttpPost]
    public async Task<IActionResult> Ekle([FromBody] SinavCreateDto dto)
    {
        bool cakisma = await _db.Sinavlar.AnyAsync(s =>
            s.SalonId == dto.SalonId &&
            s.SinavTarihi == dto.SinavTarihi &&
            s.BaslangicSaati < dto.BitisSaati &&
            dto.BaslangicSaati < s.BitisSaati);

        if (cakisma)
            return BadRequest(new { mesaj = "Bu salon bu tarihte dolu." });

        var sinav = new Sinav
        {
            DersId = dto.DersId,
            SalonId = dto.SalonId,
            SinavTarihi = dto.SinavTarihi,
            BaslangicSaati = dto.BaslangicSaati,
            BitisSaati = dto.BitisSaati
        };

        _db.Sinavlar.Add(sinav);
        await _db.SaveChangesAsync();

        return Ok(sinav);
    }

    // 📌 UPDATE (FIXED)
    [HttpPut("{id}")]
    public async Task<IActionResult> Guncelle(int id, [FromBody] SinavUpdateDto dto)
    {
        var sinav = await _db.Sinavlar.FindAsync(id);
        if (sinav == null)
            return NotFound();

        bool cakisma = await _db.Sinavlar.AnyAsync(s =>
            s.SinavId != id &&
            s.SalonId == dto.SalonId &&
            s.SinavTarihi == dto.SinavTarihi &&
            s.BaslangicSaati < dto.BitisSaati &&
            dto.BaslangicSaati < s.BitisSaati);

        if (cakisma)
            return BadRequest(new { mesaj = "Bu salon bu tarihte dolu." });

        sinav.DersId = dto.DersId;
        sinav.SalonId = dto.SalonId;
        sinav.SinavTarihi = dto.SinavTarihi;
        sinav.BaslangicSaati = dto.BaslangicSaati;
        sinav.BitisSaati = dto.BitisSaati;

        await _db.SaveChangesAsync();
        return Ok(sinav);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Sil(int id)
    {
        var sinav = await _db.Sinavlar
            .Include(x => x.OgrenciSinavlari) // 👈 ilişkili kayıt varsa temizler
            .FirstOrDefaultAsync(x => x.SinavId == id);

        if (sinav == null)
            return NotFound();

        // önce ilişkileri sil (çok önemli!)
        _db.OgrenciSinavlari.RemoveRange(sinav.OgrenciSinavlari);

        _db.Sinavlar.Remove(sinav);

        await _db.SaveChangesAsync();

        return Ok();
    }
    [HttpPost("ogrenciekle")]
    public async Task<IActionResult> OgrenciEkle([FromBody] OgrenciSinaviDto dto)
    {
        var zaten = await _db.OgrenciSinavlari
            .AnyAsync(x => x.SinavId == dto.SinavId && x.OgrenciId == dto.OgrenciId);

        if (zaten)
            return BadRequest(new { mesaj = "Öğrenci zaten ekli." });

        var kayit = new OgrenciSinavi
        {
            SinavId = dto.SinavId,
            OgrenciId = dto.OgrenciId,
            Notu = dto.Notu
        };

        _db.OgrenciSinavlari.Add(kayit);
        await _db.SaveChangesAsync();

        return Ok();
    }

    // API
    [HttpPut("notgir")]
    public async Task<IActionResult> NotGir([FromBody] OgrenciSinaviDto dto)
    {
        var kayit = await _db.OgrenciSinavlari
            .FirstOrDefaultAsync(x => x.OgrenciId == dto.OgrenciId && x.SinavId == dto.SinavId);

        if (kayit == null)
            return NotFound(new { mesaj = "Kayıt bulunamadı" });

        kayit.Notu = dto.Notu;

        await _db.SaveChangesAsync();

        return Ok(kayit);
    }
    [HttpGet("detay/{sinavId}")]
    public async Task<IActionResult> Detay(int sinavId)
    {
        var liste = await _db.OgrenciSinavlari
            .Include(x => x.Ogrenci)
            .Include(x => x.Sinav)
                .ThenInclude(s => s.Ders)
                    .ThenInclude(d => d.Hoca)
            .Include(x => x.Sinav.Salon)
            .Where(x => x.SinavId == sinavId)
         .Select(x => new OgrenciSinaviDetayDto
         {
             Id = x.Id,
             OgrenciId = x.OgrenciId,
             OgrenciNo = x.Ogrenci.OgrenciNo,
             OgrenciAdi = x.Ogrenci.Ad + " " + x.Ogrenci.Soyad,
             DersAdi = x.Sinav.Ders.DersAdi,
             HocaAdi = x.Sinav.Ders.Hoca.Ad + " " + x.Sinav.Ders.Hoca.Soyad,
             SalonAdi = x.Sinav.Salon.SalonAdi,
             SinavTarihi = x.Sinav.SinavTarihi,
             BaslangicSaati = x.Sinav.BaslangicSaati,
             BitisSaati = x.Sinav.BitisSaati,
             Notu = x.Notu
         })
            .ToListAsync();

        return Ok(liste);
    }
    [HttpGet("ogrencinotlari/{ogrenciId}")]
    public async Task<IActionResult> OgrenciNotlari(int ogrenciId)
    {
        var liste = await _db.OgrenciSinavlari
            .Where(x => x.OgrenciId == ogrenciId)
            .Select(x => new {
                x.Id,
                x.SinavId,        // ← EKLENDİ
                x.OgrenciId,
                OgrenciNo = x.Ogrenci.OgrenciNo,
                OgrenciAdi = x.Ogrenci.Ad + " " + x.Ogrenci.Soyad,
                DersAdi = x.Sinav.Ders.DersAdi,
                HocaAdi = x.Sinav.Ders.Hoca.Ad + " " + x.Sinav.Ders.Hoca.Soyad,
                SalonAdi = x.Sinav.Salon.SalonAdi,
                x.Sinav.SinavTarihi,
                x.Sinav.BaslangicSaati,
                x.Sinav.BitisSaati,
                x.Notu
            }).ToListAsync();
        return Ok(liste);
    }
    [HttpGet("sinav/{sinavId}")]
    public IActionResult SinavaGoreSorular(int sinavId)
    {
        var sorular = _db.Sorular.ToList();

        return Ok(sorular);
    }
    // GET: api/sinavlar/{id}/sorular
    [HttpGet("{id}/sorular")]
    public async Task<IActionResult> SinavSorulari(int id)
    {
        var sinav = await _db.Sinavlar
            .Include(s => s.Ders)
            .FirstOrDefaultAsync(s => s.SinavId == id);

        if (sinav == null) return NotFound();

        var sorular = await _db.Sorular
            .Where(s => s.DersId == sinav.DersId)
            .Select(s => new {
                s.SoruId,
                s.SoruMetni,
                s.SecenekA,
                s.SecenekB,
                s.SecenekC,
                s.SecenekD
                // DogruCevap'ı öğrenciye göndermiyoruz!
            }).ToListAsync();

        return Ok(sorular);
    }

    // POST: api/sinavlar/cevapla
    [HttpPost("cevapla")]
    public async Task<IActionResult> Cevapla([FromBody] CevapDto dto)
    {
        // Öğrencinin bu sınavdaki kaydını bul
        var kayit = await _db.OgrenciSinavlari
            .FirstOrDefaultAsync(x =>
                x.OgrenciId == dto.OgrenciId &&
                x.SinavId == dto.SinavId);

        if (kayit == null)
            return NotFound(new { mesaj = "Öğrenci bu sınava kayıtlı değil." });

        if (kayit.Notu.HasValue)
            return BadRequest(new { mesaj = "Bu sınav zaten tamamlandı." });

        // Doğru cevapları getir
        var sinav = await _db.Sinavlar.FindAsync(dto.SinavId);
        var sorular = await _db.Sorular
            .Where(s => s.DersId == sinav!.DersId)
            .ToListAsync();

        // Doğru sayısını hesapla
        int dogruSayisi = 0;
        foreach (var cevap in dto.Cevaplar)
        {
            var soru = sorular.FirstOrDefault(s => s.SoruId == cevap.SoruId);
            if (soru != null && soru.DogruCevap == cevap.VerilenCevap)
                dogruSayisi++;
        }

        // Not = doğru sayısı × 5 (10 soru × 5 = 100)
        kayit.Notu = dogruSayisi * 5;
        await _db.SaveChangesAsync();

        return Ok(new
        {
            dogruSayisi,
            toplamSoru = sorular.Count,
            not = kayit.Notu
        });
    }
    [HttpGet("{id}/rapor")]
    public async Task<IActionResult> SinavRaporu(int id)
    {
        var sinav = await _db.Sinavlar
            .Include(s => s.Ders).ThenInclude(d => d.Hoca)
            .Include(s => s.Salon)
            .FirstOrDefaultAsync(s => s.SinavId == id);

        if (sinav == null) return NotFound();

        var kayitlar = await _db.OgrenciSinavlari
            .Where(x => x.SinavId == id)
            .Select(x => new {
                OgrenciAdi = x.Ogrenci.Ad + " " + x.Ogrenci.Soyad,
                x.Ogrenci.OgrenciNo,
                x.Notu
            }).ToListAsync();

        var notluKayitlar = kayitlar.Where(k => k.Notu.HasValue).ToList();

        return Ok(new
        {
            SinavId = sinav.SinavId,
            DersAdi = sinav.Ders.DersAdi,
            HocaAdi = sinav.Ders.Hoca.Ad + " " + sinav.Ders.Hoca.Soyad,
            SalonAdi = sinav.Salon.SalonAdi,
            sinav.SinavTarihi,
            ToplamOgrenci = kayitlar.Count,
            NotGirilenSayisi = notluKayitlar.Count,
            Ortalama = notluKayitlar.Any() ? notluKayitlar.Average(k => k.Notu!.Value) : 0,
            EnYuksek = notluKayitlar.Any() ? notluKayitlar.Max(k => k.Notu!.Value) : 0,
            EnDusuk = notluKayitlar.Any() ? notluKayitlar.Min(k => k.Notu!.Value) : 0,
            GecenSayisi = notluKayitlar.Count(k => k.Notu >= 50),
            KalanSayisi = notluKayitlar.Count(k => k.Notu < 50),
            Ogrenciler = kayitlar
        });
    }
    public class CevapDto
    {
        public int OgrenciId { get; set; }
        public int SinavId { get; set; }
        public List<CevapItem> Cevaplar { get; set; } = new();
    }

    public class CevapItem
    {
        public int SoruId { get; set; }
        public string VerilenCevap { get; set; } = "";
    }


}