namespace Artistas.Models.DTOs
{
    public class RespuestaEspectaculoDTO
    {
        public int Id { get; set; } = 0;
        public string Titulo { get; set; } = string.Empty;
        public DateOnly Fecha { get; set; }
        public TimeOnly Horario { get; set; }
        public int ArtistaId { get; set; }
        public string NombreArtista { get; set; } = string.Empty;
    }
}
