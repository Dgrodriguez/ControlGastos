namespace ControlGastos.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using ControlGastos.Models;

    public class Presupuesto
    {
        public int NumeroPos { get; set; }
        public int Id { get; set; }
        public string Mes { get; set; }
        public int Anio { get; set; }
        public string TipoGasto { get; set; }
        public decimal Monto { get; set; }
        public string Creador { get; set; }
        public DateTime FechaCreacion { get; set; } 

        public int? IdTipoGasto { get; set; }
        public int? IdMes { get; set; }

    }
}