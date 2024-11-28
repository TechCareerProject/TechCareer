using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechCareer.Models.Dtos.Roles;
using TechCareer.Models.Events;

namespace TechCareer.Service.Abstracts
{
    public interface IEventService
    {
        Task<EventResponseDto> AddAsync(CreateEventRequestDto dto);
        Task<string> DeleteAsync(Guid id);
        Task<List<EventResponseDto>> GetAllAsync();
        Task<EventResponseDto> GetByIdAsync(Guid id);
        Task<EventResponseDto> updateAsync(Guid id, UpdateEventRequestDto dto);
    }

}
