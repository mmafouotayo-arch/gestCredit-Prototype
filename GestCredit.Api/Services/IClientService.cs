using GestCredit.Api.DTOs;

namespace GestCredit.Api.Services;

public interface IClientService
{
    Task<IEnumerable<ClientDto>> GetAllAsync();
    Task<ClientDto?> GetByIdAsync(int id);
    Task<ClientDto> CreateAsync(CreateClientDto dto);
    Task<bool> UpdateAsync(int id, UpdateClientDto dto);
    Task<bool> DeleteAsync(int id);
}