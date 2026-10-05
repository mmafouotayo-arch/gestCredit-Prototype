using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestCredit.Api.Data;
using GestCredit.Api.Models;
using GestCredit.Api.DTOs;

namespace GestCredit.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly GestCreditDbContext _context;

    public ClientsController(GestCreditDbContext context)
    {
        _context = context;
    }

    // GET /api/clients
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClientDto>>> GetClients()
    {
        var clients = await _context.Clients
            .Select(c => new ClientDto(c.Id, c.Nom, c.Ville))
            .ToListAsync();

        return Ok(clients);
    }

    // GET /api/clients/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ClientDto>> GetClient(int id)
    {
        var client = await _context.Clients.FindAsync(id);

        if (client is null)
        {
            return NotFound();
        }

        return Ok(new ClientDto(client.Id, client.Nom, client.Ville));
    }

    // POST /api/clients
    [HttpPost]
    public async Task<ActionResult<ClientDto>> CreateClient(CreateClientDto dto)
    {
        Client client;

        try
        {
            client = new Client(dto.Nom, dto.Ville);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }

        _context.Clients.Add(client);
        await _context.SaveChangesAsync();

        var resultDto = new ClientDto(client.Id, client.Nom, client.Ville);

        return CreatedAtAction(nameof(GetClient), new { id = client.Id }, resultDto);
    }

    // PUT /api/clients/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateClient(int id, UpdateClientDto dto)
    {
        var client = await _context.Clients.FindAsync(id);

        if (client is null)
        {
            return NotFound();
        }

        try
        {
            client.Renommer(dto.Nom);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE /api/clients/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClient(int id)
    {
        var client = await _context.Clients.FindAsync(id);

        if (client is null)
        {
            return NotFound();
        }

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}