using CmsShop.Areas.Admin.Data;
using CmsShop.Models.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmsShop.App_Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) {   }
        public DbSet<Page> Pages { get; set; }
        public DbSet<SidebarDTO> Sidebars { get; set; }
        public DbSet<CategoryDTO> Categories { get; set; }
    }
}
