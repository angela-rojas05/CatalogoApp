using System.Text.Json;
using CatalogoApp.Domain.Models;

namespace CatalogoApp.Infrastructure.Repositories
{
    public class JsonUsuarioRepository
    {
        private readonly string _filePath;

        public JsonUsuarioRepository(string filePath)
        {
            _filePath = filePath;
            var carpeta = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(carpeta))
                Directory.CreateDirectory(carpeta);
        }

        public List<Usuario> ObtenerTodos()
        {
            if (!File.Exists(_filePath)) return new List<Usuario>();
            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<Usuario>>(json) ?? new List<Usuario>();
        }

        public Usuario? ObtenerPorNombre(string nombreUsuario)
        {
            return ObtenerTodos().FirstOrDefault(u =>
                u.NombreUsuario.Equals(nombreUsuario, StringComparison.OrdinalIgnoreCase));
        }

        public bool ExisteNombreUsuario(string nombreUsuario)
        {
            return ObtenerTodos().Any(u =>
                u.NombreUsuario.Equals(nombreUsuario, StringComparison.OrdinalIgnoreCase));
        }

        public void Registrar(Usuario usuario)
        {
            var usuarios = ObtenerTodos();
            usuario.Id = usuarios.Count > 0 ? usuarios.Max(u => u.Id) + 1 : 1;
            usuarios.Add(usuario);
            Guardar(usuarios);
        }

        private void Guardar(List<Usuario> usuarios)
        {
            var opciones = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(_filePath, JsonSerializer.Serialize(usuarios, opciones));
        }
    }
}