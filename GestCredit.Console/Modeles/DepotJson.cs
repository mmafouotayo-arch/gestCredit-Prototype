using System.Text.Json;

namespace GestCredit.Console.Modeles;

public static class DepotJson
{
    private static readonly JsonSerializerOptions Options = new() { WriteIndented = true };

    public static async Task SauvegarderClientsAsync(List<Client> clients, string chemin)
    {
        string json = JsonSerializer.Serialize(clients, Options);
        await File.WriteAllTextAsync(chemin, json);
    }

    public static async Task<List<Client>> ChargerClientsAsync(string chemin)
    {
        if (!File.Exists(chemin))
        {
            return new List<Client>();
        }

        string json = await File.ReadAllTextAsync(chemin);
        return JsonSerializer.Deserialize<List<Client>>(json) ?? new List<Client>();
    }

    public static async Task SauvegarderDemandesAsync(List<DemandeCredit> demandes, string chemin)
    {
        string json = JsonSerializer.Serialize(demandes, Options);
        await File.WriteAllTextAsync(chemin, json);
    }

    public static async Task<List<DemandeCredit>> ChargerDemandesAsync(string chemin)
    {
        if (!File.Exists(chemin))
        {
            return new List<DemandeCredit>();
        }

        string json = await File.ReadAllTextAsync(chemin);
        return JsonSerializer.Deserialize<List<DemandeCredit>>(json) ?? new List<DemandeCredit>();
    }
}