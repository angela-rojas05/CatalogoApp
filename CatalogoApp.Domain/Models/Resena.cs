namespace CatalogoApp.Domain.Models
{
    public class Resena
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public int Calificacion { get; set; } // 1-5
        public string Comentario { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}