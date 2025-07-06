using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Artistas.Data;
using Artistas.Models;
using Artistas.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Artistas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EspectaculosController : Controller
    {
        private readonly AppDbContext _context;

        public EspectaculosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<RespuestaEspectaculoDTO>> GetEspectaculos()
        {
            List<Espectaculo> espectaculos = _context.Espectaculos
                .Include(e => e.Artista)
                .ToList();

            List<RespuestaEspectaculoDTO> espectaculosRespuesta = new List<RespuestaEspectaculoDTO>();

            foreach(Espectaculo espectaculo in espectaculos)
            {
                RespuestaEspectaculoDTO espectaculoResp = new RespuestaEspectaculoDTO();
                espectaculoResp.Id = espectaculo.Id;
                espectaculoResp.Titulo = espectaculo.Titulo ?? "";
                espectaculoResp.Fecha = espectaculo.Fecha;
                espectaculoResp.Horario = espectaculo.Horario;
                espectaculoResp.ArtistaId = espectaculo.ArtistaId ?? 0;
                espectaculoResp.NombreArtista = espectaculo.Artista?.Nombre ?? "";

                espectaculosRespuesta.Add(espectaculoResp);
            }

            return espectaculosRespuesta;
        }

        [HttpGet("{id}")]
        public ActionResult<Espectaculo> GetEspectaculo(int id)
        {
            if (id <= 0)
            {
                return BadRequest("El Id no puede ser menor o igual a cero");
            }

            Espectaculo? espectaculo = _context.Espectaculos.FirstOrDefault(e => e.Id == id);

            if (espectaculo == null)
            {
                return NotFound($"Espectaculo con id: {id} no existe");
            }

            return Ok(espectaculo);
        }

        [HttpPost]
        public ActionResult<Espectaculo> PostEspectaculo([FromBody] Espectaculo espectaculo)
        {
            // Validaciones de usuario por ahora nada de eso

            if (espectaculo == null)
            {
                return BadRequest("El body del request no puede estar vacio");
            }

            if(espectaculo.ArtistaId <= 0)
            {
                return BadRequest("El id del Artista no puede ser igual o menor a cero");
            }

            Artista? artista = _context.Artistas.FirstOrDefault(a => a.Id == espectaculo.ArtistaId);

            if (artista == null)
            {
                return BadRequest($"No existe artista con id: {espectaculo.ArtistaId}");
            }

            Espectaculo nuevoEspectaculo = new Espectaculo { };
            nuevoEspectaculo.Titulo = espectaculo.Titulo;
            nuevoEspectaculo.Fecha = espectaculo.Fecha;
            nuevoEspectaculo.Horario = espectaculo.Horario;
            nuevoEspectaculo.ArtistaId = espectaculo.ArtistaId;

            _context.Espectaculos.Add(nuevoEspectaculo);
            _context.SaveChanges();

            return Ok(nuevoEspectaculo);
            
        }
    }
}
