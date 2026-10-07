using Microsoft.AspNetCore.Mvc;
using GestCredit.Api.DTOs;
using GestCredit.Api.Services;

namespace GestCredit.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientsController(IClientService clientService)
    {
        _clientService = clientService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClientDto>>> GetClients()
    {
        return Ok(await _clientService.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ClientDto>> GetClient(int id)
    {
        var client = await _clientService.GetByIdAsync(id);
        return client is null ? NotFound() : Ok(client);
    }

    [HttpPost]
    public async Task<ActionResult<ClientDto>> CreateClient(CreateClientDto dto)
    {
        var client = await _clientService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetClient), new { id = client.Id }, client);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateClient(int id, UpdateClientDto dto)
    {
        bool updated = await _clientService.UpdateAsync(id, dto);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClient(int id)
    {
        bool deleted = await _clientService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}