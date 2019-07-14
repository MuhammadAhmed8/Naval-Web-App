using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication6.Models;

namespace WebApplication6.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository repository;
        private ICategoryRepository catrepo;
        public int PageSize = 100;
        public ProductController(IProductRepository repo,ICategoryRepository cat)
        {
            repository = repo;
            catrepo = cat;
            
        }

        public ViewResult Index(int Page = 1)
        {
            ViewData["PageSize"]= PageSize;
            ViewBag.Categories = catrepo.GetCategories();
            var Products = repository.GetAllProducts();
            ViewData["ItemCount"] = Products.Count();
            return View(Products
                        .OrderBy(p=>p.Id)
                        .Skip((Page-1)*PageSize)
                        .Take(PageSize)
                );
            

        }
        [Route("Product/{category}")]
        public ViewResult ProductsByCategory(string category = " " ,int Page = 1) {
            ViewData["PageSize"] = PageSize;
            ViewData["current_cat"] = category;
            var Products = repository.GetProductsByCategory(category);
            ViewData["ItemCount"] = Products.Count();
            ViewBag.Categories = catrepo.GetCategories();
            return View("Index",Products
                        .OrderBy(p => p.Id)
                        .Skip((Page - 1) * PageSize)
                        .Take(PageSize)

                );

        }
        
        
    }
}