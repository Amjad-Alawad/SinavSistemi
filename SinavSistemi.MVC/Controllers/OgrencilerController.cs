using Microsoft.AspNetCore.Mvc;
using SinavSistemi.MVC.Models;
using SinavSistemi.MVC.Services;
using static System.Net.WebRequestMethods;

namespace SinavSistemi.MVC.Controllers;

public class OgrencilerController : Controller
{
    private readonly ApiService _api;

    public OgrencilerController(ApiService api)
    {
        _api = api;
    }
    public async Task<IActionResult> Index()
    {
        var liste = await _api.Get<List<OgrenciVM>>("ogrenciler") ?? new();
        return View(liste);
    }

    public IActionResult Ekle() => View(new OgrenciVM());

    [HttpPost]
    public async Task<IActionResult> Ekle(OgrenciVM model)
    {
        var (basarili, mesaj) = await _api.Post("ogrenciler", new { model.OgrenciNo, model.Ad, model.Soyad });
        if (basarili) return RedirectToAction("Index");
        ViewBag.Hata = "Kayıt başarısız: " + mesaj;
        return View(model);
    }

    public async Task<IActionResult> Duzenle(int id)
    {
        var ogrenci = await _api.Get<OgrenciVM>($"ogrenciler/{id}");
        if (ogrenci == null) return RedirectToAction("Index");
        return View(ogrenci);
    }

    [HttpPost]
    public async Task<IActionResult> Duzenle(OgrenciVM model)
    {
        var (basarili, mesaj) = await _api.Put($"ogrenciler/{model.OgrenciId}", new { model.OgrenciNo, model.Ad, model.Soyad });
        if (basarili) return RedirectToAction("Index");
        ViewBag.Hata = "Güncelleme başarısız: " + mesaj;
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Sil(int id)
    {
        await _api.Delete($"ogrenciler/{id}");
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Notlar(int id)
    {
        var ogrenci = await _api.Get<OgrenciVM>($"ogrenciler/{id}");

        var liste = await _api.Get<List<OgrenciSinaviVM>>(
            $"sinavlar/ogrencinotlari/{id}"
        ) ?? new();

        ViewBag.OgrenciAdi = ogrenci != null
            ? $"{ogrenci.Ad} {ogrenci.Soyad}"
            : "Bilinmeyen Öğrenci";

        return View(liste);
    }
    public async Task<IActionResult> Detay(int id)
    {
        var json = await _api.Get<Newtonsoft.Json.Linq.JObject>($"ogrenciler/{id}/detay");
        if (json == null) return RedirectToAction("Index");

        var ogrenci = json["ogrenci"]!.ToObject<OgrenciDetayVM>()!;
        var sinavlar = json["sinavlar"]!.ToObject<List<OgrenciSinaviVM>>()!;

        ogrenci.Sinavlar = sinavlar;

        var notluSinavlar = sinavlar.Where(s => s.Notu.HasValue).ToList();
        ogrenci.Ortalama = notluSinavlar.Any() ? notluSinavlar.Average(s => s.Notu!.Value) : 0;
        ogrenci.GecenSayisi = notluSinavlar.Count(s => s.Notu >= 50);
        ogrenci.KalanSayisi = notluSinavlar.Count(s => s.Notu < 50);

        return View(ogrenci);
    }
    public async Task<IActionResult> PdfIndir(int id)
    {
        var bytes = await _api.GetBytes($"ogrenciler/{id}/pdf");

        if (bytes == null || bytes.Length == 0)
        {
            TempData["Hata"] = "PDF oluşturulamadı.";
            return RedirectToAction("Detay", new { id });
        }

        return File(bytes, "application/pdf", $"not_belgesi_{id}.pdf");
    }

}