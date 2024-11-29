using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using TechCareer.Models.Entities;
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

        // Tüm Eventleri Listeleme
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] Expression<Func<Event, bool>>? filter = null,
            [FromQuery] Func<IQueryable<Event>, IOrderedQueryable<Event>>? orderBy = null,
            [FromQuery] bool include = false,
            [FromQuery] bool withDeleted = false,
            [FromQuery] bool enableTracking = true)
        {
            var events = await _eventService.GetListAsync(filter, orderBy, include, withDeleted, enableTracking);
            return Ok(events);
        }

        // Sayfalama ile Listeleme
        [HttpGet("paginate")]
        public async Task<IActionResult> GetPaginated(
            [FromQuery] int index = 0,
            [FromQuery] int size = 10,
            [FromQuery] Expression<Func<Event, bool>>? filter = null,
            [FromQuery] Func<IQueryable<Event>, IOrderedQueryable<Event>>? orderBy = null,
            [FromQuery] bool include = true,
            [FromQuery] bool withDeleted = false,
            [FromQuery] bool enableTracking = true)
        {
            var paginatedEvents = await _eventService.GetPaginateAsync(filter, orderBy, include, index, size, withDeleted, enableTracking);
            return Ok(paginatedEvents);
        }

        // Belirli Bir Event Detayı
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

        // Yeni Event Ekleme
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateEventRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdEvent = await _eventService.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdEvent.id }, createdEvent);
        }

        // Event Güncelleme
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateEventRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedEvent = await _eventService.UpdateAsync(id, dto);
            return Ok(updatedEvent);
        }

        // Event Silme
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, [FromQuery] bool permanent = false)
        {
            try
            {
                var result = await _eventService.DeleteAsync(id, permanent);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Event not found.");
            }
        }
    }
}
