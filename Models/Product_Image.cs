using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoProgramacionAvanzada.Models
{
    public class Product_Image
    {


    [Key]

    public int id { get; set; }
    public int id_Product { get; set; }

    public byte[] Image { get; set; }

    [ForeignKey("ProductId")]
     public Product Product { get; set; }



    }
}
