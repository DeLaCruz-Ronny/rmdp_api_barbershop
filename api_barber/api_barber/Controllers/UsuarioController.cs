using api_barber.Models;
using api_barber.Servicios;
using api_barber.Usuarios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace api_barber.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuariosService _usuarios;
        public UsuarioController(UsuariosService usuarios)
        {
            _usuarios = usuarios;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerUsuarios()
        {
            var allusuarios = await _usuarios.ObtenerTodosLosUsarios();
            return Ok(allusuarios);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerUsuariosPorId(int id)
        {
            var usuarios = await _usuarios.ObtenerUsuariosPorId(id);

            if (usuarios == null)
            {
                return NotFound(new { mensaje = "Usuarios no encontrado" });
            }

            return Ok(usuarios);
        }

        [HttpPost]
        public async Task<IActionResult> CrearUsuarios([FromBody] Usuario usuario)
        {
            if (string.IsNullOrEmpty(usuario.Nombre))
            {
                return BadRequest(new { mensaje = "El nombre es obligatorio" });
            }

            if (string.IsNullOrEmpty(usuario.Telefono))
            {
                return BadRequest(new { mensaje = "El telefono es obligatorio" });
            }

            if (string.IsNullOrEmpty(usuario.Rol))
            {
                return BadRequest(new { mensaje = "El Rol es obligatorio" });
            }

            var success = await _usuarios.CrearUsuarios(usuario);

            if (success)
            {
                return CreatedAtAction(nameof(ObtenerUsuariosPorId), new { id = usuario.Id }, usuario);
            }

            return StatusCode(500, new { mensaje = "Error al crear el usuario" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarUsuario(int id, [FromBody] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return BadRequest(new { mensaje = "El ID en la URL no coincide con el ID del usuario." });
            }

            if (string.IsNullOrEmpty(usuario.Nombre))
            {
                return BadRequest(new { mensaje = "El nombre es obligatorio" });
            }

            if (string.IsNullOrEmpty(usuario.Telefono))
            {
                return BadRequest(new { mensaje = "El telefono es obligatorio" });
            }

            if (string.IsNullOrEmpty(usuario.Rol))
            {
                return BadRequest(new { mensaje = "El Rol es obligatorio" });
            }

            var existeUsuario = await _usuarios.ObtenerUsuariosPorId(usuario.Id);
            if (existeUsuario == null)
            {
                return NotFound(new { mensaje = "Usuario no encontrado." });
            }

            var success = await _usuarios.ActualizarUsuarios(usuario);
            if (success)
            {
                return Ok(new { mensaje = "Usuario actualizado correctamente." });
            }

            return StatusCode(500, new { mensaje = "Error al actualizar el Usuario." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarUsuario(int id)
        {
            var success = await _usuarios.EliminarUsuario(id);

            if (!success)
            {
                return NotFound(new { mensaje = "Usuario no encontrado o ya eliminado." });
            }

            return Ok(new { mensaje = "Usuario eliminado correctamente." });
        }
    }
}
