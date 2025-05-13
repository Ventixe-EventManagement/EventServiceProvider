using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EventServiceProvider.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventController(IEventService eventService) : ControllerBase
{
    private readonly IEventService _eventService = eventService;
   
}
