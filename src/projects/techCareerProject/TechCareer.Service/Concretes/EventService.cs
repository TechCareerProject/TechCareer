using AutoMapper;
using Core.AOP.Aspects;
using Core.Persistence.Extensions;
using System.Linq.Expressions;
using TechCareer.DataAccess.Repositories.Abstracts;
using TechCareer.Models.Dtos.Roles;
using TechCareer.Models.Entities;
using TechCareer.Models.Events;
using TechCareer.Service.Abstracts;
using TechCareer.Service.Constants;
using TechCareer.Service.Rules;

namespace TechCareer.Service.Concretes
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;
        private readonly EventBusinessRules _businessRules;

        public EventService(IEventRepository eventRepository, IMapper mapper, EventBusinessRules businessRules)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
            _businessRules = businessRules;
        }

        [LoggerAspect]
        [ClearCacheAspect("Events")]
        [AuthorizeAspect("Admin")]
        public async Task<EventResponseDto> AddAsync(CreateEventRequestDto dto)
        {
            await _businessRules.EventTitleMustBeUnique(dto.Title);

            var eventEntity = _mapper.Map<Event>(dto);
            eventEntity.Id = Guid.NewGuid();

            var addedEvent = await _eventRepository.AddAsync(eventEntity);
            return _mapper.Map<EventResponseDto>(addedEvent);
        }

        [LoggerAspect]
        [ClearCacheAspect("Events")]
        [AuthorizeAspect("Admin")]
        public async Task<string> DeleteAsync(Guid id, bool permanent = false)
        {
            var eventEntity = await _businessRules.EventMustExist(id);
            await _eventRepository.DeleteAsync(eventEntity, permanent);
            return EventMessages.EventDeleted;
        }

        [LoggerAspect]
        [ClearCacheAspect("Events")]
        [AuthorizeAspect("Admin")]
        public async Task<EventResponseDto> UpdateAsync(Guid id, UpdateEventRequestDto dto)
        {
            var eventEntity = await _businessRules.EventMustExist(id);

            _mapper.Map(dto, eventEntity);

            var updatedEvent = await _eventRepository.UpdateAsync(eventEntity);
            return _mapper.Map<EventResponseDto>(updatedEvent);
        }

        [CacheAspect(cacheKeyTemplate: "EventList", bypassCache: false, cacheGroupKey: "Events")]
        public async Task<List<EventResponseDto>> GetListAsync(
            Expression<Func<Event, bool>>? predicate = null,
            Func<IQueryable<Event>, IOrderedQueryable<Event>>? orderBy = null,
            bool include = false,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default)
        {
            var events = await _eventRepository.GetListAsync(predicate, orderBy, include, withDeleted, enableTracking, cancellationToken);
            return _mapper.Map<List<EventResponseDto>>(events);
        }


        public async Task<Paginate<EventResponseDto>> GetPaginateAsync(
            Expression<Func<Event, bool>>? predicate = null,
            Func<IQueryable<Event>, IOrderedQueryable<Event>>? orderBy = null,
            bool include = true,
            int index = 0,
            int size = 10,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default)
        {
            // Repository'den Paginate<Event> verisini alıyoruz
            var events = await _eventRepository.GetPaginateAsync(
                predicate,
                orderBy,
                include,
                index,
                size,
                withDeleted,
                enableTracking,
                cancellationToken
            );

            // Paginate<EventResponseDto> nesnesi oluşturuyoruz
            return new Paginate<EventResponseDto>
            {
                Items = _mapper.Map<IList<EventResponseDto>>(events.Items),
                Index = events.Index,
                Size = events.Size,
                Count = events.Count,
                Pages = events.Pages
            };
        }



        public async Task<EventResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var eventEntity = await _businessRules.EventMustExist(id);
            return _mapper.Map<EventResponseDto>(eventEntity);
        }
    }
}
