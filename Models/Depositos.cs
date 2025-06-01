namespace ControlGastos.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Depositos")]
    public class DepositoViewModel
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string FondoNombre { get; set; }  
        public decimal Monto { get; set; }
    }
}