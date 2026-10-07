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

    public async Task<PagedResultDto<ClientDto>> GetAllAsync(string? ville, string? sortBy, int page, int pageSize)
    {
        IQueryable<Client> query = _context.Clients;

        if (!string.IsNullOrWhiteSpace(ville))
        {
            query = query.Where(c => c.Ville.Contains(ville));
        }

        query = sortBy?.ToLower() switch
        {
            "ville" => query.OrderBy(c => c.Ville),
            "nom" => query.OrderBy(c => c.Nom),
            _ => query.OrderBy(c => c.Id)
        };

        int totalCount = await query.CountAsync();

        List<ClientDto> items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new ClientDto(c.Id, c.Nom, c.Ville))
            .ToListAsync();

        return new PagedResultDto<ClientDto>(items, page, pageSize, totalCount);
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