using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CmsShop.App_Data;
using CmsShop.Areas.Admin.Data;
using CmsShop.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CmsShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PagesController : Controller
    {
        private readonly AppDbContext _context;
        public PagesController(AppDbContext context)
        {
            _context = context;
        }
        
        public IActionResult Index()
        {
            List<PageViewModel> pagesList;
            pagesList = _context.Pages.ToArray().OrderBy(x => x.Sorting).Select(x=>new PageViewModel(x)).ToList();
            return View(pagesList);
        }

        //GET: /Pages/AddPage
        public IActionResult AddPage()
        {            
            return View();
        }

        //POST: /Pages/AddPage
        [HttpPost]
        public IActionResult AddPage(PageViewModel page)
        {
            if (!ModelState.IsValid)
            {
                return View(page);
            }
            string slug;
            var pageFromDb = _context.Pages;

            if (string.IsNullOrWhiteSpace(page.Slug))
            {
                slug = page.Title.Replace(" ", "-").ToLower();
            }
            else
            {
                slug = page.Slug.Replace(" ", "-").ToLower();
            }

            if(pageFromDb.Any(x=>x.Title==page.Title) || pageFromDb.Any(x => x.Slug == slug))
            {
                ModelState.AddModelError("", "Tytuł strony bądź adres już istnieje");
                return View(page);
            }
            var pageDTO = new Page
            {
                Title = page.Title,
                Slug = slug,
                Body = page.Body,
                HasSiteBar = page.HasSiteBar,
                Sorting = 1000
            };

            _context.Pages.Add(pageDTO);
            _context.SaveChanges();
            TempData["SM"] = "Dodałeś nową stronę";
            return RedirectToAction("AddPage");
        }

        //GET: Pages/Edit
        [HttpGet]
        public IActionResult EditPage(int id)
        {
            var pageEdit = _context.Pages.Find(id);
            if (pageEdit == null)
            {
                return Content("Strona nie istnieje");
            }
            var model = new PageViewModel(pageEdit);
            return View(model);
        }

        [HttpPost]
        public IActionResult EditPage([FromForm] PageViewModel page)
        {
            string slug="home";
            if (!ModelState.IsValid)
            {
                return View(page);
            }
            int id = page.Id;
            var pageEdited = _context.Pages.Find(id);
            
            if (page.Slug != "home")
            {
                if (string.IsNullOrWhiteSpace(page.Slug))
                {
                    slug = page.Title.Replace(" ", "-").ToLower();
                }
                else
                {
                    slug = page.Slug.Replace(" ", "-").ToLower();
                }
            }

            if(_context.Pages.Where(x=>x.Id != id).Any(x=>x.Title==page.Title) ||
                _context.Pages.Where(x => x.Id != id).Any(x => x.Slug == slug))
            {
                ModelState.AddModelError("", "Strona lub tytuł już istnieje");
            }

            pageEdited.Title = page.Title;
            pageEdited.Slug = slug;
            pageEdited.Body = page.Body;
            pageEdited.HasSiteBar = page.HasSiteBar;

            _context.SaveChanges();
            TempData["SM"] = "Zmodyfikowano stronę";

            return RedirectToAction(nameof(EditPage));
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var pageDetails = _context.Pages.FirstOrDefault(x => x.Id == id);
            if (pageDetails == null)
            {
                return Content("Strona o podanym id nie istnieje");
            }
            var model = new PageViewModel(pageDetails);
            return View(model);
        }

        //[HttpDelete("[area]/[controller]/Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var pageDelete = _context.Pages.FirstOrDefault(x => x.Id == id);
            if (pageDelete == null)
            {
                return Content("Strona nie istnieje");
            }
            _context.Pages.Remove(pageDelete);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public ActionResult ReorderPages(int[] id)
        {
            int counter = 1;
            Page dto;
            foreach (var pageId in id)
            {
                dto = _context.Pages.Find(pageId);
                dto.Sorting = counter;
                _context.SaveChanges();
                counter++;
            }
            return View();
        }
        [HttpGet]
        public ActionResult EditSidebar()
        {
            SidebarViewModel sidebar;
            var sidebarDto = _context.Sidebars.Find(1);
            sidebar = new SidebarViewModel(sidebarDto);
            return View(sidebar);
        }

        [HttpPost]
        public ActionResult EditSidebar([FromBody] SidebarViewModel model)
        {
            SidebarViewModel sidebar;
            var sidebarDto = _context.Sidebars.Find(1);
            sidebarDto.Body = model.Body;
            _context.SaveChanges();
            TempData["SM"] = "Edytowałeś pasek boczny";
            return RedirectToAction(nameof(EditSidebar));
        }
    }
}