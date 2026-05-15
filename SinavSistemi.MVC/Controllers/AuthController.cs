using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SinavSistemi.MVC.Services;

namespace SinavSistemi.MVC.Controllers;

public class AuthController : Controller
{
    private readonly ApiService _api;
    public AuthController(ApiService api) { _api = api; }

    public IActionResult Giris() => View();

    [HttpPost]
    public async Task<IActionResult> Giris(string kullaniciAdi, string sifre)
    {
        var (basarili, mesaj) = await _api.Post("auth/giris", new
        {
            KullaniciAdi = kullaniciAdi,
            Sifre = sifre
        });

        if (!basarili)
        {
            ViewBag.Hata = "Kullanıcı adı veya şifre hatalı.";
            return View();
        }

        var kullanici = JsonConvert.DeserializeObject<dynamic>(mesaj)!;

        HttpContext.Session.SetString("Rol", (string)kullanici.rol);
        HttpContext.Session.SetString("KullaniciAdi", (string)kullanici.kullaniciAdi);

        if ((string)kullanici.rol == "Ogrenci")
        {
            HttpContext.Session.SetString("OgrenciId", (string)kullanici.ogrenciId.ToString());
            HttpContext.Session.SetString("OgrenciAdi", (string)kullanici.ogrenciAdi);
            return RedirectToAction("Index", "Ogrenci");
        }

        return RedirectToAction("Index", "Home");
    }

    public IActionResult Cikis()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Giris");
    }
}