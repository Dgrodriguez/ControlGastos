namespace ControlGastos.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Gastos")]
    public class Gastos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public string Observacion { get; set; }

        [Required]
        public string Comercio { get; set; }

        [Required]
        [ForeignKey("TipoDocumentoGasto")]
        public int IdTipoDocumento { get; set; }

        [Required]
        [ForeignKey("FondosMonetarios")]
        public int IdFondoMonetario { get; set; }

        [Required]
        public int IdCreador { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        // Relación con TipoDocumentoGasto
        public virtual TipoDocumentoGasto TipoDocumentoGasto { get; set; }

        // Relación con FondosMonetarios
        public virtual FondosMonetarios FondosMonetarios { get; set; }
    }
}