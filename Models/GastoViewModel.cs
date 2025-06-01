using ControlGastos;
using System;
using System.Collections.Generic;

namespace ControlGastos.Models
{
    public class GastoViewModel
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Observacion { get; set; }
        public string Comercio { get; set; }
        public string TipoDocumento { get; set; }
        public string FondoMonetario { get; set; }
        public List<GastosDetalle> GastosDetalles { get; set; }
    }
}