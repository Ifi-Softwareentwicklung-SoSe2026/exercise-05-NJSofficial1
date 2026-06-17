using System.Data;
using System.Security.Principal;
using System.Text.Json;
using Spiellogik;
using Wettlogik;

Turnier wm = new Turnier("Weltmeisterschaft 2026");

if (args.Length == 0)
{
    Console.WriteLine("Keine Parameter genannt - führe beide Befehle nacheinander aus.");
    New();
    string[] testSetArgs = { "set", "1" , "Siegwette", "2.5" };
    Set(testSetArgs);
    string[] testGetArgs = { "get", "1" , "Siegwette" };
    Get(testGetArgs);
    Print();
    string[] testBidArgs1 = { "bid", "Julian Nagelsmann", "1", "Siegwette", "3000000.0" };
    bid(testBidArgs1);
    
    string[] testBidArgs2 = { "bid", "Manuel Neuer", "1", "Siegwette", "1" };
    bid(testBidArgs2);
    
    string[] testResultArgs = { "result", "1", "2:1" };
    result(testResultArgs);
    
    Print();
}
else
{
    string command = args[0].ToLower();

    switch (command)
    {
        case "":
            New();
            Print();
            break;

        case "new": 
            New();
            break;

        case "print": 
            Print();
            break;

        case "set":
            Set(args);
            break;
        case "get":
            Get(args);
            break;
        case "bid":
            bid(args);
            break;
        case "result":
            result(args);
            break;
        default:
            Console.WriteLine($"Fehler: Der Befehl {command} ist nicht bekannt.");
            Console.WriteLine("Die umgesetzten Befehle lauten: 'new', 'print', 'set' und 'get'.");
            Console.WriteLine("Die Syntax bei 'set' erfolgt nach der Syntax set <spielid> <Wetttyp> <Wettquote>.");
            Console.WriteLine("Die Syntax bei 'get' erfolgt nach der Syntax get <spielid> <Wetttyp>.");
            break;
    }
}

void New()
{
    wm.Initialisieren();
    wm.save();
    Console.WriteLine("Initialisierung erfolgreich");
}

