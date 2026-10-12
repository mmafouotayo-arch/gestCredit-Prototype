using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GestCredit.Api.DTOs;
using GestCredit.Api.Services;

namespace GestCredit.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Administrateur")]
public class UtilisateursController : ControllerBase
{
    private readonly IAuthService _authService;

    public UtilisateursController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPut("{id}/role")]
    public async Task<IActionResult> ChangerRole(int id, AssignRoleDto dto)
    {
        bool modifie = await _authService.ChangerRoleAsync(id, dto.Role);
        return modifie ? NoContent() : NotFound();
    }
}