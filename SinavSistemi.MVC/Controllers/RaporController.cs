using Microsoft.AspNetCore.Mvc;
using SinavSistemi.MVC.Models;
using SinavSistemi.MVC.Services;

namespace SinavSistemi.MVC.Controllers;

public class RaporController : Controller
{
    private readonly ApiService _api;
    public RaporController(ApiService api) { _api = api; }

    public async Task<IActionResult> Index()
    {
        var hocalar = await _api.Get<List<HocaVM>>("hocalar") ?? new();
        return View(hocalar);
    }

    public async Task<IActionResult> HocaRaporu(int hocaId)
    {
        var rapor = await _api.Get<List<dynamic>>($"rapor/hocasinavlari/{hocaId}") ?? new();
        var hoca = await _api.Get<HocaVM>($"hocalar/{hocaId}");
        ViewBag.HocaAdi = $"{hoca?.Ad} {hoca?.Soyad}";
        return View(rapor);
    }
}