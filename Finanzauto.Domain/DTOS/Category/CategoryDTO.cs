﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finanzauto.Domain.DTOS.Category
{
    public class CategoryDTO
    {
        public long Id { get; set; }
        public string CategoryName { get; set; }
        public string? Description { get; set; }
        public string? Picture { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
