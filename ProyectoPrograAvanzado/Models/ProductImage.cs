using System.ComponentModel.DataAnnotations;

namespace ProyectoProgramacionAvanzada.Models
{
    public class ProductImage
    {
        [Key]

        public int Id { get; set; }
        public int ProductId { get; set; }

        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public Product product { get; set; }
    }
}
