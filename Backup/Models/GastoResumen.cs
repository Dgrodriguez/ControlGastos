using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ControlGastos.Models
{
    public class GastoResumen
    {
        public string Tipo { get; set; } // Tipo de gasto
        public decimal Presupuestado { get; set; } // Monto presupuestado
        public decimal Ejecutado { get; set; } // Monto ejecutado
    }
}