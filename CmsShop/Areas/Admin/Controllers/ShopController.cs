using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CmsShop.App_Data;
using CmsShop.Models.Data;
using CmsShop.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        [HttpPost]
        public IActionResult ReorderCat(int[] id)
        {
            
            int count = 1;
            CategoryDTO catDto;
            foreach (var catId in id)
            {
                catDto = _context.Categories.Find(catId);
                catDto.Sorting = count;
                _context.SaveChanges();
                count++;
            }
            return RedirectToAction(nameof(Categories));
        }

        //[HttpPost]
        public IActionResult DeleteCategory(int id)
        {
            var cattoDel=_context.Categories.Find(id);
            _context.Categories.Remove(cattoDel);
            _context.SaveChanges();
            return RedirectToAction(nameof(Categories));
        }

        [HttpPost]
        public string RenameCategory(string newCatName, int id)
        {
            if (_context.Categories.Any(x => x.Name == newCatName))
            {
                return "tytulzajety";
            }
            var catDto = _context.Categories.Find(id);
            catDto.Name = newCatName;
            catDto.Slug = newCatName.Replace(" ", "-");
            //_context.Categories.Add(catDto);
            _context.SaveChanges();
            return "Ok";
        }

        [HttpGet]
        public IActionResult AddProduct()
        {
            ProductViewModel product = new ProductViewModel();
            product.Categories = new SelectList(_context.Categories.ToList(), "Id", "Name");
            return View(product);
        }

        [HttpPost]
        public IActionResult AddProduct([FromForm] ProductViewModel product, IFormFile file)
        {
            if (!ModelState.IsValid)
            {
                product.Categories = new SelectList(_context.Categories.ToList(), "Id", "Name");
                return View(product);
            }

            if (_context.Products.Any(x => x.Name == product.Name))
            {
                product.Categories = new SelectList(_context.Categories.ToList(), "Id", "Name");
                ModelState.AddModelError("", "Ta nazwa produktu jest zajęta");
                return View(product);
            }
            var id = _context.Products.Find(product.id);
            return View(product);
        }
    }
}