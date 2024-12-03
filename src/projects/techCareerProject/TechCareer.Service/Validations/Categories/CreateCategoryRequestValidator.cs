using FluentValidation;
using TechCareer.Models.Dtos.Category;

namespace TechCareer.Service.Validations.Categories
{
    public class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequestDto>
    {
        public CreateCategoryRequestValidator()
        {
            // Kategori adı boş olamaz ve en az 3 karakter olmalı
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name is required.")
                .MinimumLength(3).WithMessage("Category name must be at least 3 characters long.");

            // Kategorinin benzersizliği gibi ek kurallar eklenebilir
        }
    }
<<<<<<< HEAD
}
=======
}
>>>>>>> 716439d72e0a58805e01a0ae2e996290e82d92e6
