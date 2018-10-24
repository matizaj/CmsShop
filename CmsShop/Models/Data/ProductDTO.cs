using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CmsShop.Models.Data
{
    public class ProductDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int  Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
        public string ImageName { get; set; }

        [ForeignKey("CategoryId")]
        public virtual CategoryDTO Category { get; set; }
    }
}
