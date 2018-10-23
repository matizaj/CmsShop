using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CmsShop.App_Data;
using CmsShop.Models.Data;
using CmsShop.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CmsShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ShopController : Controller
    {
        private readonly AppDbContext _context;
        public ShopController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Categories()
        {
            return View(_context.Categories.ToArray().OrderBy(x=>x.Sorting).Select(x=>new CategoryViewModel(x)).ToList());
        }

        [HttpPost]
        public string AddNewCategory(string  catName)
        {
            string id;
            if (_context.Categories.Any(x => x.Name == catName))
            {
                return "tytulzajety";
            }
            CategoryDTO catDto = new CategoryDTO();
            catDto.Name = catName;
            catDto.Slug = catName.Replace(" ", "-").ToLower();
            catDto.Sorting = 1000;

            _context.Categories.Add(catDto);
            _context.SaveChanges();


            id = catDto.Id.ToString();

            return id;

        }
    }
}