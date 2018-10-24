using CmsShop.Models.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShop.Models.ViewModels
{
    public class ProductViewModel
    {
        public ProductViewModel()
        {
        }

        public ProductViewModel(ProductDTO row)
        {
            Id = row.Id;
            Name = row.Name;
            Description = row.Description;
            Price = row.Price;
            CategoryId = row.CategoryId;
            CategoryName = row.CategoryName;
            ImageName = row.ImageName;
        }

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string CategoryName { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public string ImageName { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<string> GalleryImages { get; set; }
    }
}
