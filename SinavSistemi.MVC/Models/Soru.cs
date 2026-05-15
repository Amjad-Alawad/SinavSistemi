public class Soru
{
    public int SoruId { get; set; }
    public string SoruMetni { get; set; }

    public string SecenekA { get; set; }
    public string SecenekB { get; set; }
    public string SecenekC { get; set; }
    public string SecenekD { get; set; }

    public string DogruCevap { get; set; }

    public int DersId { get; set; }
    public Ders Ders { get; set; }
}

public class Ders
{
    public int DersId { get; set; }
    public string DersAdi { get; set; }
}