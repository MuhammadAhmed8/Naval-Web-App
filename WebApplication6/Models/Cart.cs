using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication6.Models
{
    public class Cart
    {
        private List<CartLine> LineCollection = new List<CartLine>();

        public virtual void AddItem(Product product,float quantity)
        {
            CartLine line = LineCollection
                            .Where(l => l.Product.Id == product.Id)
                            .FirstOrDefault();
           
          
            if(line == null)
            {
                LineCollection.Add(new CartLine
                {
                    Product = product,
                    Quantity = 1
                });
            }
            else
            {
                if (quantity == -1)
                    line.Quantity += 1;
                else
                    line.Quantity = (float)quantity;
            }
        }

   
        public virtual void RemoveItem(Product product)
        {
            LineCollection.RemoveAll(l => l.Product.Id == product.Id);
        }
        public virtual decimal CalculateTotal()
        {
            decimal total = 0;
            foreach(var l in LineCollection)
            {
                total += l.Product.Price * (decimal)l.Quantity;
            }
            return total;

        }
        public virtual void Clear()
        {
            LineCollection.Clear();
        }
        public IEnumerable<CartLine> Lines
        {
            get
            {
                return LineCollection;
            }
        }      
    }

    public class CartLine
    {
        public int CartLineID { get; set; }
        public Product Product { get; set; }
        public float Quantity { get; set; }
    }
}
