using CatalogoApp.Application.Services;
using CatalogoApp.Domain.Models;
using CatalogoApp.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalogo.Controllers
{
    public class CatalogoController : Controller
    {
        /* CatalogoController
         * ==================
         * Gestiona el catálogo de videojuegos.
         * - Ver catálogo: cualquier usuario.
         * - Agregar videojuego: solo usuarios logueados.
         * * * * */

        private readonly ItemService _itemService;
        private readonly JsonResenaRepository _resenaRepo;

        public CatalogoController(ItemService itemService, JsonResenaRepository resenaRepo)
        {
            _itemService = itemService;
            _resenaRepo = resenaRepo;
        }

        public IActionResult Index(string? genero)
        {
            var todos = _itemService.ObtenerTodos();
            var resultado = string.IsNullOrEmpty(genero)
                ? todos
                : todos.Where(i => i.Genero == genero).ToList();

            ViewBag.Generos = _itemService.ObtenerGeneros();
            ViewBag.GeneroActual = genero;
            ViewBag.UsuarioLogueado = HttpContext.Session.GetString("UsuarioLogueado");
            return View(resultado);
        }

        public IActionResult Detalle(int id)
        {
            var item = _itemService.ObtenerPorId(id);
            if (item == null) return NotFound();

            ViewBag.Resenas = _resenaRepo.ObtenerPorItem(id);
            ViewBag.UsuarioLogueado = HttpContext.Session.GetString("UsuarioLogueado");
            return View(item);
        }

        // GET: Solo logueados
        public IActionResult Agregar()
        {
            if (HttpContext.Session.GetString("UsuarioLogueado") == null)
                return RedirectToAction("Login", "Usuario",
                    new { returnUrl = Url.Action("Agregar", "Catalogo") });
            return View();
        }

        // POST: Solo logueados
        [HttpPost]
        public IActionResult Agregar(Item item)
        {
            if (HttpContext.Session.GetString("UsuarioLogueado") == null)
                return RedirectToAction("Login", "Usuario");

            _itemService.Agregar(item);
            return RedirectToAction("Index");
        }
    }
}