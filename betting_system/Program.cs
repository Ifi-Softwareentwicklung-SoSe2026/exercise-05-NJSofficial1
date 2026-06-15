using System.Security.Principal;
using System.Text.Json;
using Spiellogik;
using Wettlogik;

/* Testdatensatz
// Testdaten
Turnier weltmeisterschaft = new Turnier("Weltmeisterschaft 2026");
Mannschaft deutschland = new Mannschaft("deutschland");
Mannschaft brasilien = new Mannschaft("brasilien");

Gruppe gruppeA = new Gruppe("Gruppe A");
gruppeA.Teams.Add(deutschland);
gruppeA.Teams.Add(brasilien);

Spiel eroffnungsspiel = new Spiel(1, DateTime.Now, TimeSpan.Parse("20:00"), deutschland, brasilien);
gruppeA.Spiele.Add(eroffnungsspiel);

weltmeisterschaft.Gruppen.Add(gruppeA);

weltmeisterschaft.save();
*/




if (args.Length == 0)
{
    Console.WriteLine("Fehler: Bitte gib einen Befehl an ('new' oder 'print').");
    return; // Programm beenden
}

// Den Befehl aus dem ersten Argument lesen
string command = args[0].ToLower();

if (command == "new")
{
    string json = File.ReadAllText("Startdaten.json");
    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    var data = JsonSerializer.Deserialize<Turnier>(json, options);
    
    if (data != null) {
        data.save();
        Console.WriteLine("Turnier wurde erfolgreich initialisiert.");
    }
}
else if (command == "print")
{
    Turnier wm = new Turnier("Weltmeisterschaft 2026");
    wm.load();
    
    Console.WriteLine($"Turnier: {wm.Name}");
    foreach (var gruppe in wm.Gruppen)
    {
        Console.WriteLine($"Gruppe: {gruppe.Name}");
        foreach (var spiel in gruppe.Spiele)
        {
            string heim = spiel.HeimMannschaft?.Name ?? "Unbekannt";
            string ausw = spiel.AuswaertsMannschaft?.Name ?? "Unbekannt";
            Console.WriteLine($"ID {spiel.SpielId}: {heim} vs {ausw}");
        }
    }
}