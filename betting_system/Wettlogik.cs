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

public class WettManager()
{
    private List<Wette> _wetten = new();
    private List<Benutzer> _benutzer = new();

    public void PlatziereBenutzerwette(string spielername, int spielId, string wetttyp, double einsatz, double quote)
    {
        // Suchen des Benutzer unter Missachtung von Groß- und Kleinschreibung
        var benutzer = _benutzer.Find(b => b.Name.Equals(spielername, StringComparison.OrdinalIgnoreCase));
        if(benutzer == null)
        {
            // Falls der Nutzer noch nicht registriert worden ist, wird er ins System eingepflegt und erhält 100€ Startguthaben
            benutzer = new Benutzer(spielername, 100.0);
            _benutzer.Add(benutzer);
        }

        // Ausschluss des Kontoüberzugs ;-)
        if (benutzer.Guthaben < einsatz)
        {
            Console.WriteLine($"Fehler: Es ist kein ausreichendes Kontingent vorhanden - {spielerName} hat nur {benutzer.Guthaben} €.");
            return;
        }

        // Abzug der Wette vom Guthaben (mittels Update-Methode) und Registrierung der Wette
        benutzer.UpdateGuthaben(-einsatz);
        _wetten.Add(new Wette (wetttyp, quote, einsatz, benutzer, spielId));
        Console.WriteLine($"Wette platziert: {spielerName} setzt {einsatz} € auf '{wetttyp}' (Quote: {quote}). Restguthaben: {benutzer.Guthaben} €")
    }

    public static void WerteWetteAus(int spielId, string ergebnis)
    {
        // Suchen aller noch nicht mit `result` ausgewerteten Wetten
        var offeneWetten = _wetten.FindAll(w => w.SpielId == spielId && !w.IstAusgewertet);
        if(offeneWetten.Count == 0) return;

        var tore = ergebnis.Split(':');
        // TODO: Vereinfachung
        if (tore.Length != 2 || !int.TryParse(tore[0], out int heimTore) || !int.TryParse(tore[1], out int auswaertsTore))

        foreach(var wette in offeneWetten)
        {
            wette.IstAusgewertet = true;
            var b = _benutzer.Find(u => u.Name.Equals(wette.Benutzer.Name, StringComparison.OrdinalIgnoreCase));
            if(b == null) continue;

            // Siegwette <=> Heimteam gewinnt (als Vereinfachung)
            bool hatGewonnen = wette.Typ.Equals("Siegwette", StringComparison.OrdinalIgnoreCase && heimTore > auswaertsTore);
        }

        

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

