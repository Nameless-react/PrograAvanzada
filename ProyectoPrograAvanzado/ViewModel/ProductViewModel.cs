
using System.ComponentModel.DataAnnotations;

namespace ProyectoProgramacionAvanzada.ViewModel
{
    public class ProductViewModel
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter the name of the product")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "The product name must be between 2 and 100 characters long.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter the price of the product")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        [StringLength(500, ErrorMessage = "Details should not exceed 500 characters.")]
        public string Details { get; set; }


        [Display(Name = "Choose the gallery images of your product")]
        [Required(ErrorMessage = "Please choose at least one gallery image.")]
        public IFormFileCollection Images { get; set; }
       
        public List<ProductImageViewModel> productImages { get; set; }
    }
}
