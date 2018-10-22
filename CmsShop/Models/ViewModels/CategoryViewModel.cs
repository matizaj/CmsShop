using CmsShop.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShop.Models.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public int Sorting { get; set; }

        public CategoryViewModel()
        {

        }
        public CategoryViewModel(CategoryDTO row)
        {
            Id = row.Id;
            Name = row.Name;
            Slug = row.Slug;
            Sorting = row.Sorting;
        }
    }
}
