using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CmsShop.Areas.Admin.Data;

namespace CmsShop.Models.ViewModels
{
    public class PageViewModel
    {
        public PageViewModel()
        {
        }
        public PageViewModel(Page row)
        {
            Id = row.Id;
            Title = row.Title;
            Slug = row.Slug;
            Body = row.Body;
            Sorting = row.Sorting;
            HasSiteBar = row.HasSiteBar;
        }
        
        public int Id { get; set; }
        [Required(ErrorMessage="Nazwa strony jest wymagana")]        
        public string Title { get; set; }
        public string Slug { get; set; }
        [Required(ErrorMessage = "Treść strony jest wymagana")]
        public string Body { get; set; }
        public int Sorting { get; set; }
        public bool HasSiteBar { get; set; }
    }
}
