using System.Security.Claims;
using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventServiceProvider.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EventsController(IEventService eventService) : ControllerBase
{
    private readonly IEventService _eventService = eventService;

    // GET: api/events
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _eventService.GetEventsAsync();
        return result.Success ? Ok(result.Result) : StatusCode(result.StatusCode, result.Error);
    }

    // GET: api/events/mine
    [HttpGet("mine")]
    public async Task<IActionResult> GetMine()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized("Missing or invalid user ID.");

        var result = await _eventService.GetEventsByCreatorAsync(userId);
        return result.Success ? Ok(result.Result) : StatusCode(result.StatusCode, result.Error);
    }

    // GET: api/events/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _eventService.GetEventByIdAsync(id);
        return result.Success ? Ok(result.Result) : StatusCode(result.StatusCode, result.Error);
    }

    // POST: api/events
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEventRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized("Missing or invalid user ID.");

        request.CreatorId = userId;

        var result = await _eventService.CreateEventAsync(request, userIdClaim);
        return result.Success ? StatusCode(201) : StatusCode(result.StatusCode, result.Error);
    }

    // PUT: api/events/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateEventRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userIdClaim))
            return Unauthorized("Missing or invalid user ID.");

        var result = await _eventService.UpdateEventAsync(id, request, userIdClaim);
        return result.Success ? NoContent() : StatusCode(result.StatusCode, result.Error);
    }
    // DELETE: api/events/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userIdClaim))
            return Unauthorized("Missing or invalid user ID.");

        var result = await _eventService.DeleteEventAsync(id, userIdClaim);
        return result.Success ? NoContent() : StatusCode(result.StatusCode, result.Error);
    }

}
