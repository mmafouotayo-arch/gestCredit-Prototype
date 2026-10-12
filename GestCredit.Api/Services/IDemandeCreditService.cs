using GestCredit.Api.DTOs;
using GestCredit.Api.Models;

namespace GestCredit.Api.Services;

public interface IDemandeCreditService
{
    Task<PagedResultDto<DemandeCreditDto>> GetAllAsync(string? statut, int page, int pageSize);
    Task<DemandeCreditDto?> GetByIdAsync(int id);
    Task<DemandeCreditDto> CreateAsync(CreateDemandeCreditDto dto);
    Task<DemandeCreditDto?> ChangerStatutAsync(int id, StatutDemande nouveauStatut);
}