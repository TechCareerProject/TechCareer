﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechCareer.Models.Events
{
    public sealed record CreateEventRequestDto(
        
        string Title, //att
        string Description,
        string ImageUrl,
        string ParticipationText,
        int CategoryId)

    {
    }
}
