using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CmsShop.App_Data;
using Microsoft.AspNetCore.Mvc;

namespace CmsShop.Controllers
{
    public class TestAjaxController : Controller
    {
        private AppDbContext _context;
        public TestAjaxController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public List<string> Index()
        {
            List<string> List = new List<string>();
            List.Add("sourav kayal");
            List.Add("Ajay Joshi");
            List.Add("Nontey Banerjee");
            return List;
        }

        [HttpGet]
        public IActionResult Edit()
        {
            return View(_context.Pages.Find(1));
        }
    }
}