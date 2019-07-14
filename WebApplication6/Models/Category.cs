using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication6.Models
{
    public class Category
    {
        public Category()
        {
        }

        public Category(int Id, string Name)
        {
            this.Id = Id;
            this.Name = Name;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
