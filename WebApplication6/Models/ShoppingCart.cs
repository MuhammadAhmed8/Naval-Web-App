using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication6.Models
{
    public class ShoppingCart
    {
        public ShoppingCart() { }
        public ShoppingCart(IEnumerable<Product> Products)
        {
            this.Products = Products;
        }
        public IEnumerable<Product> Products { get; set; }
        public decimal TotalPrice { get; set; }

    }
}
