using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ControlGastos.Data;
using ControlGastos.Models;
using Microsoft.Win32;

namespace ControlGastos.Controllers
{
    public class GastoController : Controller
    {
        public ActionResult Crear()
        {
            using (var context = new ControlGastosContext())
            {
                ViewBag.Fondos = context.FondoMonetario
                    .Select(f => new SelectListItem
                    {
                        Value = f.Id.ToString(),
                        Text = f.Nombre
                    }).ToList();

                ViewBag.TiposDocumento = context.TipoDocumento
                    .Select(t => new SelectListItem
                    {
                        Value = t.Id.ToString(),
                        Text = t.Nombre
                    }).ToList();

                ViewBag.TiposGasto = context.TipoGasto
                    .Select(tg => new SelectListItem
                    {
                        Value = tg.Id.ToString(),
                        Text = tg.Nombre
                    }).ToList(); ;


                ViewBag.GastosDetalle = new List<GastosDetalle>();
            }
            return View();
        }

        [HttpPost]
        public ActionResult Crear(Gasto registro, List<GastosDetalle> GastosDetalle)
        {
            var context = new ControlGastosContext();

            string usuarioNombre = (string)Session["Usuario"];
            var usuarioActual = context.Usuarios.FirstOrDefault(u => u.NombreUsuario == usuarioNombre);
            int userId = usuarioActual?.Id ?? 0;
            registro.IdCreador = userId;

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    if (GastosDetalle == null || !GastosDetalle.Any())
                    {
                        throw new Exception("Debe agregar al menos un gasto.");
                    }

                    var detallesAprobados = new List<GastosDetalle>();
                    var detallesRechazados = new List<string>();

                    foreach (var detalle in GastosDetalle)
                    {
                        detalle.IdCreador = userId;

                        if (ValidarSobregiro((int)detalle.IdTipoGasto, detalle.Monto))
                        {
                            var tipoGastoNombre = context.TipoGasto
                                .Where(t => t.Id == detalle.IdTipoGasto)
                                .Select(t => t.Nombre)
                                .FirstOrDefault();

                            detallesRechazados.Add($"Concepto: {tipoGastoNombre}, Sobregiro con monto: {detalle.Monto}");
                            continue;
                        }

                        detallesAprobados.Add(detalle);
                    }

                    if (!detallesAprobados.Any())
                    {
                        throw new Exception($"Operación cancelada. Gastos rechazados:\n{string.Join("\n", detallesRechazados)}");
                    }

                    context.Gasto.Add(registro);
                    context.SaveChanges();

                    foreach (var detalle in detallesAprobados)
                    {
                        detalle.IdGastos = registro.Id;
                        context.GastosDetalle.Add(detalle);
                    }

                    if (detallesRechazados.Any())
                    {
                        transaction.Rollback();
                        ModelState.AddModelError("", $"Operación cancelada. Gastos rechazados:\n{string.Join("\n", detallesRechazados)}");
                        ViewBag.GastosDetalle = GastosDetalle;
                        ViewBag.Fondos = context.FondoMonetario.Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.Nombre }).ToList();
                        ViewBag.TiposDocumento = context.TipoDocumento.Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Nombre }).ToList();
                        ViewBag.TiposGasto = context.TipoGasto.Select(tg => new SelectListItem { Value = tg.Id.ToString(), Text = tg.Nombre }).ToList();
                        return View(registro);
                    }
                    else
                    {
                        context.SaveChanges();
                        transaction.Commit();
                        TempData["Mensaje"] = "Gasto almacenado exitosamente.";
                        return RedirectToAction("Crear");
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ModelState.AddModelError("", $"Error: {ex.Message}");

                    ViewBag.Fondos = context.FondoMonetario.Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.Nombre }).ToList();
                    ViewBag.TiposDocumento = context.TipoDocumento.Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Nombre }).ToList();
                    ViewBag.TiposGasto = context.TipoGasto.Select(tg => new SelectListItem { Value = tg.Id.ToString(), Text = tg.Nombre }).ToList();

                    ViewBag.GastosDetalle = GastosDetalle ?? new List<GastosDetalle>();

                    return View(registro);
                }
            }
        }

        private bool ValidarSobregiro(int idTipoGasto, decimal montoNuevo)
        {
            var context = new ControlGastosContext();
            var montoGastado = context.GastosDetalle
                .Where(dg => dg.IdTipoGasto == idTipoGasto)
                .Sum(dg => (decimal?)dg.Monto) ?? 0;

            var presupuesto = context.Presupuestos
                .Where(p => p.IdTipoGasto == idTipoGasto)
                .Select(p => (decimal?)p.Monto)
                .FirstOrDefault() ?? 0;

            var total = montoGastado + montoNuevo;

            if (total > presupuesto)
            {
                return true;
            }

            //System.Diagnostics.Debug.WriteLine($"Gasto permitido: TipoGasto={idTipoGasto}, Presupuesto={presupuesto}, Total después de agregar={total}");
            return false;
        }


    }
}