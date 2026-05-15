using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SinavSistemi.API.Data;
using SinavSistemi.API.Models;

namespace SinavSistemi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SoruController : ControllerBase
    {
        private readonly AppDbContext _db;

        public SoruController(AppDbContext db)
        {
            _db = db;
        }

        // 🔹 TÜM SORULAR
        [HttpGet]
        public IActionResult GetAll()
        {
            var sorular = _db.Sorular
                .Include(x => x.Ders)
                .ToList();

            return Ok(sorular);
        }

        // 🔹 TEK SORU
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var soru = _db.Sorular.Find(id);

            if (soru == null)
                return NotFound("Soru bulunamadı");

            return Ok(soru);
        }

        // 🔹 EKLE
        [HttpPost]
        public IActionResult Ekle(CreateSoruDto dto)
        {
            var ders = _db.Dersler.Find(dto.DersId);
            if (ders == null)
                return BadRequest("Geçersiz DersId");

            var soru = new Soru
            {
                DersId = dto.DersId,
                SoruMetni = dto.SoruMetni,
                SecenekA = dto.SecenekA,
                SecenekB = dto.SecenekB,
                SecenekC = dto.SecenekC,
                SecenekD = dto.SecenekD,
                DogruCevap = dto.DogruCevap
            };

            _db.Sorular.Add(soru);
            _db.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = soru.SoruId }, soru);
        }

        // 🔹 GÜNCELLE
        [HttpPut("{id}")]
        public IActionResult Guncelle(int id, CreateSoruDto dto)
        {
            var soru = _db.Sorular.Find(id);

            if (soru == null)
                return NotFound("Soru bulunamadı");

            soru.DersId = dto.DersId;
            soru.SoruMetni = dto.SoruMetni;
            soru.SecenekA = dto.SecenekA;
            soru.SecenekB = dto.SecenekB;
            soru.SecenekC = dto.SecenekC;
            soru.SecenekD = dto.SecenekD;
            soru.DogruCevap = dto.DogruCevap;

            _db.SaveChanges();

            return Ok(soru);
        }

        // 🔹 SİL
        [HttpDelete("{id}")]
        public IActionResult Sil(int id)
        {
            var soru = _db.Sorular.Find(id);

            if (soru == null)
                return NotFound("Soru bulunamadı");

            _db.Sorular.Remove(soru);
            _db.SaveChanges();

            return Ok("Silindi");
        }
    }
}