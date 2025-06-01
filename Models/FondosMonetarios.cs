namespace ControlGastos.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("FondosMonetarios")]
    public class FondosMonetarios
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        public string TipoNombre { get; set; }

        [Required]
        [StringLength(50)]
        public int Tipo { get; set; } 

        [Required]
        public int IdCreador { get; set; }

        public DateTime FechaCreacion { get; set; } 
    }
}