﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication6.Models
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetCategories();
    }
}
