using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CmsShop.App_Data;
using CmsShop.Models.Data;
using CmsShop.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CmsShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ShopController : Controller
    {
        private readonly AppDbContext _context;
        private IHostingEnvironment _env;

        public ShopController(AppDbContext context, IHostingEnvironment env)
        {
            _context = context;
            _env = env;
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

            var productDTO = new ProductDTO
            {
                Name=product.Name,
                CategoryId=product.CategoryId,
                Description=product.Description,
                Price=product.Price                
            };

            var catDTO = _context.Categories.FirstOrDefault(x => x.Id == product.CategoryId);
            productDTO.CategoryName = catDTO.Name;
            _context.Products.Add(productDTO);
            _context.SaveChanges();
            var id = productDTO.Id;

            TempData["SM"] = "Dodałeś produkt";

            #region Upload Image
            var webRoot = _env.WebRootPath;
            var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Upload", webRoot));
            var pathProducts = Path.Combine(originalDirectory.ToString(), "Products");
            var pathProductId = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString());
            var pathThumbs = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString()+"\\Thumbs");
            var pathGallery = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery");
            var pathGalleryThumbs = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery\\Thumbs");

            if (!Directory.Exists(pathProducts))
                Directory.CreateDirectory(pathProducts);
            if (!Directory.Exists(pathProductId))
                Directory.CreateDirectory(pathProductId);
            if (!Directory.Exists(pathThumbs))
                Directory.CreateDirectory(pathThumbs);            
            if (!Directory.Exists(pathGallery))
                Directory.CreateDirectory(pathGallery);
            if (!Directory.Exists(pathGalleryThumbs))
                Directory.CreateDirectory(pathGalleryThumbs);

            //check file ist an image
            if(file !=null && file.Length>0)
            {
                string ext = file.ContentType.ToLower();
                if(ext !="image/jpg" && ext != "image/jpeg" && ext != "image/pjpeg" && ext != "image/gif" && ext != "image/png" && ext != "image/x-png")
                {
                    ModelState.AddModelError("", "Nieprawidłowy format obrazu");
                    product.Categories = new SelectList(_context.Categories.ToList(), "Id", "Name");
                    return View(product);
                }

            }
            //init file name
            string imageName = file.FileName;
           
             //save file name to database
             var dto = _context.Products.Find(id);
            dto.ImageName = imageName;
            _context.SaveChanges();

            var path = $"{pathProductId}\\{imageName}";
            var path2 = $"{pathThumbs}\\{imageName}";
            //file.OpenReadStream().Write(path);
            #endregion
            return RedirectToAction(nameof(AddProduct));
        }
    }
}