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




if (args.Length > 0)
{
    string command = args[0].ToLower();
    RunCommand(command);
}
else
{
    RunInteractiveMode();
}

static void RunInteractiveMode()
{
    bool running = true;
    while (running)
    {
        Console.WriteLine("\nWas möchtest du tun? (new/print/exit)");
        string? input = Console.ReadLine()?.ToLower();

        switch (input)
        {
            case "new": RunCommand("new"); break;
            case "print": RunCommand("print"); break;
            case "exit": running = false; break;
            default: Console.WriteLine("Befehl nicht erkannt."); break;
        }
    }
}

static void RunCommand(string command)
{
    Turnier wm = new Turnier("Weltmeisterschaft 2026");

    switch (command)
    {
        case "new":
        try {
            string json = File.ReadAllText("Startdaten.json");
            
            // keine Unterscheidung von Groß- und Kleinschreibung -> einfacheres Mapping aus der json
            var options = new JsonSerializerOptions { 
                PropertyNameCaseInsensitive = true 
            };

            // Versuch der Deserialisierung
            var data = JsonSerializer.Deserialize<Turnier>(json, options);
            
            if(data != null) {
                data.save();
                Console.WriteLine("Turnier wurde aus den Daten initialisiert und gespeichert.");
            }
        } catch (JsonException ex) {
            Console.WriteLine("Fehler: Das JSON-Format passt nicht zum Konstruktor der Klasse 'Spiel'.");
            Console.WriteLine("Details: " + ex.Message);
        }
        break;

        case "print":
            wm.load();
            Console.WriteLine($"Turnier: {wm.Name}");
            foreach (var gruppe in wm.Gruppen)
            {
                Console.WriteLine($"Gruppe: {gruppe.Name}");
                if (gruppe.Spiele != null) // Prüfen, ob die Liste der Gruppen existiert
                {
                    foreach (var spiel in gruppe.Spiele)
                    {
                        // Sicherer Zugriff: Wenn Heim/Auswärts null ist, wird "Unbekannt" als Platzhalter angezeigt
                        // -> kein Zuweisungsfehler
                        string heim = spiel.HeimMannschaft?.Name ?? "Unbekannt";
                        string auswaerts = spiel.AuswaertsMannschaft?.Name ?? "Unbekannt";
                        
                        Console.WriteLine($"ID {spiel.SpielId}: {heim} vs {auswaerts}");
                    }
                }
            }
            break;
    }
}