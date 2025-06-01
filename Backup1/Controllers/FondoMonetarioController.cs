using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ControlGastos.Data;
using ControlGastos.Models;


namespace ControlGastos.Controllers
{
    public class FondoMonetarioController : Controller
    {
        // GET: FondoMonetario
        public ActionResult Index()
        {
            using (var context = new ControlGastosContext())
            {
                var fondos = context.Database.SqlQuery<Models.FondosMonetarios>(
                    @"SELECT fm.Id, fm.Nombre, fm.Tipo ,tf.Nombre AS TipoNombre , fm.IdCreador , fm.FechaCreacion
                      FROM FondosMonetario fm
                      LEFT JOIN TipoFondosMonetario tf ON fm.Tipo = tf.Id"
                ).ToList();
                return View(fondos);
            }
        }

        public ActionResult Crear()
        {
            using (var context = new ControlGastosContext())
            {
                ViewBag.TipoFondosMonetarios = context.TipoFondoMonetario.ToList();
                return View();
            }
        }

        [HttpPost]
        public ActionResult Crear(ControlGastos.FondosMonetario FondoMonetario)
        {
            using (var context = new ControlGastosContext())
            {
                if (ModelState.IsValid)
                {
                    string usuarioNombre = (string)Session["Usuario"];
                    var usuarioActual = context.Usuarios.FirstOrDefault(u => u.NombreUsuario == usuarioNombre);
                    int userId = usuarioActual?.Id ?? 0;
                    FondoMonetario.IdCreador = userId;
                    context.FondoMonetario.Add(FondoMonetario);
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(FondoMonetario);
            }
        }

        public ActionResult Eliminar(int id)
        {
            using (var context = new ControlGastosContext())
            {
                var fondo = context.FondoMonetario.Find(id);
                if (fondo != null)
                {
                    context.FondoMonetario.Remove(fondo);
                    context.SaveChanges();
                }
                return RedirectToAction("Index");
            }
        }

        public ActionResult Editar(int id)
        {
            using (var context = new ControlGastosContext())
            {
                var fondo = context.FondoMonetario.Find(id);
                if (fondo == null)
                {
                    return HttpNotFound();
                }
                ViewBag.TipoFondosMonetarios = context.TipoFondoMonetario.ToList();
                return View(fondo);
            }
        }

        [HttpPost]
        public ActionResult GuardarEditar(ControlGastos.FondosMonetario FondosMonetario)
        {
            using (var context = new ControlGastosContext())
            {
                var fondoExistente = context.FondoMonetario.Find(FondosMonetario.Id);
                if (fondoExistente != null)
                {
                    fondoExistente.Tipo = FondosMonetario.Tipo;
                    fondoExistente.Nombre = FondosMonetario.Nombre;

                    // 🔹 Verifica que los valores sean correctos antes de guardarlos
                    Console.WriteLine("Tipo: " + fondoExistente.Tipo);
                    Console.WriteLine("Nombre: " + fondoExistente.Nombre);

                    context.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            return View();
        }


    }
}