namespace ControlGastos.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using ControlGastos.Models;

    [Table("GastosDetalles")]
    public class GastosDetalles
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("TiposGasto")]
        public int IdTipoGasto { get; set; }

        [Required]
        [ForeignKey("Gastos")]
        public int IdGastos { get; set; }

        [Required]
        public float Monto { get; set; }

        [Required]
        public int IdCreador { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        // Relación con TiposGasto
        public virtual TiposGasto TiposGasto { get; set; }

        // Relación con Gastos
        public virtual Gastos Gastos { get; set; }
    }
}