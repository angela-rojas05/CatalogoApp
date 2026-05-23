using CatalogoApp.Domain.Models;
using CatalogoApp.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CatalogoApp.Presentation.Controllers
{
    public class ResenaController : Controller
    {
        /* ResenaController
         * ================
         * Gestiona las reseñas de videojuegos.
         * RESTRICCIÓN: Solo usuarios logueados pueden crear reseñas.
         * Si no hay sesión activa, redirige al login.
         * * * * */

        private readonly JsonResenaRepository _resenaRepo;

        public ResenaController(JsonResenaRepository resenaRepo)
        {
            _resenaRepo = resenaRepo;
        }

        // POST: /Resena/Agregar
        [HttpPost]
        public IActionResult Agregar(int itemId, int calificacion, string comentario)
        {
            // RESTRICCIÓN: Verificar que el usuario esté logueado
            var usuarioLogueado = HttpContext.Session.GetString("UsuarioLogueado");
            if (string.IsNullOrEmpty(usuarioLogueado))
            {
                // Redirigir al login con returnUrl al detalle del juego
                return RedirectToAction("Login", "Usuario",
                    new { returnUrl = Url.Action("Detalle", "Catalogo", new { id = itemId }) });
            }

            if (string.IsNullOrWhiteSpace(comentario) || calificacion < 1 || calificacion > 5)
            {
                TempData["ErrorResena"] = "La reseña debe tener un comentario y calificación entre 1 y 5.";
                return RedirectToAction("Detalle", "Catalogo", new { id = itemId });
            }

            var resena = new Resena
            {
                ItemId = itemId,
                NombreUsuario = usuarioLogueado,
                Calificacion = calificacion,
                Comentario = comentario
            };
            _resenaRepo.Agregar(resena);

            TempData["ExitoResena"] = "¡Reseña agregada correctamente!";
            return RedirectToAction("Detalle", "Catalogo", new { id = itemId });
        }
    }
}