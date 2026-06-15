using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Wettlogik;


namespace Spiellogik;

public class Turnier(string name)
{
    public string Name {get; set;} = name;
    public List<Gruppe> Gruppen {get; set;} = new();
    public Viertelfinale Viertelfinale {get; set;}
    public Halbfinale Halbfinale {get; set;}
    public Finale Finale {get; set;}
    public Spiel DritterPlatzSpiel {get; set;}
    private string dateipfad = @".\Turnierdaten.json"; 

    public void save()
    {
        
        var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(dateipfad, json);
    }
     
    public void Initialisieren()
    {
        this.Gruppen.Clear();
        
        Mannschaft mexiko = new Mannschaft("Mexiko");
        Mannschaft suedafrika = new Mannschaft("Südafrika");

        Gruppe gruppeA = new Gruppe("Gruppe A");
        gruppeA.Teams.Add(mexiko);
        gruppeA.Teams.Add(suedafrika);

        Spiel eroffnungsspiel = new Spiel(1, DateTime.Now, TimeSpan.Parse("20:00"), mexiko, suedafrika);
        gruppeA.Spiele.Add(eroffnungsspiel);

        this.Gruppen.Add(gruppeA);
    }
    
}

public class Gruppe(string name)
{
    public string Name {get; set;} = name;
    public List<Mannschaft> Teams {get; set;} = new();
    public List<Spiel> Spiele {get; set;} = new();
}

public class Mannschaft
{
    public string Name { get; set; } = string.Empty;

    // Parameterloser Konstruktor für den Serializer
    public Mannschaft() { }

    // Konstruktor für die manuelle Erstellung mittels "new" durch den Nutzer
    public Mannschaft(string name)
    {
        Name = name;
    }

    public string GetName() => Name;
}

public class Spiel
{
    public int SpielId { get; set; }
    public DateTime Datum { get; set; }
    public TimeSpan Uhrzeit { get; set; }
    public Mannschaft HeimMannschaft { get; set; }
    public Mannschaft AuswaertsMannschaft { get; set; }
    public string Ergebnis { get; set; }
    public List<Wettquote> Quoten { get; set; } = new();

    // Parameterloser Konstruktor für den Serializer
    public Spiel() { }

    // Konstruktor für die manuelle Erstellung mittels new aus den Startdaten heraus
    public Spiel(int spielId, DateTime datum, TimeSpan uhrzeit, Mannschaft heim, Mannschaft auswaerts)
    {
        SpielId = spielId;
        Datum = datum;
        Uhrzeit = uhrzeit;
        HeimMannschaft = heim;
        AuswaertsMannschaft = auswaerts;
    }

    public void SetErgebnis(int toreHeim, int toreAuswaerts)
    {
        Ergebnis = $"{toreHeim}:{toreAuswaerts}";
    }
}

public class Viertelfinale(List<Spiel> spiele)
{
    public List<Spiel> Spiele { get; set; } = spiele;
}

public class Halbfinale(List<Spiel> spiele)
{
    public List<Spiel> Spiele { get; set; } = spiele;
}

public class Finale(Spiel finalspiel)
{
    public Spiel Finalspiel { get; set; } = finalspiel;
}
