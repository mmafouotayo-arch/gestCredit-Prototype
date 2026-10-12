using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GestCredit.Api.DTOs;
using GestCredit.Api.Models;
using GestCredit.Api.Services;

namespace GestCredit.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DemandesCreditController : ControllerBase
{
    private readonly IDemandeCreditService _service;

    public DemandesCreditController(IDemandeCreditService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResultDto<DemandeCreditDto>>> GetDemandes(
        [FromQuery] string? statut,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        return Ok(await _service.GetAllAsync(statut, page, pageSize));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DemandeCreditDto>> GetDemande(int id)
    {
        var demande = await _service.GetByIdAsync(id);
        return demande is null ? NotFound() : Ok(demande);
    }

    // L'Agent crée la demande
    [HttpPost]
    [Authorize(Roles = "Agent,Administrateur")]
    public async Task<ActionResult<DemandeCreditDto>> CreateDemande(CreateDemandeCreditDto dto)
    {
        var demande = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetDemande), new { id = demande.Id }, demande);
    }

    // L'Agent soumet la demande (Brouillon -> Soumise)
    [HttpPost("{id}/soumettre")]
    [Authorize(Roles = "Agent,Administrateur")]
    public async Task<ActionResult<DemandeCreditDto>> Soumettre(int id)
    {
        var demande = await _service.ChangerStatutAsync(id, StatutDemande.Soumise);
        return demande is null ? NotFound() : Ok(demande);
    }

    // L'Analyste décide (EnAnalyse, Approuvee, Rejetee)
    [HttpPost("{id}/statut")]
    [Authorize(Roles = "Analyste,Administrateur")]
    public async Task<ActionResult<DemandeCreditDto>> ChangerStatut(int id, ChangerStatutDto dto)
    {
        var nouveauStatut = Enum.Parse<StatutDemande>(dto.NouveauStatut, ignoreCase: true);
        var demande = await _service.ChangerStatutAsync(id, nouveauStatut);
        return demande is null ? NotFound() : Ok(demande);
    }
}