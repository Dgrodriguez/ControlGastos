using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ControlGastos.Models
{
    public class Movimiento
    {
        public DateTime Fecha { get; set; }
        public decimal Monto { get; set; }
        public string Tipo { get; set; } // "Depósito" o "Gasto"
    }
}