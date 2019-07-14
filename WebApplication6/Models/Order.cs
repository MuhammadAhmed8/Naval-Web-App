using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication6.Models
{
    public class Order
    {
        [BindNever]
        public int OrderID { get; set; }

        [BindNever]
        public ICollection<CartLine> OrderLines { get; set; }

        [Required(ErrorMessage = "Please Enter Your Name")]
        [DataType(DataType.Text)]
        [Display(Name="Full Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please Provide The Complete Address")]
        [Display(Name="Delivery Address")]
        public string Address { get; set; }
        
        [Required(ErrorMessage = "Please Provide The Contact Number")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name="Mobile Number")]
        [RegularExpression(@"^((\\92)|(0092))-{0,1}\d{3}-{0,1}\d{7}$|^\d{11}$|^\d{4}-\d{7}$", ErrorMessage = "Not A Valid Mobile Number")]
        public string Phone { get; set; }

        [DataType(DataType.Text)]
        [Display(Name="Notes (optional)")]
        public string AdditionalDetails { get; set; }

        [BindNever]
        public bool isDelivered { get; set; }
    }
}
