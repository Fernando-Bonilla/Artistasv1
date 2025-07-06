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
        public ActionResult<RespuestaEspectaculoDTO> GetEspectaculo(int id)
        {
            if (id <= 0)
            {
                return BadRequest("El Id no puede ser menor o igual a cero");
            }

            Espectaculo? espectaculo = _context.Espectaculos
                .Include(e => e.Artista)
                .FirstOrDefault(e => e.Id == id);

            if (espectaculo == null)
            {
                return NotFound($"Espectaculo con id: {id} no existe");
            }

            // Ahora siguiendo solo mostramos los datos que queremos a travez del DTO
            RespuestaEspectaculoDTO espectaculoResp = new RespuestaEspectaculoDTO();
            espectaculoResp.Id = espectaculo.Id;
            espectaculoResp.Titulo = espectaculo.Titulo ?? "";
            espectaculoResp.Fecha = espectaculo.Fecha;
            espectaculoResp.Horario = espectaculo.Horario;
            espectaculoResp.ArtistaId = espectaculo.ArtistaId ?? 0;
            espectaculoResp.NombreArtista = espectaculo.Artista?.Nombre ?? "";

            return Ok(espectaculoResp);
        }

        [HttpPost]
        public ActionResult<RespuestaEspectaculoDTO> PostEspectaculo([FromBody] EspectaculoDTO espectaculo)
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

            // Creamos el objeto espectaculo usando los datos que pedimos con el EspectaculoDTO
            Espectaculo nuevoEspectaculo = new Espectaculo { };
            nuevoEspectaculo.Titulo = espectaculo.Titulo;
            nuevoEspectaculo.Fecha = DateOnly.Parse(espectaculo.Fecha);
            nuevoEspectaculo.Horario = TimeOnly.Parse(espectaculo.Horario);
            nuevoEspectaculo.ArtistaId = espectaculo.ArtistaId;            

            _context.Espectaculos.Add(nuevoEspectaculo);
            _context.SaveChanges();

            // Y mostramos el resultado usando el DTO de respuesta
            RespuestaEspectaculoDTO espectaculoResp = new RespuestaEspectaculoDTO();
            espectaculoResp.Id = nuevoEspectaculo.Id;
            espectaculoResp.Titulo = nuevoEspectaculo.Titulo ?? "";
            espectaculoResp.Fecha = nuevoEspectaculo.Fecha;
            espectaculoResp.Horario = nuevoEspectaculo.Horario;
            espectaculoResp.ArtistaId = nuevoEspectaculo.ArtistaId ?? 0;
            espectaculoResp.NombreArtista = artista.Nombre ?? "";

            return Ok(espectaculoResp);
            
        }

        [HttpPut("{id}")]
        public ActionResult<RespuestaEspectaculoDTO> PutEspectaculo(int id, [FromBody] EspectaculoDTO parametrosEspectaculo)
        {
            if(parametrosEspectaculo == null)
            {
                return BadRequest("El body del request estaba vacio");
            }

            Espectaculo? espectaculo = _context.Espectaculos.FirstOrDefault(e => e.Id == id);
            if (espectaculo == null)
            {
                return NotFound($"No existe espectaculo con id: {id}");
            }

            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            // Chequeamos que si el capo deja algunos campos vacios al modificar, toma por defecto los que ya tiene
            espectaculo.Titulo = string.IsNullOrEmpty(parametrosEspectaculo.Titulo) ? espectaculo.Titulo : parametrosEspectaculo.Titulo;
            espectaculo.Fecha = string.IsNullOrEmpty(parametrosEspectaculo.Fecha) ? espectaculo.Fecha : DateOnly.Parse(parametrosEspectaculo.Fecha);
            espectaculo.Horario = string.IsNullOrEmpty(parametrosEspectaculo.Horario) ? espectaculo.Horario : TimeOnly.Parse(parametrosEspectaculo.Horario);
            espectaculo.ArtistaId = (parametrosEspectaculo?.ArtistaId == null || parametrosEspectaculo?.ArtistaId == 0) ? espectaculo.ArtistaId : parametrosEspectaculo.ArtistaId;

            // Armamos el DTO de respuesta
            RespuestaEspectaculoDTO espectaculoResp = new RespuestaEspectaculoDTO();
            espectaculoResp.Id = espectaculo.Id;
            espectaculoResp.Titulo = espectaculo.Titulo ?? "";
            espectaculoResp.Fecha = espectaculo.Fecha;
            espectaculoResp.Horario = espectaculo.Horario;
            espectaculoResp.ArtistaId = espectaculo.ArtistaId ?? 0;
            espectaculoResp.NombreArtista = espectaculo?.Artista?.Nombre ?? "";

            try
            {
                //_context.Espectaculos.Update(espectaculo); 
                _context.SaveChanges();
                return Ok(espectaculoResp);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
