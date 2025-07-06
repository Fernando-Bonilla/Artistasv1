using Artistas.Models;

namespace Artistas.Models.DTOs
{
    public class EspectaculoDTO
    {
        public string Titulo { get; set; } = string.Empty;
        public string? Fecha {  get; set; }
        public string? Horario { get; set; }
        public int? ArtistaId { get; set; }
    }
}


