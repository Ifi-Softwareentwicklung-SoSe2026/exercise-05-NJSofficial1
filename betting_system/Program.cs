using spiellogik;
using wettlogik

public class ImportJSON
{
    static void importJSONData()
    {
        // Angabe des Dateipfades: hier hart gecodet, in einem echten Projekt aus einem config file eingelesen
        private readonly string jsonDateipfad = ".\Turnierdaten.json";

        // Einlesen des JSON-Inhaltes
        private string jsonInhalt = File.ReadAllText(dateiPfad);

        // Initialisieren des Turniers
        Turnier weltmeisterschaft = JsonSerializer.Deserialize<Turnier>(jsonInhalt);

        if weltmeisterschaft != null
        {
            
        }

    }
    

}