using Ember.Domain.Data;
using Ember.Service;
using Ember.WebServer.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ember.WebServer.Areas.People.Controllers;

[ApiController]
[Route("api/v01/[controller]/[action]")]
[Authorize]
public sealed class InvitationController(
    IEmberDbContext dbContext) : ControllerBase
{
    [HttpPost]
    [Authorize(Policy = PolicyConstants.AllowToInviteUser)]
    public async Task<IActionResult> CreateInvitation(CreateInvitationDto dto)
    {
        var userIdStr = User.FindFirst("sub")?.Value;
        if (userIdStr is null) return Unauthorized();

        var invitation = new Invitation
        {
            Id = Guid.NewGuid(),
            InvitedByUserId = Guid.Parse(userIdStr),
            RealName = dto.RealName,
            IsInLegalAge = dto.IsInLegalAge,
            Jurisdiction = dto.Jurisdiction,
            Email = dto.Email,
            Phone = dto.Phone,
            InviteCode = GenerateInviteCode(),
            CreatedAt = DateTimeOffset.UtcNow,
            ExpiresAt = dto.ExpiresAt,
        };

        dbContext.Invitations.Add(invitation);
        await dbContext.SaveChangesAsync();

        return Ok(new InvitationDto(invitation));
    }

    [HttpGet]
    public async Task<ActionResult<List<InvitationDto>>> GetMyInvitations()
    {
        var userIdStr = User.FindFirst("sub")?.Value;
        if (userIdStr is null) return Unauthorized();

        var userId = Guid.Parse(userIdStr);
        var invitations = await dbContext.Invitations
            .Where(i => i.InvitedByUserId == userId)
            .OrderByDescending(i => i.CreatedAt)
            .Select(i => new InvitationDto(i))
            .ToListAsync();

        return Ok(invitations);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<InvitationPreviewDto>> GetByCode([FromQuery] string code)
    {
        var invitation = await dbContext.Invitations
            .FirstOrDefaultAsync(i => i.InviteCode == code);

        if (invitation == null
            || invitation.AcceptedAt.HasValue
            || (invitation.ExpiresAt.HasValue && invitation.ExpiresAt < DateTimeOffset.UtcNow))
        {
            return NotFound(new { error = "Invalid or expired invitation code" });
        }

        return Ok(new InvitationPreviewDto(invitation));
    }

    private static string GenerateInviteCode()
        => Guid.NewGuid().ToString("N")[..16].ToUpperInvariant();
}

public record CreateInvitationDto(
    string RealName,
    bool IsInLegalAge,
    string Jurisdiction,
    string? Email,
    string? Phone,
    DateTimeOffset? ExpiresAt);

public record InvitationDto
{
    public Guid Id { get; init; }
    public string InviteCode { get; init; }
    public string RealName { get; init; }
    public bool IsInLegalAge { get; init; }
    public string Jurisdiction { get; init; }
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? ExpiresAt { get; init; }
    public DateTimeOffset? AcceptedAt { get; init; }
    public Guid? AcceptedByUserId { get; init; }

    public InvitationDto(Invitation i)
    {
        Id = i.Id;
        InviteCode = i.InviteCode;
        RealName = i.RealName;
        IsInLegalAge = i.IsInLegalAge;
        Jurisdiction = i.Jurisdiction;
        Email = i.Email;
        Phone = i.Phone;
        CreatedAt = i.CreatedAt;
        ExpiresAt = i.ExpiresAt;
        AcceptedAt = i.AcceptedAt;
        AcceptedByUserId = i.AcceptedByUserId;
    }
}

/// <summary>Minimal info returned to anonymous users previewing an invite before registration.</summary>
public record InvitationPreviewDto
{
    public string RealName { get; init; }
    public bool IsInLegalAge { get; init; }
    public string Jurisdiction { get; init; }
    public DateTimeOffset? ExpiresAt { get; init; }

    public InvitationPreviewDto(Invitation i)
    {
        RealName = i.RealName;
        IsInLegalAge = i.IsInLegalAge;
        Jurisdiction = i.Jurisdiction;
        ExpiresAt = i.ExpiresAt;
    }
}
