namespace SinavSistemi.MVC.Models
{
    public class CreateSoruDto
    {
        public int SinavId { get; set; }   // 🔥 OLMAZSA BOŞ GELİR
        public int DersId { get; set; }

        public string SoruMetni { get; set; }
        public string SecenekA { get; set; }
        public string SecenekB { get; set; }
        public string SecenekC { get; set; }
        public string SecenekD { get; set; }
        public string DogruCevap { get; set; }
    }
}
