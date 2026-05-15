using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SinavSistemi.API.Data;
using SinavSistemi.API.Models;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Colors;

namespace SinavSistemi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OgrencilerController : ControllerBase
{
    private readonly AppDbContext _db;
    public OgrencilerController(AppDbContext db) { _db = db; }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var liste = await _db.Ogrenciler
            .Select(x => new OgrenciDto
            {
                OgrenciId = x.OgrenciId,
                OgrenciNo = x.OgrenciNo,
                Ad = x.Ad,
                Soyad = x.Soyad
            })
            .ToListAsync();

        return Ok(liste);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var ogrenci = await _db.Ogrenciler.FindAsync(id);
        if (ogrenci == null) return NotFound();
        return Ok(ogrenci);
    }

    [HttpPost]
    public async Task<IActionResult> Ekle([FromBody] Ogrenci ogrenci)
    {
        bool varMi = await _db.Ogrenciler.AnyAsync(o => o.OgrenciNo == ogrenci.OgrenciNo);
        if (varMi) return BadRequest(new { mesaj = "Bu öğrenci numarası zaten kayıtlı." });
        _db.Ogrenciler.Add(ogrenci);
        await _db.SaveChangesAsync();
        return Ok(ogrenci);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Guncelle(int id, [FromBody] Ogrenci gelen)
    {
        var ogrenci = await _db.Ogrenciler.FindAsync(id);
        if (ogrenci == null) return NotFound();
        ogrenci.Ad = gelen.Ad;
        ogrenci.Soyad = gelen.Soyad;
        ogrenci.OgrenciNo = gelen.OgrenciNo;
        await _db.SaveChangesAsync();
        return Ok(ogrenci);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Sil(int id)
    {
        var ogrenci = await _db.Ogrenciler.FindAsync(id);
        if (ogrenci == null) return NotFound();
        _db.Ogrenciler.Remove(ogrenci);
        await _db.SaveChangesAsync();
        return Ok();
    }
    [HttpGet("{id}/detay")]
    public async Task<IActionResult> Detay(int id)
    {
        var ogrenci = await _db.Ogrenciler
            .Select(o => new {
                o.OgrenciId,
                o.OgrenciNo,
                o.Ad,
                o.Soyad,
                DersSayisi = o.OgrenciDersleri.Count,
                SinavSayisi = o.OgrenciSinavlari.Count
            })
            .FirstOrDefaultAsync(o => o.OgrenciId == id);

        if (ogrenci == null) return NotFound();

        var sinavlar = await _db.OgrenciSinavlari
            .Where(x => x.OgrenciId == id)
            .Select(x => new {
                x.Id,
                x.SinavId,
                DersAdi = x.Sinav.Ders.DersAdi,
                HocaAdi = x.Sinav.Ders.Hoca.Ad + " " + x.Sinav.Ders.Hoca.Soyad,
                SalonAdi = x.Sinav.Salon.SalonAdi,
                x.Sinav.SinavTarihi,
                x.Sinav.BaslangicSaati,
                x.Notu
            })
            .OrderBy(x => x.SinavTarihi)
            .ToListAsync();

        return Ok(new { ogrenci, sinavlar });
    }
    [HttpGet("{id}/pdf")]
    public async Task<IActionResult> PdfIndir(int id)
    {
        try
        {
            var ogrenci = await _db.Ogrenciler.FindAsync(id);
            if (ogrenci == null) return NotFound("Öğrenci bulunamadı.");

            var sinavlar = await _db.OgrenciSinavlari
                .Where(x => x.OgrenciId == id && x.Notu.HasValue)
                .Select(x => new {
                    DersAdi = x.Sinav.Ders.DersAdi,
                    HocaAdi = x.Sinav.Ders.Hoca.Ad + " " + x.Sinav.Ders.Hoca.Soyad,
                    x.Sinav.SinavTarihi,
                    x.Notu
                })
                .ToListAsync();

            decimal ortalama = sinavlar.Any() ? sinavlar.Average(s => s.Notu!.Value) : 0;

            var ms = new MemoryStream();
            var writer = new PdfWriter(ms);
            writer.SetCloseStream(false);
            var pdf = new PdfDocument(writer);
            var doc = new Document(pdf);

            var kalin = iText.Kernel.Font.PdfFontFactory.CreateFont(
                iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD);
            var normal = iText.Kernel.Font.PdfFontFactory.CreateFont(
                iText.IO.Font.Constants.StandardFonts.HELVETICA);

            doc.Add(new Paragraph("SINAV SISTEMI")
                .SetFont(kalin).SetFontSize(20)
                .SetTextAlignment(TextAlignment.CENTER));

            doc.Add(new Paragraph("OGRENCI NOT BELGESI")
                .SetFont(kalin).SetFontSize(14)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetMarginBottom(20));

            doc.Add(new Paragraph($"Ad Soyad   : {ogrenci.Ad} {ogrenci.Soyad}")
                .SetFont(normal).SetFontSize(12));
            doc.Add(new Paragraph($"Ogrenci No : {ogrenci.OgrenciNo}")
                .SetFont(normal).SetFontSize(12));
            doc.Add(new Paragraph($"Tarih      : {DateTime.Now:dd.MM.yyyy}")
                .SetFont(normal).SetFontSize(12).SetMarginBottom(15));

            var tablo = new Table(new float[] { 3, 3, 2, 2 }).UseAllAvailableWidth();

            foreach (var baslik in new[] { "Ders", "Hoca", "Tarih", "Not" })
            {
                tablo.AddHeaderCell(
                    new Cell().Add(new Paragraph(baslik).SetFont(kalin))
                        .SetBackgroundColor(ColorConstants.LIGHT_GRAY));
            }

            if (sinavlar.Any())
            {
                foreach (var s in sinavlar)
                {
                    tablo.AddCell(new Cell().Add(new Paragraph(s.DersAdi ?? "-").SetFont(normal)));
                    tablo.AddCell(new Cell().Add(new Paragraph(s.HocaAdi ?? "-").SetFont(normal)));
                    tablo.AddCell(new Cell().Add(new Paragraph(s.SinavTarihi.ToString()).SetFont(normal)));
                    tablo.AddCell(new Cell().Add(new Paragraph(s.Notu!.Value.ToString("F1")).SetFont(normal)));
                }
            }
            else
            {
                tablo.AddCell(new Cell(1, 4).Add(
                    new Paragraph("Henuz not girilmemis sinav yok.").SetFont(normal)));
            }

            doc.Add(tablo);

            doc.Add(new Paragraph($"\nGenel Ortalama: {ortalama:F1}")
                .SetFont(kalin).SetFontSize(13).SetMarginTop(15));

            doc.Add(new Paragraph(ortalama >= 50 ? "DURUM: BASARILI" : "DURUM: BASARISIZ")
                .SetFont(kalin).SetFontSize(12));

            doc.Close();

            var bytes = ms.ToArray();
            ms.Dispose();

            return File(bytes, "application/pdf",
                $"not_belgesi_{ogrenci.OgrenciNo}.pdf");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Hata: {ex.Message} | {ex.InnerException?.Message}");
        }
    }


}