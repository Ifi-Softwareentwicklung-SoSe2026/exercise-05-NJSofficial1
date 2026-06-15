using System.Data;
using System.Security.Principal;
using System.Text.Json;
using Spiellogik;
using Wettlogik;





using Wettlogik;

Turnier wm = new Turnier("Weltmeisterschaft 2026");

if (args.Length == 0)
{
    Console.WriteLine("Keine Parameter genannt - führe beide Befehle nacheinander aus.");
    
    wm.Initialisieren();
    
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
else
{
    string command = args[0].ToLower();

    if (command == "new")
    {
        wm.Initialisieren();
        wm.save();
        Console.WriteLine("Initialisierung erfolgreich");
    }
    else if (command == "print")
    {
        wm.Initialisieren();
        
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
    else
    {
        Console.WriteLine($"Fehler: Der Befehl '{command}' ist unbekannt.");
        Console.WriteLine("Erlaubte Befehle sind 'new' oder 'print'.");
    }
}