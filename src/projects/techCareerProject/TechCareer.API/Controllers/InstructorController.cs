using Core.Security.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechCareer.Service.Abstracts;

namespace TechCareer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorController : ControllerBase
    {
        private readonly IInstructorService _instructorService;

        public InstructorController(IInstructorService instructorService)
        {
            _instructorService = instructorService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Instructor>>> GetAll()
        {
            var instructors = await _instructorService.GetAllAsync();
            return Ok(instructors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Instructor>> GetById(Guid id)
        {
            var instructor = await _instructorService.GetByIdAsync(id);

            if (instructor == null)
                return NotFound();

            return Ok(instructor);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Instructor instructor)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _instructorService.AddAsync(instructor);
            return CreatedAtAction(nameof(GetById), new { id = instructor.Id }, instructor);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Instructor instructor)
        {
            if (id != instructor.Id)
                return BadRequest("ID mismatch.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingInstructor = await _instructorService.GetByIdAsync(id);
            if (existingInstructor == null)
                return NotFound();

            await _instructorService.UpdateAsync(instructor); // Burada değişken ataması gerekmez.
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid Guid)
        {
            var existingInstructor = await _instructorService.GetByIdAsync(Guid);
            if (existingInstructor == null)
                return NotFound();

            await _instructorService.DeleteAsync(Guid);
            return NoContent();
        }
    }
}
