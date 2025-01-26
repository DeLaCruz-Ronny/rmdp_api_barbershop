using api_barber.Models;
using api_barber.Servicios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    }
}
