using Microsoft.EntityFrameworkCore;
using GestCredit.Api.Data;
using GestCredit.Api.Models;
using GestCredit.Api.DTOs;

namespace GestCredit.Api.Services;

public class ClientService : IClientService
{
    private readonly GestCreditDbContext _context;

    public ClientService(GestCreditDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ClientDto>> GetAllAsync()
    {
        return await _context.Clients
            .Select(c => new ClientDto(c.Id, c.Nom, c.Ville))
            .ToListAsync();
    }

    public async Task<ClientDto?> GetByIdAsync(int id)
    {
        var client = await _context.Clients.FindAsync(id);
        return client is null ? null : new ClientDto(client.Id, client.Nom, client.Ville);
    }

    public async Task<ClientDto> CreateAsync(CreateClientDto dto)
    {
        // Laisse l'ArgumentException remonter : le middleware d'exception globale la traduira en 400
        var client = new Client(dto.Nom, dto.Ville);

        _context.Clients.Add(client);
        await _context.SaveChangesAsync();

        return new ClientDto(client.Id, client.Nom, client.Ville);
    }

    public async Task<bool> UpdateAsync(int id, UpdateClientDto dto)
    {
        var client = await _context.Clients.FindAsync(id);

        if (client is null)
        {
            return false;
        }

        client.Renommer(dto.Nom);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var client = await _context.Clients.FindAsync(id);

        if (client is null)
        {
            return false;
        }

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();

        return true;
    }
}
