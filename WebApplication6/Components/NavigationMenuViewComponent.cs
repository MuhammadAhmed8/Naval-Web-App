using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication6.Models;

namespace WebApplication6.Components
{
    public class NavigationMenuViewComponent:ViewComponent
    {
        private ICategoryRepository repository;
        public NavigationMenuViewComponent(ICategoryRepository repo)
        {
            repository = repo;
        }
        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedCategory = RouteData?.Values["category"];
            return View(repository.GetCategories());
        }
    }
}
