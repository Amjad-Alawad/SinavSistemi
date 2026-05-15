using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SinavSistemi.API.Data;
using SinavSistemi.API.Models;

namespace SinavSistemi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GirisController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GirisController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var user = await _context.Kullanicilar
                .FirstOrDefaultAsync(x =>
                    x.KullaniciAdi == model.KullaniciAdi &&
                    x.Sifre == model.Sifre);

            if (user == null)
                return Unauthorized("Hatalı giriş");

            return Ok(user);
        }
    }
}