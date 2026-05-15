using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Dapper;
using SinavSistemi.API.Models;

namespace SinavSistemi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KullanicilarController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public KullanicilarController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // TÜM KULLANICILAR
        [HttpGet]
        public IActionResult GetAll()
        {
            using var connection = new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection"));

            var list = connection.Query<Kullanici>(
                "SELECT * FROM Kullanicilar");

            return Ok(list);
        }

        // LOGIN
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            using var connection = new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection"));

            var user = connection.QueryFirstOrDefault<Kullanici>(
                @"SELECT * FROM Kullanicilar 
                  WHERE KullaniciAdi=@KullaniciAdi 
                  AND Sifre=@Sifre",
                new
                {
                    model.KullaniciAdi,
                    model.Sifre
                });

            if (user == null)
                return Unauthorized("Hatalı kullanıcı adı veya şifre");

            return Ok(user);
        }
    }

    public class LoginModel
    {
        public string KullaniciAdi { get; set; }

        public string Sifre { get; set; }
    }
}