void Print()
{
    wm.Load();
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

void Set(string[] args)
{
    // Prüfen auf Syntax ... set <spielid> <Wetttyp> <Wettquote>, also 4 Parameter
    if (args.Length < 4)
    {
        Console.WriteLine("Fehler: Unvollständige Parameter - bitte folgende Syntax beachten \n : dotnet run set <spielid> <Wetttyp> <Wettquote>");
        return;
    }

    int id = int.Parse(args[1]);
    string typ = args[2];
    // CultureInfo, damit bei floats der '.' genutzt werden kann
    double quote = double.Parse(args[3], System.Globalization.CultureInfo.InvariantCulture);


    wm.Load();
    // Auslesen des Spiels aus seiner eindeutigen Spiel-ID
    var spiel = wm.Gruppen.SelectMany(g => g.Spiele).FirstOrDefault(s => s.SpielId == id);
    if (spiel == null)
    {
        Console.WriteLine($"Fehler: Spiel mit ID {id} wurde nicht im Turnier gefunden.");
        return;
    }
    // Laden der bereits vorhandenen Quoten
    QuotenManager.ImportiereQuoten(spiel.Quoten);
    // Setzen der neuen Quote
    QuotenManager.SetQuote(id, typ, quote);
    // Setzen der neuen Quote im Quoten-Manager
    spiel.Quoten = QuotenManager.ExportiereQuoten();
    // Abspeichern des Turniers mitsamt des aktualisierten Spiels
    wm.save();
    Console.WriteLine($"Quote für Spiel-ID {id} des Wetttyps '{typ}' mit Wert {quote} erfolgreich gesetzt und gespeichert!");
}

void Get(string[] args)
{
    if(args.Length < 3)
    {
        Console.WriteLine("Fehler: Unvollständige Parameter. Nutzung: dotnet run get <spielid> <Wetttyp>");
        return;
    }

    int id = int.Parse(args[1]);
    string typ = args[2];

    // Laden der bestehenden Quoten des Turniers
    wm.Load();

    // Heraussuchen des Spiels mit der passenden Spiel-ID in allen Spielen aller Gruppen -> 
    // Flattening der Liste mittels SelectMany
    var spiel = wm.Gruppen.SelectMany(g => g.Spiele).FirstOrDefault(s => s.SpielId == id);

    if (spiel != null)
    {
        // Laden der im jeweiligen Spielobjekt gespeicherten Quoten in den Quoten-Manager
        QuotenManager.ImportiereQuoten(spiel.Quoten);
    }
    else
    {
        Console.WriteLine($"Fehler: Spiel mit ID {id} wurde nicht gefunden.");
        return;
    }

    // Auslesen der Quote mittels Get-Methode des Quoten-Managers
    double quote = QuotenManager.GetQuote(id, typ);
    Console.WriteLine($"Quote für Spiel ID {id} ({typ}): {quote}");
}

void bid(string [] args)
{
    if(args.Length < 5)
    {
        Console.WriteLine("Fehler: Unvollständige Parameter. Nutzung: dotnet run bid <player> <spielid> <Wetttyp> <amount>");
        return;
    }

    string player = args[1];
    int id = int.Parse(args[2]);
    string typ = args[3];
    double amount = double.Parse(args[4], System.Globalization.CultureInfo.InvariantCulture);

    // Laden der bestehenden Quoten des Turniers
    wm.Load();

    // Heraussuchen des Spiels mit der passenden Spiel-ID in allen Spielen aller Gruppen -> 
    // Flattening der Liste mittels SelectMany
    var spiel = wm.Gruppen.SelectMany(g => g.Spiele).FirstOrDefault(s => s.SpielId == id);
    if (spiel == null)
    {
        Console.WriteLine($"Fehler: Spiel mit ID {id} wurde nicht gefunden.");
        return;
    }

    // Ermitteln der Wettquote über den Quotenmanager
    QuotenManager.ImportiereQuoten(spiel.Quoten);
    double quote = QuotenManager.GetQuote(id, typ);

    // Wettmanager-Zuweisung von Wette und Benutzer
    WettManager.ImportiereWetten(wm.Wetten);
    WettManager.ImportiereBenutzer(wm.Benutzer);

    // Setzen der Wette durch den Wettmanager
    WettManager.PlatziereBenutzerwette(player, id, typ, amount, quote);

    wm.Wetten = WettManager.ExportiereWetten();
    wm.Benutzer = WettManager.ExportiereBenutzer();
    wm.save();


}

void result(string[] args)
{
    if (args.Length < 3)
    {
        Console.WriteLine("Fehler: Unvollständige Parameter. Nutzung: dotnet run result <spielid> <ToreHeim>:<ToreAuswärts>");
        return;
    }

    int id = int.Parse(args[1]);
    string ergebnis = args[2];

    wm.Load();

    var spiel = wm.Gruppen.SelectMany(g => g.Spiele).FirstOrDefault(s => s.SpielId == id);
    if (spiel == null)
    {
        Console.WriteLine($"Fehler: Spiel mit ID {id} wurde nicht gefunden.");
        return;
    }

    // Eintragen des Ergebnisses im Spiel
    var tore = ergebnis.Split(':');
    // TODO: Vereinfachen dieser Logik
    if (tore is [var h, var a] && int.TryParse(h, out int tHeim) && int.TryParse(a, out int tAusw))
    {
        spiel.SetErgebnis(tHeim, tAusw);
    }

    // Übergabe der Parameter an den Wettmanager, Auswertung, Rückgabe der Ergebnisse
    WettManager.ImportiereWetten(wm.Wetten);
    WettManager.ImportiereBenutzer(wm.Benutzer);

    WettManager.WerteWetteAus(id, ergebnis);

    wm.Wetten = WettManager.ExportiereWetten();
    wm.Benutzer = WettManager.ExportiereBenutzer();
    wm.save();
}