using System;
using System.Linq;
using System.Web.Mvc;
using ControlGastos.Data;
using ControlGastos.Models;

namespace ControlGastos.Controllers
{
    public class MovimientosController : Controller
    {
        private ControlGastosContext context = new ControlGastosContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Grafico()
        {
            return View();
        }

        [HttpGet]
        public ActionResult FiltrarPorFechas(DateTime FechaInicio, DateTime FechaFin)
        {
            var movimientos = context.Database.SqlQuery<Movimiento>(
             @"SELECT Fecha, Monto, 'Depósito' AS Tipo 
              FROM Deposito 
              WHERE Fecha BETWEEN @p0 AND @p1

              UNION ALL 

              SELECT g.Fecha, gd.Monto, 'Gasto' AS Tipo 
              FROM Gasto g 
              INNER JOIN GastosDetalle gd ON g.Id = gd.IdGastos 
              WHERE g.Fecha BETWEEN @p0 AND @p1",
             FechaInicio, FechaFin
         ).ToList();

            return View("Index", movimientos);
        }


        public ActionResult ObtenerDatosGrafico(DateTime FechaInicio, DateTime FechaFin)
        {
            var datos = context.Database.SqlQuery<GastoResumen>(
                @"SELECT tg.Nombre AS Tipo, 
                       COALESCE(SUM(p.Monto), 0) AS Presupuestado, 
                       COALESCE(SUM(gd.Monto), 0) AS Ejecutado 
                FROM TiposGasto tg
                LEFT JOIN Presupuesto p ON tg.Id = p.IdTipoGasto
                LEFT JOIN GastosDetalle gd ON tg.Id = gd.IdGastos
                LEFT JOIN Gasto g ON gd.IdGastos = g.Id
                WHERE (
                    (p.Anio IS NOT NULL AND p.Mes IS NOT NULL AND DATEFROMPARTS(p.Anio, p.Mes, 1) BETWEEN @p0 AND @p1)
                    OR (g.Fecha BETWEEN @p0 AND @p1)
                )
                GROUP BY tg.Nombre;",
                FechaInicio, FechaFin
            ).ToList();

            return Json(datos, JsonRequestBehavior.AllowGet);
        }
    }
}