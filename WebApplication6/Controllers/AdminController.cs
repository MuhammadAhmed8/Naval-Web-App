using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication6.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace WebApplication6.Controllers
{
    public class AdminController : Controller
    {
        private IProductRepository productRepo;
        private ICategoryRepository categoryRepo;
        public AdminController(IProductRepository P_repo,ICategoryRepository C_repo)
        {
            productRepo = P_repo;
            categoryRepo = C_repo;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("uid_a") == null || HttpContext.Session.GetString("mhk") != "admin")
            {
                return Redirect("/account/Login");
            }
            return View(productRepo.GetAllProducts());
        }
      
        public IActionResult AddProduct()
        {
            if (HttpContext.Session.GetString("uid_a") == null || HttpContext.Session.GetString("mhk") != "admin")
            {
                return Redirect("/account/Login");
            }

            ViewBag.Categories = categoryRepo.GetCategories();
            return View("SaveProduct");
        }
        
      
        [HttpPost]
        public ActionResult AddProduct(Product product, IFormFile file)
        {
            if (HttpContext.Session.GetString("uid_a") == null || HttpContext.Session.GetString("mhk") != "admin")
            {
                return Redirect("/account/Login");
            }

            if (file != null && file.Length > 0)
            {

                if (file.ContentType == "image/jpeg" || file.ContentType == "image/jpg" || file.ContentType == "image/png" || file.ContentType == "image/gif")
                {
                    var ext = Path.GetExtension(file.FileName);
                    var path = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot/images",
                    DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString() + ext
                    );

                    product.Image = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString() + ext;

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyToAsync(stream);
                    }
                }
                else
                {
                    ModelState.AddModelError("file", "File type Error");

                }
            }
            else
            {
                if (product.Image == null)
                    product.Image = "default.jpg";
            }
          
            if (ModelState.IsValid)
            {
                productRepo.SaveProduct(product);
                TempData["message"] = "Item Saved Succesfully";
                return RedirectToAction("Index");
            }
           
            TempData["message"] = "Error: Item Failed To Save";
            ViewBag.Categories = categoryRepo.GetCategories();

            return View("SaveProduct");



        }

        public IActionResult EditProduct(int Id)
        {
            
            if (HttpContext.Session.GetString("uid_a") == null || HttpContext.Session.GetString("mhk") != "admin")
            {
                return Redirect("/account/Login");
            }

            ViewBag.Categories = categoryRepo.GetCategories();

            Product product = productRepo.GetAllProducts()
                                         .FirstOrDefault(p => p.Id == Id);
            if (product == null)
                return Redirect("/admin");

            return View("SaveProduct",product);
            
         
        }
    }
}