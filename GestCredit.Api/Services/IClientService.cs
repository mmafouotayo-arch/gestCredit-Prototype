using GestCredit.Api.DTOs;

namespace GestCredit.Api.Services;

public interface IClientService
{
    Task<PagedResultDto<ClientDto>> GetAllAsync(string? ville, string? sortBy, int page, int pageSize);
    Task<ClientDto?> GetByIdAsync(int id);
    Task<ClientDto> CreateAsync(CreateClientDto dto);
    Task<bool> UpdateAsync(int id, UpdateClientDto dto);
    Task<bool> DeleteAsync(int id);
}