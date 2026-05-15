using Microsoft.AspNetCore.Mvc;
using SinavSistemi.MVC.Models;
using SinavSistemi.MVC.Services;

namespace SinavSistemi.MVC.Controllers;

public class OgrenciController : Controller
{
    private readonly ApiService _api;
    public OgrenciController(ApiService api) { _api = api; }

    public async Task<IActionResult> Index()
    {
        var ogrenciId = HttpContext.Session.GetString("OgrenciId");
        if (string.IsNullOrEmpty(ogrenciId))
            return RedirectToAction("Giris", "Auth");

        var sinavlar = await _api.Get<List<OgrenciSinaviVM>>(
            $"sinavlar/ogrencinotlari/{ogrenciId}") ?? new();

        ViewBag.OgrenciAdi = HttpContext.Session.GetString("OgrenciAdi");
        return View(sinavlar);
    }

    public async Task<IActionResult> SinavBasla(int id)
    {
        // id = SinavId
        var ogrenciId = HttpContext.Session.GetString("OgrenciId");
        if (string.IsNullOrEmpty(ogrenciId))
            return RedirectToAction("Giris", "Auth");

        var sorular = await _api.Get<List<SoruVM>>($"sinavlar/{id}/sorular") ?? new();

        if (!sorular.Any())
        {
            TempData["Hata"] = "Bu sınava ait soru bulunamadı. Önce soru eklenmelidir.";
            return RedirectToAction("Index");
        }

        ViewBag.SinavId = id;
        ViewBag.OgrenciId = int.Parse(ogrenciId);
        return View(sorular);
    }

    [HttpPost]
    public async Task<IActionResult> SinavBitir(int sinavId, int ogrenciId, IFormCollection form)
    {
        var cevaplar = new List<object>();

        foreach (var key in form.Keys)
        {
            if (key.StartsWith("soru_"))
            {
                if (int.TryParse(key.Replace("soru_", ""), out int soruId))
                {
                    cevaplar.Add(new
                    {
                        SoruId = soruId,
                        VerilenCevap = form[key].ToString()
                    });
                }
            }
        }

        var (basarili, mesaj) = await _api.Post("sinavlar/cevapla", new
        {
            OgrenciId = ogrenciId,
            SinavId = sinavId,
            Cevaplar = cevaplar
        });

        if (basarili)
        {
            var sonuc = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(mesaj)!;
            TempData["SinavSonuc"] = $"Sınav tamamlandı! Doğru: {sonuc.dogruSayisi}/10 — Notunuz: {sonuc.not}";
        }
        else
        {
            TempData["Hata"] = "Hata: " + mesaj;
        }

        return RedirectToAction("Index");
    }
}