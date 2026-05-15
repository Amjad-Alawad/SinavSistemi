namespace SinavSistemi.API.Models;

public class Soru
{
    public int SoruId { get; set; }
    public int DersId { get; set; }
    public string SoruMetni { get; set; } = "";
    public string SecenekA { get; set; } = "";
    public string SecenekB { get; set; } = "";
    public string SecenekC { get; set; } = "";
    public string SecenekD { get; set; } = "";
    public string DogruCevap { get; set; } = "";

    public Ders Ders { get; set; } = null!;
    public int? SinavId { get; set; }

    public Sinav Sinav { get; set; }
}
