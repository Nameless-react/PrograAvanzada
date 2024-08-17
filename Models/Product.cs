using System.ComponentModel.DataAnnotations;

namespace ProyectoProgramacionAvanzada.Models
{
    public class Product
    {
        [Key]

        public int id { get; set; }
        public string name { get; set; }
        public decimal price { get; set; }
        public string details { get; set; }
       
    }
}

