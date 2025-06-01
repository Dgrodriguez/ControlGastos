using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ControlGastos.Data;
using ControlGastos.Models;

namespace ControlGastos.Controllers
{
    public class DepositoController : Controller
    {
        private ControlGastosContext context = new ControlGastosContext();
        public ActionResult Index()
        {
            var depositos = context.Deposito.ToList();
            var fondos = context.FondoMonetario.ToDictionary(f => f.Id, f => f.Nombre);

            var vistaDepositos = depositos.Select(d => new DepositoViewModel
            {
                Id = d.Id,
                Fecha = d.Fecha,
                FondoNombre = d.IdFondoMonetario.HasValue && fondos.ContainsKey(d.IdFondoMonetario.Value)
                              ? fondos[d.IdFondoMonetario.Value]
                              : "No Asignado",
                Monto = d.Monto
            }).ToList();

            return View(vistaDepositos);
        }

        // 🔹 Crear depósito (GET)
        public ActionResult Crear()
        {
            var fondos = context.FondoMonetario.ToList();
            ViewBag.Fondos = fondos;
            return View();
        }

        // 🔹 Crear depósito (POST)
        [HttpPost]
        public ActionResult Crear(Deposito deposito)
        {
            if (ModelState.IsValid)
            {
                string usuarioNombre = (string)Session["Usuario"];
                var usuarioActual = context.Usuarios.FirstOrDefault(u => u.NombreUsuario == usuarioNombre);
                int userId = usuarioActual?.Id ?? 0;
                deposito.IdCreador = userId;

                context.Deposito.Add(deposito);
                context.SaveChanges();
                TempData["Mensaje"] = "Depósito registrado exitosamente.";
                return RedirectToAction("Index");
            }

            ViewBag.Fondos = new SelectList(context.FondoMonetario, "Id", "Nombre");
            return View(deposito);
        }

        public ActionResult Editar(int id)
        {
            var deposito = context.Deposito.Find(id);


            if (deposito == null)
                return HttpNotFound();


            ViewBag.Fondos = context.FondoMonetario.ToList();

            return View(deposito);
        }

        [HttpPost]
        public ActionResult Editar(Deposito deposito)
        {
            if (ModelState.IsValid)
            {
                context.Entry(deposito).State = EntityState.Modified;
                context.SaveChanges();
                TempData["Mensaje"] = "Depósito actualizado correctamente.";
                return RedirectToAction("Index");
            }

            ViewBag.Fondos = new SelectList(context.FondoMonetario, "Id", "Nombre", deposito.IdFondoMonetario);
            return View(deposito);
        }

        // 🔹 Eliminar depósito (POST)
        [HttpGet]
        public ActionResult Eliminar(int id)
        {
            var deposito = context.Deposito.Find(id);
            if (deposito == null)
                return HttpNotFound();

            context.Deposito.Remove(deposito);
            context.SaveChanges();
            TempData["Mensaje"] = "Depósito eliminado correctamente.";
            return RedirectToAction("Index");
        }
    }
}