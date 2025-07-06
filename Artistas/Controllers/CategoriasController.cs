using Artistas.Models;
using Artistas.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Artistas.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Artistas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : Controller
    {
        private readonly AppDbContext _context;

        public CategoriasController(AppDbContext context )
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<RespuestaCategoriaDTO>> GetCategorias()
        {
            List<Categoria> categorias = _context.Categorias.ToList();
            List<RespuestaCategoriaDTO> categoriasRespuesta = new List<RespuestaCategoriaDTO>();

            foreach (Categoria categoria in categorias)
            {
                RespuestaCategoriaDTO resp = new RespuestaCategoriaDTO();
                resp.Id = categoria.Id;
                resp.Nombre = categoria.Nombre ?? "";
                resp.Descripcion = categoria.Descripcion ?? "";

                categoriasRespuesta.Add(resp);
            }

            if(!categoriasRespuesta.Any())
            {
                return NotFound("No existen categorias");
            }

            return categoriasRespuesta;
        }

        [HttpGet("{id}")]
        public ActionResult<RespuestaCategoriaDTO> GetCategoria(int id) 
        {
            if(id <= 0)
            {
                return BadRequest("El Id no puede ser menor o igual a cero");
            }

            Categoria? categoria = _context.Categorias.FirstOrDefault(c => c.Id == id);
            if(categoria == null)
            {
                return NotFound($"No se encontraron categorias con id: {id}");
            }

            // Armamos el DTO de respuesta para no mostrarle al usuario los maximos secretos
            RespuestaCategoriaDTO categoriaResp = new RespuestaCategoriaDTO();
            categoriaResp.Id = categoria.Id;
            categoriaResp.Nombre = categoria.Nombre ?? "";
            categoriaResp.Descripcion = categoria.Descripcion ?? "";

            return Ok(categoriaResp);

        }

        [HttpPost]
        public ActionResult<RespuestaCategoriaDTO> PostCategoria([FromBody] CategoriaDTO categoriaParametros)
        {
            if (categoriaParametros == null)
            {
                return BadRequest("El body del request no puede estar vacio");
            }

            Categoria nuevaCategoria = new Categoria();
            nuevaCategoria.Nombre = categoriaParametros.Nombre;
            nuevaCategoria.Descripcion = categoriaParametros.Descripcion;

            _context.Categorias.Add(nuevaCategoria);

            try
            {
                _context.SaveChanges();
                RespuestaCategoriaDTO resp = new RespuestaCategoriaDTO();
                resp.Id = nuevaCategoria.Id;
                resp.Nombre = nuevaCategoria.Nombre;
                resp.Descripcion = nuevaCategoria.Descripcion;

                return Ok(resp);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public ActionResult<RespuestaCategoriaDTO> PutCategoria(int id, [FromBody] CategoriaDTO parametrosCategoria)
        {
            if(id <= 0)
            {
                return BadRequest("El Id no puede ser menor o igual a cero");
            }

            if(parametrosCategoria == null)
            {
                return BadRequest("El body del request estaba vacio");
            }

            Categoria? categoria = _context.Categorias.FirstOrDefault(c => c.Id == id);
            if (categoria == null)
            {
                return NotFound($"No existe espectaculo con id: {id}");
            }

            if(ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            categoria.Nombre = parametrosCategoria.Nombre;
            categoria.Descripcion = parametrosCategoria?.Descripcion ?? "";

            // Armamos el DTO del tipo Respuesta
            RespuestaCategoriaDTO resp = new RespuestaCategoriaDTO();
            resp.Id = categoria.Id;
            resp.Nombre = categoria.Nombre;
            resp.Descripcion = categoria.Descripcion;

            try
            {
                _context.SaveChanges();
                return Ok(resp);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpDelete("{id}")]
        public ActionResult<bool> DeleteCategorias(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Necesita ingresar un id");
            }

            Categoria? categoria = _context.Categorias
                .Include(c => c.Artistas)
                .FirstOrDefault(e => e.Id == id);

            if (categoria == null)
            {
                return NotFound($"No existe Espectaculo con id: {id}");
            }

            if(categoria.Artistas.Any())
            {
                return BadRequest("No se puede eliminar la categoria porque tiene artistas asociados");
            }

            try
            {
                _context.Categorias.Remove(categoria);
                _context.SaveChanges();

                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
