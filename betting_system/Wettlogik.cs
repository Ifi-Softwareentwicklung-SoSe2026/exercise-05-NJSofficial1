using Spiellogik;

namespace Wettlogik;

public class Wette(string typ, double quote, double einsatz, Benutzer benutzer, int spielId)
{
    public string Typ { get; set; } = typ;
    public double Quote { get; set; } = quote;
    public double Einsatz { get; set; } = einsatz;
    public bool IstAusgewertet { get; set; }
    public Benutzer Benutzer { get; set; } = benutzer;
    public int SpielId { get; set; } = spielId;
}

public class Wettquote(int spielId, string typ, double quote)
{
    public int SpielId { get; set; } = spielId;
    public string Typ { get; set; } = typ;
    public double Quote { get; set; } = quote;
}

public class Benutzer(string name, double guthaben)
{
    public string Name { get; set; } = name;
    public double Guthaben { get; set; } = guthaben;

    public void UpdateGuthaben(double betrag)
    {
        Guthaben += betrag;
    }
}