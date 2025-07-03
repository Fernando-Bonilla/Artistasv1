namespace Artistas.Models
{
    public class Espectaculo
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public DateOnly Fecha { get; set; }
        public TimeOnly Horario { get; set; }

        public int? ArtistaId { get; set; }
        public Artista? Artista { get; set; }


        public Espectaculo() { }

        public Espectaculo(string titulo, DateOnly fecha, TimeOnly horario, int id)
        {
            Titulo = titulo;
            Fecha = fecha;
            Horario = horario;
            ArtistaId = id;
        }
    }
}
