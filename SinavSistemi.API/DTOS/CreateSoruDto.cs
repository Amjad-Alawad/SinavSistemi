public class CreateSoruDto
{
    public int DersId { get; set; }
    public int SinavId { get; set; }   // 🔥 EKLE
    public string SoruMetni { get; set; }
    public string SecenekA { get; set; }
    public string SecenekB { get; set; }
    public string SecenekC { get; set; }
    public string SecenekD { get; set; }
    public string DogruCevap { get; set; }
}