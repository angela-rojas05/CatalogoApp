using CatalogoApp.Domain.Models;
using CatalogoApp.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace CatalogoApp.Presentation.Controllers
{
    public class UsuarioController : Controller
    {
        /* UsuarioController
         * =================
         * Gestiona el registro y login de usuarios.
         * Usa sesión HTTP para mantener el estado de login.
         * Las contraseñas se hashean con SHA256 antes de guardar.
         * * * * */

        private readonly JsonUsuarioRepository _repo;

        public UsuarioController(JsonUsuarioRepository repo)
        {
            _repo = repo;
        }

        // GET: /Usuario/Login
        public IActionResult Login(string? returnUrl)
        {
            if (HttpContext.Session.GetString("UsuarioLogueado") != null)
                return RedirectToAction("Index", "Catalogo");

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: /Usuario/Login
        [HttpPost]
        public IActionResult Login(string nombreUsuario, string password, string? returnUrl)
        {
            var hash = HashPassword(password);
            var usuario = _repo.ObtenerPorNombre(nombreUsuario);

            if (usuario == null || usuario.PasswordHash != hash)
            {
                ViewBag.Error = "Usuario o contraseña incorrectos.";
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }

            HttpContext.Session.SetString("UsuarioLogueado", usuario.NombreUsuario);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Catalogo");
        }

        // GET: /Usuario/Registro
        public IActionResult Registro()
        {
            if (HttpContext.Session.GetString("UsuarioLogueado") != null)
                return RedirectToAction("Index", "Catalogo");
            return View();
        }

        // POST: /Usuario/Registro
        [HttpPost]
        public IActionResult Registro(string nombreUsuario, string email, string password, string confirmar)
        {
            if (password != confirmar)
            {
                ViewBag.Error = "Las contraseñas no coinciden.";
                return View();
            }

            if (_repo.ExisteNombreUsuario(nombreUsuario))
            {
                ViewBag.Error = "Ese nombre de usuario ya existe.";
                return View();
            }

            var usuario = new Usuario
            {
                NombreUsuario = nombreUsuario,
                Email = email,
                PasswordHash = HashPassword(password)
            };
            _repo.Registrar(usuario);

            HttpContext.Session.SetString("UsuarioLogueado", usuario.NombreUsuario);
            return RedirectToAction("Index", "Catalogo");
        }

        // GET: /Usuario/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UsuarioLogueado");
            return RedirectToAction("Index", "Catalogo");
        }

        private static string HashPassword(string password)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            return Convert.ToHexString(bytes).ToLower();
        }
    }
}