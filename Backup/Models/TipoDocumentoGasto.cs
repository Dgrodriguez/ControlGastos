namespace ControlGastos.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TipoDocumentoGasto")]
    public class TipoDocumentoGasto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } // Comprobante, Factura, Otro

        [Required]
        public int IdCreador { get; set; } // Usuario que creó el registro

        [Required]
        public DateTime FechaCreacion { get; set; } = DateTime.Now; // Fecha automática de creación
    }
}