using AutoMapper;
using Core.AOP.Aspects;
using Core.Persistence.Extensions;
using System.Linq.Expressions;
using TechCareer.DataAccess.Repositories.Abstracts;
using TechCareer.Models.Dtos.Categories;
using TechCareer.Models.Entities;
using TechCareer.Models.Events;
using TechCareer.Service.Abstracts;
using TechCareer.Service.Constants;
using TechCareer.Service.Rules;

namespace TechCareer.Service.Concretes
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly CategoryBusinessRules _businessRules;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper, CategoryBusinessRules businessRules)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _businessRules = businessRules;
        }

        [LoggerAspect]
        [ClearCacheAspect("Categories")]
        [AuthorizeAspect("Admin")]
        public async Task<CategoryResponseDto> AddAsync(CreateCategoryRequestDto dto)
        {
            // Kategorinin adı benzersiz olmalı
            await _businessRules.CategoryNameMustBeUnique(dto.Name);

            var categoryEntity = _mapper.Map<Category>(dto);
            categoryEntity.Id = Guid.NewGuid(); // Id'yi yeni bir GUID ile oluşturuyoruz

            var addedCategory = await _categoryRepository.AddAsync(categoryEntity);
            return _mapper.Map<CategoryResponseDto>(addedCategory);
        }

        [LoggerAspect]
        [ClearCacheAspect("Categories")]
        [AuthorizeAspect("Admin")]
        public async Task<string> DeleteAsync(Guid id, bool permanent = false)
        {
            var categoryEntity = await _businessRules.CategoryMustExist(id);
            await _categoryRepository.DeleteAsync(categoryEntity, permanent);
            return CategoryMessages.CategoryDeleted;
        }

        [LoggerAspect]
        [ClearCacheAspect("Categories")]
        [AuthorizeAspect("Admin")]
        public async Task<CategoryResponseDto> UpdateAsync(Guid id, UpdateCategoryRequestDto dto)
        {
            var categoryEntity = await _businessRules.CategoryMustExist(id);

            _mapper.Map(dto, categoryEntity);

            var updatedCategory = await _categoryRepository.UpdateAsync(categoryEntity);
            return _mapper.Map<CategoryResponseDto>(updatedCategory);
        }

        [CacheAspect(cacheKeyTemplate: "CategoryList", bypassCache: false, cacheGroupKey: "Categories")]
        public async Task<List<CategoryResponseDto>> GetListAsync(
            Expression<Func<Category, bool>>? predicate = null,
            Func<IQueryable<Category>, IOrderedQueryable<Category>>? orderBy = null,
            bool include = false,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default)
        {
            var categories = await _categoryRepository.GetListAsync(predicate, orderBy, include, withDeleted, enableTracking, cancellationToken);
            return _mapper.Map<List<CategoryResponseDto>>(categories);
        }

        public async Task<Paginate<CategoryResponseDto>> GetPaginateAsync(
            Expression<Func<Category, bool>>? predicate = null,
            Func<IQueryable<Category>, IOrderedQueryable<Category>>? orderBy = null,
            bool include = true,
            int index = 0,
            int size = 10,
            bool withDeleted = false,
            bool enableTracking = true,
            CancellationToken cancellationToken = default)
        {
            // Repository'den Paginate<Category> verisini alıyoruz
            var categories = await _categoryRepository.GetPaginateAsync(
                predicate,
                orderBy,
                include,
                index,
                size,
                withDeleted,
                enableTracking,
                cancellationToken
            );

            // Paginate<CategoryResponseDto> nesnesi oluşturuyoruz
            return new Paginate<CategoryResponseDto>
            {
                Items = _mapper.Map<IList<CategoryResponseDto>>(categories.Items),
                Index = categories.Index,
                Size = categories.Size,
                Count = categories.Count,
                Pages = categories.Pages
            };
        }

        public async Task<CategoryResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var categoryEntity = await _businessRules.CategoryMustExist(id);
            return _mapper.Map<CategoryResponseDto>(categoryEntity);
        }
    }
}
