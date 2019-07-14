using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication6.Models
{
    public static class ExtensionMethods
    {
        public static decimal GetTotalPrice(this IEnumerable<Product> products)
        {
            decimal total = 0;
            foreach(var p in products)
            {
                total += p.Price;
            }
            return total;
        }

        public static IEnumerable<Product> Filter(this IEnumerable<Product> products,Func<Product,bool> selector)
        {
            foreach(var p in products)
            {
                if(selector(p) == true)
                {
                    yield return p;
                }
            }
        }
    }
}
