using Microsoft.AspNetCore.Mvc;
using SinavSistemi.MVC.Models;
using SinavSistemi.MVC.Services;

public class SoruController : Controller
{
    private readonly ApiService _api;

    public SoruController(ApiService api)
    {
        _api = api;
    }

    // ================= INDEX (GENEL SORULAR) =================
    public async Task<IActionResult> Index()
    {
        var sorular = await _api.Get<List<Soru>>("https://localhost:7012/api/Soru")
                       ?? new List<Soru>();

        var grouped = sorular
            .GroupBy(x => x.Ders?.DersAdi ?? "Bilinmeyen Ders")
            .ToList();

        return View(grouped);
    }

    // ================= SINAVA GÖRE SORULAR =================
    public async Task<IActionResult> Sorular(int sinavId)
    {
        var sorular = await _api.Get<List<Soru>>(
            $"https://localhost:7012/api/Soru/sinav/{sinavId}"
        ) ?? new List<Soru>();

        ViewBag.SinavId = sinavId;

        return View(sorular);
    }

    // ================= EKLE (GET) =================
    [HttpGet]
    public async Task<IActionResult> Ekle()
    {
        ViewBag.Dersler = await _api.Get<List<DersVM>>(
            "https://localhost:7012/api/Dersler"
        ) ?? new List<DersVM>();

        ViewBag.Sinavlar = await _api.Get<List<SinavVM>>(
            "https://localhost:7012/api/Sinavlar"
        ) ?? new List<SinavVM>();

        return View(new CreateSoruDto());
    }

    // ================= EKLE (POST) =================
    [HttpPost]
    public async Task<IActionResult> Ekle(CreateSoruDto dto)
    {
        var result = await _api.Post(
            "https://localhost:7012/api/Soru",
            dto
        );

        if (result.Basarili)
            return RedirectToAction("Index");

        ViewBag.Error = result.Mesaj;

        ViewBag.Dersler = await _api.Get<List<DersVM>>(
            "https://localhost:7012/api/Dersler"
        ) ?? new List<DersVM>();

        ViewBag.Sinavlar = await _api.Get<List<SinavVM>>(
            "https://localhost:7012/api/Sinavlar"
        ) ?? new List<SinavVM>();

        return View(dto);
    }
    public async Task<IActionResult> Detay(string dersAdi)
    {
        var sorular = await _api.Get<List<Soru>>(
            "https://localhost:7012/api/Soru"
        ) ?? new List<Soru>();

        var filtre = sorular
            .Where(x => x.Ders?.DersAdi == dersAdi)
            .ToList();

        ViewBag.DersAdi = dersAdi;

        return View(filtre);
    }
    [HttpPost]
    [HttpPost]
    public async Task<IActionResult> SinavBitir(IFormCollection form)
    {
        var sinavId = Convert.ToInt32(form["sinavId"]);

        var sorular = await _api.Get<List<Soru>>(
            $"https://localhost:7012/api/Soru/sinav/{sinavId}"
        ) ?? new List<Soru>();

        int dogru = 0;

        foreach (var s in sorular)
        {
            var cevap = form[$"cevap_{s.SoruId}"].ToString();

            if (!string.IsNullOrEmpty(cevap) &&
                cevap.Trim().ToUpper() == s.DogruCevap.Trim().ToUpper())
            {
                dogru++;
            }
        }

        int toplam = sorular.Count;
        int yanlis = toplam - dogru;
        int puan = toplam == 0 ? 0 : (dogru * 100 / toplam);

        ViewBag.Dogru = dogru;
        ViewBag.Yanlis = yanlis;
        ViewBag.Puan = puan;

        return View("Sonuc"); // 👈 kritik
    }

}