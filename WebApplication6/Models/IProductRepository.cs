using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication6.Models
{
    public interface IProductRepository
    {

        IQueryable<Product> GetAllProducts();
        IQueryable<Product> GetProductsByCategory(string category);
        void SaveProduct(Product product);
    }
}
