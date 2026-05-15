namespace SinavSistemi.API.Models;

public class Kullanici
{
    public int KullaniciId { get; set; }
    public string KullaniciAdi { get; set; } = "";
    public string Sifre { get; set; } = "";
    public string Rol { get; set; } = "";
    public int? OgrenciId { get; set; }
    public Ogrenci? Ogrenci { get; set; }
}