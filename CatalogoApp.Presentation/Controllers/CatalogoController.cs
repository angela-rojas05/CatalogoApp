using CatalogoApp.Domain.Models;
using Microsoft.AspNetCore.Mvc;



namespace Catalogo.Controllers
{
    public class CatalogoController : Controller
    {
        /* CatalogoController
         * ==================
         * 
         * Controlador encargado de gestionar el flujo de datos
         * del catálogo de videojuegos.
         * 
         * Su función es recibir las peticiones del usuario,
         * consultar la lista de items y devolver la vista
         * correspondiente.
         * * * * */



        private static List<Item> _items = new()
 {
 new Item {
 Id = 1,
 Titulo = "Devil May Cry",
 Genero = "Hack and Slash",
 Ano = 2001,
 Consola = "PlayStation 2",
 Descripcion = "Videojuego que trata de un cazador mitad humano mitad demonio que debe evitar el regreso del rey del infierno."
 },
 new Item {
 Id = 2,
 Titulo = "Castlevania: Symphony of the Night",
 Genero = "Metroidvania",
 Ano = 1997,
 Consola = "PlayStation 2",
 Descripcion = "Videojuego que trata de un cazador vampiro que debe detener a su padre, el conde Drácula."
 },
 new Item {
 Id = 3,
 Titulo = "NieR: Automata",
 Genero = "Acción-RPG",
 Ano = 2017,
 Consola = "PlayStation 4",
 Descripcion = "Videojuego que trata de unos androides de batalla que deben detener a las máquinas alienígenas."
 }
 };
        public IActionResult Index(string? genero)
        {
            var resultado = string.IsNullOrEmpty(genero)
            ? _items
            : _items.Where(i => i.Genero == genero).ToList();



            ViewBag.Generos = _items.Select(i => i.Genero).Distinct().ToList();
            ViewBag.GeneroActual = genero;

            return View(resultado);
        }



        public IActionResult Detalle(int id)
        {
            var item = _items.FirstOrDefault(i => i.Id == id);
            return item == null ? NotFound() : View(item);
        }



        public IActionResult Agregar()
        {
            return View();
        }



        [HttpPost]
        public IActionResult Agregar(Item item)
        {
            item.Id = _items.Count + 1;
            _items.Add(item);
            return RedirectToAction("Index");
        }
    }
}