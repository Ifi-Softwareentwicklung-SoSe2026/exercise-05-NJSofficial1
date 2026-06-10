using Spiellogik;

namespace Wettlogik;

public class Wette(string typ, double quote, double einsatz, Benutzer benutzer, Spiel spiel)
{
    private string _typ = typ;
    private double _quote = quote;
    private double _einsatz = einsatz;
    private bool _istAusgewertet;
    private Benutzer _benutzer = benutzer;
    private Spiel _spiel = spiel;

    public double Auswerten(string ergebnis) => 0.0;
}

public class Wettquote(string typ, double quote)
{
    private string _typ = typ;
    private double _quote = quote;
}

public class Benutzer(string name, double guthaben)
{
    private string _name = name;
    private double _guthaben = guthaben;

    public void UpdateGuthaben(double amount)
    {
        _guthaben += amount;
    }
}