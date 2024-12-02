using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechCareer.Models.Dtos.Instructors;

public sealed record UpdateInstructorRequestDto(Guid Id,string Name, string About);
