using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication6.Models
{
    public class Product
    {
        public Product() { }
        public Product(int Id,string Name, decimal Price,string Description,float Quantity,string Image,bool isAvailable = true)
        {
            this.Id = Id;
            this.Name = Name;
            this.Price = Price;
            this.Description = Description;
            this.Quantity = Quantity;
            this.Image = Image;
            this.isAvailable = isAvailable;

        }
        public int? Id { get; set; }

        [Required(ErrorMessage = "Please Enter Product's Name")]
        
        public string Name { get; set; }

        [Required(ErrorMessage = "Please Enter Price")]
        [Range(0.01,double.MaxValue,ErrorMessage = "Enter a Positive Value")]
        public decimal Price { get; set; }

        public string Description { get; set; }

        public float Quantity { get; set; }

        [DataType(DataType.Upload)]
        public string Image { get; set; }

        [Display(Name="In Stock")]
        public bool isAvailable { get; set; }

        [Required(ErrorMessage = "Please Specify A Category")]
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }
    }

    public struct ProductQuantity
    {
        public int ProductId { get; set; }
        public float Quantity { get; set; }
    }
}
