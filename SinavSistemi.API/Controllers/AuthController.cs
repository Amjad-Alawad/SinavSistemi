using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SinavSistemi.API.Data;

namespace SinavSistemi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    public AuthController(AppDbContext db) { _db = db; }

    [HttpPost("giris")]
    public async Task<IActionResult> Giris([FromBody] LoginDto dto)
    {
        var kullanici = await _db.Kullanicilar
            .Include(k => k.Ogrenci)
            .FirstOrDefaultAsync(k =>
                k.KullaniciAdi == dto.KullaniciAdi &&
                k.Sifre == dto.Sifre);

        if (kullanici == null)
            return Unauthorized(new { mesaj = "Kullanıcı adı veya şifre hatalı." });

        return Ok(new
        {
            kullanici.KullaniciId,
            kullanici.KullaniciAdi,
            kullanici.Rol,
            kullanici.OgrenciId,
            OgrenciAdi = kullanici.Ogrenci != null
                ? kullanici.Ogrenci.Ad + " " + kullanici.Ogrenci.Soyad
                : ""
        });
    }
}

public class LoginDto
{
    public string KullaniciAdi { get; set; } = "";
    public string Sifre { get; set; } = "";
}