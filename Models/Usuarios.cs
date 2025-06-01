namespace ControlGastos.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Usuarios")]
    public class Usuarios
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Usuario { get; set; }

        [StringLength(250)]
        public string Clave { get; set; } // Puede ser nulo según la definición en la BD

        [Required]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}