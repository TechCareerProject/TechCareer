using Microsoft.AspNetCore.Mvc;
using TechCareer.Models.Events;
using TechCareer.Service.Abstracts;

namespace TechCareer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var events = await _eventService.GetAllAsync();
            return Ok(events);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var eventDetails = await _eventService.GetByIdAsync(id);
                return Ok(eventDetails);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Event not found.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateEventRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdEvent = await _eventService.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdEvent.id }, createdEvent);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateEventRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedEvent = await _eventService.updateAsync(id, dto);
            return Ok(updatedEvent);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var result = await _eventService.DeleteAsync(id);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Event not found.");
            }
        }
    }
}
