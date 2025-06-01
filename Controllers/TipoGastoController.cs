using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ControlGastos.Data;
using ControlGastos;


namespace ControlGastos.Controllers
{
    public class TipoGastoController : Controller
    {
        //private readonly ControlGastosContext db = new ControlGastosContext();
        // GET: TipoGasto
        public ActionResult Index()
        {
            using (var context = new ControlGastosContext())
            {
                var tipos = context.TipoGasto.ToList();
                return View(tipos);
            }
        }

        [HttpPost]
        public ActionResult Crear(ControlGastos.TiposGasto tipoGasto)
        {
            using (var context = new ControlGastosContext())
            {
                if (ModelState.IsValid)
                {
                    string usuarioNombre = (string)Session["Usuario"];
                    var usuarioActual = context.Usuarios.FirstOrDefault(u => u.NombreUsuario == usuarioNombre);
                    int userId = usuarioActual?.Id ?? 0;
                    var ultimoCodigo = context.TipoGasto.OrderByDescending(g => g.Codigo).Select(g => g.Codigo).FirstOrDefault();
                    if (ultimoCodigo is null || ultimoCodigo.Length < 5) { ultimoCodigo = "GAS-0000"; }
                    int numero = int.Parse(ultimoCodigo.Substring(4)) + 1;
                    tipoGasto.Codigo = $"GAS-{numero:D4}";
                    tipoGasto.IdCreador = userId;
                    context.TipoGasto.Add(tipoGasto);
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(tipoGasto);
            }
        }

        public ActionResult Eliminar(int id)
        {
            using (var context = new ControlGastosContext())
            {
                var tipoGasto = context.TipoGasto.Find(id);
                if (tipoGasto != null)
                {
                    context.TipoGasto.Remove(tipoGasto);
                    context.SaveChanges();
                }
                return RedirectToAction("Index");
            }
        }

        public ActionResult Editar(int id)
        {
            using (var context = new ControlGastosContext())
            {
                var tipoGasto = context.TipoGasto.Find(id);
                if (tipoGasto == null)
                {
                    return HttpNotFound();
                }
                return View(tipoGasto);
            }
        }


        [HttpPost]
        public ActionResult Editar(TiposGasto tipoGasto)
        {
            using (var context = new ControlGastosContext())
            {
                var tipoExistente = context.TipoGasto.Find(tipoGasto.Id);
                if (tipoExistente != null)
                {
                    tipoExistente.Nombre = tipoGasto.Nombre;
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