using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ControlGastos.Data;
using ControlGastos.Models;
using DevExpress.Web.Internal.XmlProcessor;
using DevExpress.XtraScheduler.Outlook.Interop;

namespace ControlGastos.Controllers
{
    public class PresupuestoController : Controller
    {
        public ActionResult Index()
        {
            using (var context = new ControlGastosContext())
            {
                var Presupuesto = context.Database.SqlQuery<Models.Presupuesto>(
                    @"SET LANGUAGE Spanish;
                    SELECT 
                        CAST(ROW_NUMBER() OVER(ORDER BY a.Id ASC) AS INT) AS NumeroPos, 
                        a.Id,
                        DATENAME(MONTH, DATEFROMPARTS(2025, a.Mes, 1)) AS Mes,
                        a.Anio,
                        b.Nombre as TipoGasto,
                        a.Monto, 
                        c.NombreUsuario as Creador,
                        a.FechaCreacion
                    FROM Presupuesto a
                    JOIN TiposGasto b ON a.IdTipoGasto = b.Id
                    JOIN Usuario c ON c.Id = a.IdCreador;").ToList();
                ViewBag.TipoFondosMonetarios = context.TipoFondoMonetario.ToList();
                return View(Presupuesto);
            }
        }

        [HttpGet]
        public ActionResult Crear()
        {
            using (var context = new ControlGastosContext())
            {
                ViewBag.TipoGasto = context.TipoGasto.ToList();
                return View();
            }
        }
        [HttpPost]
        public ActionResult Crear(Models.Presupuesto Presup)
        {
            ControlGastosContext context = new ControlGastosContext();
            Presupuesto nuevoPresupuesto = new Presupuesto();
                if (ModelState.IsValid)
                {
                    string usuarioNombre = (string)Session["Usuario"];
                    var usuarioActual = context.Usuarios.FirstOrDefault(u => u.NombreUsuario == usuarioNombre);
                    int userId = usuarioActual?.Id ?? 0;
                    nuevoPresupuesto = new Presupuesto
                    {
                        Mes = (int)Presup.IdMes,
                        Anio = Presup.Anio,
                        IdTipoGasto = Presup.IdTipoGasto,
                        Monto = Presup.Monto,
                        IdCreador = userId
                    };
                    context.Presupuestos.Add(nuevoPresupuesto);
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.TipoGasto = context.TipoGasto.ToList();
                return View(nuevoPresupuesto);
        }


        public ActionResult Eliminar(int id)
        {
            using (var context = new ControlGastosContext())
            {
                var fondo = context.Presupuestos.Find(id);
                if (fondo != null)
                {
                    context.Presupuestos.Remove(fondo);
                    context.SaveChanges();
                }
                return RedirectToAction("Index");
            }
        }
    }
}