using System.Runtime.CompilerServices;
using Spiellogik;

namespace Wettlogik;

public class Wettquote(int spielId, string typ, double quote)
{
    public int SpielId { get; set; } = spielId;
    public string Typ { get; set; } = typ;
    public double Quote { get; set; } = quote;
}

public class Wette(string typ, double quote, double einsatz, Benutzer benutzer, int spielId)
{
    public string Typ { get; set; } = typ;
    public double Quote { get; set; } = quote;
    public double Einsatz { get; set; } = einsatz;
    public bool IstAusgewertet { get; set; }
    public Benutzer Benutzer { get; set; } = benutzer;
    public int SpielId { get; set; } = spielId;
}

public class QuotenManager
{
    private static List<Wettquote> _quote = new();

    public static void SetQuote(int spielId, string typ, double wert)
    {
        // TODO: Finden einer einfacheren Lösung
        var bestehendeQuote = _quoten.FirstOrDefault(q => q.SpielId == spielId && q.Typ.Equals(typ, Comparison.OrdinalIgnoreCase));

        if(bestehendeQuote != null)
        {
            bestehendeQuote.Quote = wert;
        }
        else
        {
            _quoten.Add(new Wettquote(spielId, typ, wert));
        }
    }

    public static void GetQuote(int spielId, string typ)
    {
        // TODO: Finden einer einfacheren Lösung
        var quote = _quoten.FirstOrDefault(q => q.SpielId == spielId && q.Typ.Equals(typ, StringComparison.OrdinalIgnoreCase));
        return quote?.Quote ?? 1.0; // 1.0 als Standardquote hinterlegt, falls keine explizit angegeben wird durch den Nutzer
    }
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

