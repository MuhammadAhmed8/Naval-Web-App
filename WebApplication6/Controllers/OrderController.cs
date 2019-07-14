using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication6.Infrastructure;
using WebApplication6.Models;

namespace WebApplication6.Controllers
{
    public class OrderController : Controller
    {
        private IOrderRepository repository;

        public OrderController(IOrderRepository repo)
        {
            repository = repo;
        }

        public ActionResult List()
        {
            if (HttpContext.Session.GetString("uid_a") == null || HttpContext.Session.GetString("mhk") != "admin")
            {
                return Redirect("/account/Login");
            }
                return View(repository.GetOrders());
        }
        [HttpPost]
        public IActionResult MarkDelivered(int OrderId)
        {
            if (HttpContext.Session.GetString("uid_a") == null || HttpContext.Session.GetString("mhk") != "admin")
            {
                return Redirect("/account/Login");
            }
            repository.MarkDelivered(OrderId);
            return RedirectToAction("List");
        }
        [HttpPost]
        public IActionResult Cancel(int OrderId)
        {
            if (HttpContext.Session.GetString("uid_a") == null || HttpContext.Session.GetString("mhk") != "admin")
            {
                return Redirect("/account/Login");
            }
            repository.CancelOrder(OrderId);
            return RedirectToAction("List");
        }

        public ViewResult Checkout()
        {
            return View(new Order());
        }

        [HttpPost]
        public ViewResult Checkout(Order order)
        {
            Cart cart = GetCart();
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry,Your cart is empty");
            }
            if (ModelState.IsValid)
            {
                order.OrderLines = cart.Lines.ToArray();
                repository.SaveOrder(order);
                HttpContext.Session.Remove("Cart");
                return View("Summary");
            }
            else
            {
                return View("Checkout");
            }
        }
        private Cart GetCart()
        {
            Cart cart = HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart();
            return cart;
        }
    }
}