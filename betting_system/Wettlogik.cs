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
    private static List<Wettquote> _quoten = new();

    public static void SetQuote(int spielId, string typ, double wert)
    {
        var bestehendeQuote = _quoten.Find(q => q.SpielId == spielId && 
                                                q.Typ.Equals(typ, StringComparison.OrdinalIgnoreCase));

        if(bestehendeQuote != null)
        {
            bestehendeQuote.Quote = wert;
        }
        else
        {
            _quoten.Add(new Wettquote(spielId, typ, wert));
        }
    }

    public static double GetQuote(int spielId, string typ)
    {
       var bestehendeQuote = _quoten.Find(q => q.SpielId == spielId && 
                                                q.Typ.Equals(typ, StringComparison.OrdinalIgnoreCase));
        return bestehendeQuote?.Quote ?? 1.0; // 1.0 als Standardquote hinterlegt, falls keine explizit angegeben wird durch den Nutzer
    }

    public static List<Wettquote> ExportiereQuoten()
    {
        return _quoten;
    }

    public static void ImportiereQuoten(List<Wettquote> geladeneQuoten)
    {
        _quoten = geladeneQuoten ?? new List<Wettquote>();
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

