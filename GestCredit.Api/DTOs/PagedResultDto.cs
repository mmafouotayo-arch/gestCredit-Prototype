namespace GestCredit.Api.DTOs;

public record PagedResultDto<T>(List<T> Items, int Page, int PageSize, int TotalCount);