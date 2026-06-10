using System;
using System.Collections.Generic;
using Wettlogik;

namespace Spiellogik;

public class Turnier(string name)
{
    private string _name = name;
    private List<Gruppe> _gruppen = new();
    private Viertelfinale _viertelfinale;
    private Halbfinale _halbfinale;
    private Finale _finale;
    private Spiel _dritterPlatzSpiel;

    public void save() { }
    public void load() { }
}

public class Gruppe(string name)
{
    private string _name = name;
    private List<Mannschaft> _teams = new();
    private List<Spiel> _spiele = new();
}

public class Mannschaft(string name)
{
    private string _name = name;
    public string GetName() => _name;
}

public class Spiel(int spielId, DateTime datum, TimeSpan uhrzeit, Mannschaft heim, Mannschaft auswaerts)
{
    private int _spielId = spielId;
    private DateTime _datum = datum;
    private TimeSpan _uhrzeit = uhrzeit;
    private Mannschaft _heimMannschaft = heim;
    private Mannschaft _auswaertsMannschaft = auswaerts;
    private string _ergebnis;
    private List<Wettquote> _quoten = new();

    public void SetErgebnis(int toreHeim, int toreAuswaerts)
    {
        _ergebnis = $"{toreHeim}:{toreAuswaerts}";
    }
}

public class Viertelfinale(List<Spiel> spiele)
{
    private List<Spiel> _spiele = spiele;
}

public class Halbfinale(List<Spiel> spiele)
{
    private List<Spiel> _spiele = spiele;
}

public class Finale(Spiel finalspiel)
{
    private Spiel _finalspiel = finalspiel;
}

// TODO: HIER GEHT ES WEITER
/*
    + Logik zum Speichern und Laden der Turnierdaten erstellen.
    + Command-Line-Befehle new und print programmieren.
    + Program.cs für automatischen Aufruf von new und print bei dotnet run anpassen.
    + Code in einem eigenen Branch entwickeln und Pull Request nach main erstellen.
*/ 