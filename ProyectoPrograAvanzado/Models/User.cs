using System.ComponentModel.DataAnnotations;

namespace ProyectoProgramacionAvanzada.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(255)]
        public string Password { get; set; }

        [Required]
        [StringLength(255)]
        public string Email { get; set; }

        [Required]
        public bool Status { get; set; }

        [Required]
        [StringLength(20)]
        public string Phone { get; set; }

        [Required]
        [StringLength(15)]
        public string Identification { get; set; }

        [Required]
        [StringLength(255)]
        public string Role { get; set; }
    }
}
