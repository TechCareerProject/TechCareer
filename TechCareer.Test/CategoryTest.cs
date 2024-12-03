using AutoMapper;
using Moq;
using System;
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
        private readonly Mock<ICategoryRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CategoryBusinessRules _businessRules;
        private readonly CategoryService _service;

        public CategoryServiceTests()
        {
            _mockRepository = new Mock<ICategoryRepository>();
            _mockMapper = new Mock<IMapper>();
            _businessRules = new CategoryBusinessRules(_mockRepository.Object);
            _service = new CategoryService(_mockRepository.Object, _mockMapper.Object, _businessRules);
        }

        [Fact]
        public async Task AddAsync_Should_Add_Category_Successfully()
        {
            // Arrange
            var createDto = new CreateCategoryRequestDto("Test Category");
            var categoryEntity = new Category { Id = 1, Name = createDto.Name };
            var responseDto = new CategoryResponseDto(1, "Test Category");

            _mockRepository
                .Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Category, bool>>>()))
                .ReturnsAsync(false); // Name is unique

            _mockMapper
                .Setup(m => m.Map<Category>(createDto))
                .Returns(categoryEntity);

            _mockRepository
                .Setup(r => r.AddAsync(categoryEntity))
                .ReturnsAsync(categoryEntity);

            _mockMapper
                .Setup(m => m.Map<CategoryResponseDto>(categoryEntity))
                .Returns(responseDto);

            // Act
            var result = await _service.AddAsync(createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(responseDto.Id, result.Id);
            Assert.Equal(responseDto.name, result.name);
            _mockRepository.Verify(r => r.AddAsync(It.IsAny<Category>()), Times.Once);
        }

        [Fact]
        public async Task AddAsync_Should_Throw_Exception_When_CategoryName_Already_Exists()
        {
            // Arrange
            var createDto = new CreateCategoryRequestDto("Duplicate Name");

            _mockRepository
                .Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Category, bool>>>()))
                .ReturnsAsync(true); // Duplicate exists

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.AddAsync(createDto));

            Assert.Equal(CategoryMessages.CategoryTitleAlreadyExists, exception.Message);
        }

        [Fact]
        public async Task DeleteAsync_Should_Delete_Category_Successfully()
        {
            // Arrange
            var categoryId = 1;
            var categoryEntity = new Category { Id = categoryId, Name = "To Delete" };

            _mockRepository
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<Category, bool>>>()))
                .ReturnsAsync(categoryEntity);

            _mockRepository
                .Setup(r => r.DeleteAsync(categoryEntity, false))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.DeleteAsync(categoryId, false);

            // Assert
            Assert.Equal(CategoryMessages.CategoryDeleted, result);
            _mockRepository.Verify(r => r.DeleteAsync(categoryEntity, false), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_Should_Throw_Exception_When_Category_Not_Found()
        {
            // Arrange
            var categoryId = 99;

            _mockRepository
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<Category, bool>>>()))
                .ReturnsAsync((Category)null); // Not found

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _service.DeleteAsync(categoryId, false));

            Assert.Equal(CategoryMessages.CategoryNotFound, exception.Message);
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_Category_Successfully()
        {
            // Arrange
            var categoryId = 1;
            var updateDto = new UpdateCategoryRequestDto(categoryId, "Updated Name");
            var categoryEntity = new Category { Id = categoryId, Name = "Old Name" };

            _mockRepository
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<Category, bool>>>()))
                .ReturnsAsync(categoryEntity);

            _mockMapper
                .Setup(m => m.Map(updateDto, categoryEntity))
                .Callback(() => { categoryEntity.Name = updateDto.name; });

            _mockRepository
                .Setup(r => r.UpdateAsync(categoryEntity))
                .ReturnsAsync(categoryEntity);

            _mockMapper
                .Setup(m => m.Map<CategoryResponseDto>(categoryEntity))
                .Returns(new CategoryResponseDto(categoryId, updateDto.name));

            // Act
            var result = await _service.UpdateAsync(categoryId, updateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updateDto.Id, result.Id);
            Assert.Equal(updateDto.name, result.name);
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Category>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_Should_Throw_Exception_When_Category_Not_Found()
        {
            // Arrange
            var categoryId = 99;
            var updateDto = new UpdateCategoryRequestDto(categoryId, "New Name");

            _mockRepository
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<Category, bool>>>()))
                .ReturnsAsync((Category)null); // Not found

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _service.UpdateAsync(categoryId, updateDto));

            Assert.Equal(CategoryMessages.CategoryNotFound, exception.Message);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Category()
        {
            // Arrange
            var categoryId = 1;
            var categoryEntity = new Category { Id = categoryId, Name = "Test Category" };
            var responseDto = new CategoryResponseDto(categoryId, categoryEntity.Name);

            _mockRepository
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<Category, bool>>>()))
                .ReturnsAsync(categoryEntity);

            _mockMapper
                .Setup(m => m.Map<CategoryResponseDto>(categoryEntity))
                .Returns(responseDto);

            // Act
            var result = await _service.GetByIdAsync(categoryId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(categoryId, result.Id);
            Assert.Equal(categoryEntity.Name, result.name);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Throw_Exception_When_Category_Not_Found()
        {
            // Arrange
            var categoryId = 99;

            _mockRepository
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<Category, bool>>>()))
                .ReturnsAsync((Category)null); // Not found

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _service.GetByIdAsync(categoryId));

            Assert.Equal(CategoryMessages.CategoryNotFound, exception.Message);
        }
    }
}
