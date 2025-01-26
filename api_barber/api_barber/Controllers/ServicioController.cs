using api_barber.Models;
using api_barber.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace api_barber.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicioController : ControllerBase
    {
        private readonly ServicioService _service;
        public ServicioController(ServicioService servicio)
        {
            _service = servicio;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerServicios()
        {
            var allServicios = await _service.ObtenerTodosLosServicios();
            return Ok(allServicios);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerServiciosPorId(int id)
        {
            var servicio = await _service.ObtenerServiciosPorId(id);

            if (servicio == null)
            {
                return NotFound(new { mensaje = "Servicio no encontrado" });
            }

            return Ok(servicio);
        }

        [HttpPost]
        public async Task<IActionResult> CrearServicios([FromBody] Servicio servicio)
        {
            if (string.IsNullOrEmpty(servicio.Nombre))
            {
                return BadRequest(new { mensaje = "El nombre es obligatorio" });
            }

            if (servicio.Precio < 0)
            {
                return BadRequest(new { mensaje = "El precio no puede estar en negativo" });
            }

            if (servicio.Precio == 0)
            {
                return BadRequest(new { mensaje = "El precio debe ser un valor positivo." });
            }

            var success = await _service.CrearServicio(servicio);

            if (success)
            {
                return CreatedAtAction(nameof(ObtenerServiciosPorId), new { id = servicio.Id }, servicio);
            }

            return StatusCode(500, new { mensaje = "Error al crear el servicio" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarServicio(int id, [FromBody] Servicio servicio)
        {
            if (id != servicio.Id)
            {
                return BadRequest(new { mensaje = "El ID en la URL no coincide con el ID del servicio." });
            }

            if (string.IsNullOrEmpty(servicio.Nombre))
            {
                return BadRequest(new { mensaje = "El nombre es obligatorio" });
            }

            if (servicio.Precio < 0)
            {
                return BadRequest(new { mensaje = "El precio no puede estar en negativo" });
            }

            if (servicio.Precio == 0)
            {
                return BadRequest(new { mensaje = "El precio debe ser un valor positivo." });
            }

            var existeServicio = await _service.ObtenerServiciosPorId(servicio.Id);
            if (existeServicio == null)
            {
                return NotFound(new { mensaje = "Servicio no encontrado o está eliminado." });
            }

            var success = await _service.ActualizarServicios(servicio);
            if (success)
            {
                return Ok(new { mensaje = "Servicio actualizado correctamente." });
            }

            return StatusCode(500, new { mensaje = "Error al actualizar el Servicio." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarServicio(int id)
        {
            var success = await _service.EliminarServicio(id);

            if (!success)
            {
                return NotFound(new { mensaje = "Servicio no encontrado o ya eliminado." });
            }

            return Ok(new { mensaje = "Servicio eliminado correctamente." });
        }
    }
}
