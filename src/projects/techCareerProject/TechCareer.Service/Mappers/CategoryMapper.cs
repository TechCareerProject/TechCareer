﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechCareer.Models.Dtos.Category;
using TechCareer.Models.Dtos.Events;
using TechCareer.Models.Entities;

namespace TechCareer.Service.Mappers
{
    public class CategoryMapper : Profile
    {
        public CategoryMapper()
        {

            CreateMap<CreateCategoryRequestDto, Category>();
            CreateMap<Category, CategoryResponseDto>();
            CreateMap<UpdateCategoryRequestDto, Category>();


        }
    }
<<<<<<< HEAD
}
=======
}
>>>>>>> 716439d72e0a58805e01a0ae2e996290e82d92e6
