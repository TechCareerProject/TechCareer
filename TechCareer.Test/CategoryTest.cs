using AutoMapper;
using Moq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TechCareer.DataAccess.Repositories.Abstracts;
using TechCareer.Models.Dtos.Category;
using TechCareer.Models.Entities;
using TechCareer.Service.Concretes;
using TechCareer.Service.Constants;
using TechCareer.Service.Rules;
using Xunit;

namespace TechCareer.Tests.Services
{
    public class CategoryServiceTests
    {
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CategoryBusinessRules _businessRules;
        private readonly CategoryService _categoryService;

        public CategoryServiceTests()
        {
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _mockMapper = new Mock<IMapper>();
            _businessRules = new CategoryBusinessRules(_mockCategoryRepository.Object);
            _categoryService = new CategoryService(
                _mockCategoryRepository.Object,
                _mockMapper.Object,
                _businessRules
            );
        }

        [Fact]
        public async Task AddAsync_Should_Add_Category_When_Valid_Request()
        {
            // Arrange
            var createDto = new CreateCategoryRequestDto("Test Category");
            var categoryEntity = new Category { Id = 1, Name = "Test Category" };
            var responseDto = new CategoryResponseDto(1, "Test Category");

            _mockCategoryRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Category, bool>>>()))
                .ReturnsAsync(false); // No duplicate
            _mockMapper.Setup(x => x.Map<Category>(createDto)).Returns(categoryEntity);
            _mockCategoryRepository.Setup(x => x.AddAsync(categoryEntity)).ReturnsAsync(categoryEntity);
            _mockMapper.Setup(x => x.Map<CategoryResponseDto>(categoryEntity)).Returns(responseDto);

            // Act
            var result = await _categoryService.AddAsync(createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(responseDto.Id, result.Id);
            Assert.Equal(responseDto.name, result.name);
        }

        [Fact]
        public async Task AddAsync_Should_Throw_Exception_When_CategoryName_Already_Exists()
        {
            // Arrange
            var createDto = new CreateCategoryRequestDto("Existing Category");

            _mockCategoryRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Category, bool>>>()))
                .ReturnsAsync(true); // Duplicate exists

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _categoryService.AddAsync(createDto));

            Assert.Equal(CategoryMessages.CategoryTitleAlreadyExists, exception.Message);
        }

        [Fact]
        public async Task DeleteAsync_Should_Delete_Category_When_Category_Exists()
        {
            // Arrange
            int categoryId = 1;
            var categoryEntity = new Category { Id = categoryId, Name = "To Delete" };

            _mockCategoryRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Category, bool>>>()))
                .ReturnsAsync(categoryEntity);
            _mockCategoryRepository.Setup(x => x.DeleteAsync(categoryEntity, false)).Verifiable();

            // Act
            var result = await _categoryService.DeleteAsync(categoryId, false);

            // Assert
            Assert.Equal(CategoryMessages.CategoryDeleted, result);
            _mockCategoryRepository.Verify(x => x.DeleteAsync(categoryEntity, false), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_Should_Throw_Exception_When_Category_Not_Found()
        {
            // Arrange
            int categoryId = 99;

            _mockCategoryRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Category, bool>>>()))
                .ReturnsAsync((Category)null); // Not found

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _categoryService.DeleteAsync(categoryId, false));

            Assert.Equal(CategoryMessages.CategoryNotFound, exception.Message);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Category_When_Exists()
        {
            // Arrange
            int categoryId = 1;
            var categoryEntity = new Category { Id = categoryId, Name = "Existing Category" };
            var responseDto = new CategoryResponseDto(categoryId, "Existing Category");

            _mockCategoryRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Category, bool>>>()))
                .ReturnsAsync(categoryEntity);
            _mockMapper.Setup(x => x.Map<CategoryResponseDto>(categoryEntity)).Returns(responseDto);

            // Act
            var result = await _categoryService.GetByIdAsync(categoryId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(responseDto.Id, result.Id);
            Assert.Equal(responseDto.name, result.name);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Throw_Exception_When_Category_Not_Found()
        {
            // Arrange
            int categoryId = 99;

            _mockCategoryRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Category, bool>>>()))
                .ReturnsAsync((Category)null); // Not found

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _categoryService.GetByIdAsync(categoryId));

            Assert.Equal(CategoryMessages.CategoryNotFound, exception.Message);
        }
    }
}
