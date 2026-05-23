using System.Text.Json;
using CatalogoApp.Domain.Models;

namespace CatalogoApp.Infrastructure.Repositories
{
    public class JsonResenaRepository
    {
        private readonly string _filePath;

        public JsonResenaRepository(string filePath)
        {
            _filePath = filePath;
            var carpeta = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(carpeta))
                Directory.CreateDirectory(carpeta);
        }

        public List<Resena> ObtenerTodas()
        {
            if (!File.Exists(_filePath)) return new List<Resena>();
            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<Resena>>(json) ?? new List<Resena>();
        }

        public List<Resena> ObtenerPorItem(int itemId)
        {
            return ObtenerTodas().Where(r => r.ItemId == itemId).ToList();
        }

        public void Agregar(Resena resena)
        {
            var resenas = ObtenerTodas();
            resena.Id = resenas.Count > 0 ? resenas.Max(r => r.Id) + 1 : 1;
            resena.FechaCreacion = DateTime.Now;
            resenas.Add(resena);
            Guardar(resenas);
        }

        private void Guardar(List<Resena> resenas)
        {
            var opciones = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(_filePath, JsonSerializer.Serialize(resenas, opciones));
        }
    }
}