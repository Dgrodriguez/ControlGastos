using System;
using System.Linq;
using System.Web.Mvc;
using System.Dynamic;
using ControlGastos.Data;
using ControlGastos.Models;
public class AccountController : Controller
{
    [HttpGet]
    public ActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Login(string txtUsuario, string txtClave)
    {
        if (ValidarLogin(txtUsuario, txtClave))
        {
            Session["Usuario"] = txtUsuario;
            return RedirectToAction("Index", "Home");
        }
        else
        {
            ViewBag.Error = "Usuario o contraseña incorrectos.";
            return View();
        }
    }

    private bool ValidarLogin(string usuario, string clave)
    {
        using (var context = new ControlGastosContext())
        {
            string claveEncriptada = Seguridad.EncriptarMD5(clave);
            return context.Usuarios.Any(u => u.NombreUsuario == usuario && u.Clave == claveEncriptada);
        }
    }
}