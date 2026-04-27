using Ember.WebServer.Areas.Knowledge.Models;
using Ember.Domain.Data;
using Ember.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ember.Infrastructure;

namespace Ember.WebServer.Areas.People.Controllers;

[ApiController]
[Route("api/v01/[controller]")]
[Authorize]
public class TestimonialsController(EmberDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TestimonialModel>>> GetTestimonials()
    {
        var testimonials = await dbContext.Testimonials
            .Include(t => t.ByEmberUser)
            .Include(t => t.ForEmberUser)
            .Include(t => t.BadgeDefinition)
            .ToListAsync();

        var models = testimonials.Select(t => new TestimonialModel
        {
            Id = t.Id,
            ByEmberUserId = t.ByEmberUserId,
            ForEmberUserId = t.ForEmberUserId,
            BadgeDefinitionId = t.BadgeDefinitionId,
            ApprovesBooleanBadge = t.ApprovesBooleanBadge,
            NumericBadgeValue = t.NumericBadgeValue,
            Message = t.Message,
            FromTime = t.FromTime,
            ToTime = t.ToTime
        });

        return Ok(models);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TestimonialModel>> GetTestimonial(Guid id)
    {
        var testimonial = await dbContext.Testimonials
            .Include(t => t.ByEmberUser)
            .Include(t => t.ForEmberUser)
            .Include(t => t.BadgeDefinition)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (testimonial == null)
        {
            return NotFound();
        }

        var model = new TestimonialModel
        {
            Id = testimonial.Id,
            ByEmberUserId = testimonial.ByEmberUserId,
            ForEmberUserId = testimonial.ForEmberUserId,
            BadgeDefinitionId = testimonial.BadgeDefinitionId,
            ApprovesBooleanBadge = testimonial.ApprovesBooleanBadge,
            NumericBadgeValue = testimonial.NumericBadgeValue,
            Message = testimonial.Message,
            FromTime = testimonial.FromTime,
            ToTime = testimonial.ToTime
        };

        return Ok(model);
    }

    [HttpPost]
    public async Task<ActionResult<TestimonialModel>> CreateTestimonial(TestimonialCreateModel createModel)
    {
        var userId = Guid.Parse(User.FindFirst("sub")?.Value ?? Guid.Empty.ToString());

        var testimonial = new Testimonial
        {
            Id = Guid.NewGuid(),
            ByEmberUserId = userId,
            ForEmberUserId = createModel.ForEmberUserId,
            BadgeDefinitionId = createModel.BadgeDefinitionId,
            ApprovesBooleanBadge = createModel.ApprovesBooleanBadge,
            NumericBadgeValue = createModel.NumericBadgeValue,
            Message = createModel.Message,
            FromTime = createModel.FromTime,
            ToTime = createModel.ToTime
        };

        dbContext.Testimonials.Add(testimonial);
        await dbContext.SaveChangesAsync();

        var model = new TestimonialModel
        {
            Id = testimonial.Id,
            ByEmberUserId = testimonial.ByEmberUserId,
            ForEmberUserId = testimonial.ForEmberUserId,
            BadgeDefinitionId = testimonial.BadgeDefinitionId,
            ApprovesBooleanBadge = testimonial.ApprovesBooleanBadge,
            NumericBadgeValue = testimonial.NumericBadgeValue,
            Message = testimonial.Message,
            FromTime = testimonial.FromTime,
            ToTime = testimonial.ToTime
        };

        return CreatedAtAction(nameof(GetTestimonial), new { id = testimonial.Id }, model);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTestimonial(Guid id, TestimonialUpdateModel updateModel)
    {
        var existing = await dbContext.Testimonials.FindAsync(id);
        if (existing == null)
        {
            return NotFound();
        }

        // Check ownership
        var userId = Guid.Parse(User.FindFirst("sub")?.Value ?? Guid.Empty.ToString());
        if (existing.ByEmberUserId != userId)
        {
            return Forbid();
        }

        existing.BadgeDefinitionId = updateModel.BadgeDefinitionId;
        existing.ApprovesBooleanBadge = updateModel.ApprovesBooleanBadge;
        existing.NumericBadgeValue = updateModel.NumericBadgeValue;
        existing.Message = updateModel.Message;
        existing.FromTime = updateModel.FromTime;
        existing.ToTime = updateModel.ToTime;

        await dbContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTestimonial(Guid id)
    {
        var testimonial = await dbContext.Testimonials.FindAsync(id);
        if (testimonial == null)
        {
            return NotFound();
        }

        // Check ownership
        var userId = Guid.Parse(User.FindFirst("sub")?.Value ?? Guid.Empty.ToString());
        if (testimonial.ByEmberUserId != userId)
        {
            return Forbid();
        }

        dbContext.Testimonials.Remove(testimonial);
        await dbContext.SaveChangesAsync();

        return NoContent();
    }
}