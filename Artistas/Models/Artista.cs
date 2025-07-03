using System.ComponentModel.DataAnnotations;

namespace Artistas.Models
{
    public class Artista
    {
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; } = string.Empty;
        public string? Genero { get; set; }
        public DateOnly FechaNacimiento { get; set; }
        public string? Nacionalidad { get; set; }
        public int? CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }
        public int? UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public List<Espectaculo>? Espectaculos { get; set; } = new List<Espectaculo>();

        public Artista() { }
        public Artista(string nombre, string genero, DateOnly fechaNacimiento, string nacionalidad, int categoriaId, int usuarioId)
        {
            Nombre = nombre;
            Genero = genero;
            FechaNacimiento = fechaNacimiento;
            Nacionalidad = nacionalidad;
            CategoriaId = categoriaId;
            UsuarioId = usuarioId;
        }
    }
}
