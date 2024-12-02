using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TechCareer.Models.Dtos.Instructors;
using TechCareer.Models.Entities;

namespace TechCareer.Service.Mappers
{
    public class InstructorMapper : Profile
    {
        public InstructorMapper()
        {
            CreateMap<CreateInstructorRequestDto, Instructor>();
            CreateMap<Instructor, InstructorResponseDto>();
            CreateMap<UpdateInstructorRequestDto, Instructor>();
        }
    }
}
