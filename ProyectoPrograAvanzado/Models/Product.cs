using System.ComponentModel.DataAnnotations;

namespace ProyectoProgramacionAvanzada.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Details { get; set; }
        public ICollection<ProductImage> Images { get; set; }


    }
}

