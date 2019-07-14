using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication6.Infrastructure;
using WebApplication6.Models;
using WebApplication6.ViewModels;

namespace WebApplication6.Controllers
{
   
    public class CartController : Controller
    {
        private IProductRepository repository;
        public CartController(IProductRepository repo)
        {
            repository = repo;
        }
        public IActionResult Index(string returnUrl)
        {
            CartIndexViewModel cartViewModel = new CartIndexViewModel
            {
                Cart = GetCart(),
                ReturnUrl = returnUrl
            };
            return View("Index",cartViewModel);
        }

        [Route("api/{controller}")]
        [HttpGet]
        public Cart Get()
        {
            return GetCart();

        }


        [Route("api/{controller}")]
        [HttpPost]
        public decimal[] Post([FromBody] Product pro)
        {
            Product product = repository.GetAllProducts().FirstOrDefault(p => p.Id == pro.Id);

            Cart cart = GetCart();
            if (product != null)
            {
                cart.AddItem(product,pro.Quantity);
                SaveCart(cart);
            }
            
            decimal total = cart.CalculateTotal();
            decimal quantity = cart.Lines.Count();
            HttpContext.Session.SetString("total",total.ToString());
            HttpContext.Session.SetString("cartquantity", quantity.ToString());

            return new decimal[] { total, quantity };
        }
      
        [HttpPost]
        public IActionResult RemoveFromCart(int id)
        {
            Product product = repository.GetAllProducts()
                            .Where(p => p.Id == id)
                            .FirstOrDefault();
            Cart cart = GetCart();
            if (product != null)
            {
              
                cart.RemoveItem(product);
                SaveCart(cart);
            }
            decimal total = cart.CalculateTotal();
            decimal quantity = cart.Lines.Count();
            HttpContext.Session.SetString("total", total.ToString());
            HttpContext.Session.SetString("cartquantity", quantity.ToString());
            return RedirectToAction("Index");
        }
        private Cart GetCart()
        {
            Cart cart = HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart();
            return cart;
        }
        private void SaveCart(Cart cart)
        {
            HttpContext.Session.SetJson<Cart>("Cart", cart);
        }

      
    }
}