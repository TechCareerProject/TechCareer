﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechCareer.Models.Dtos.Category
{
    public sealed record UpdateCategoryRequestDto (int Id, string name)
    {
    }
}