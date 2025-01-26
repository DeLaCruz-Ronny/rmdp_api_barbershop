using api_barber.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace api_barber.Servicios
{
    public class ServicioService
    {
        private readonly string ?_context;

        public ServicioService(IConfiguration configuration)
        {
            _context = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<Servicio>> ObtenerTodosLosServicios()
        {
            var servicios = new List<Servicio>();

            using var connection = new MySqlConnection(_context);
            await connection.OpenAsync();

            string query = "SELECT id,nombre,descripcion,precio,activo FROM Servicios WHERE ACTIVO <> 0";
            using var command = new MySqlCommand(query, connection);

            using var reader = await command.ExecuteReaderAsync();
            while(await reader.ReadAsync())
            {
                servicios.Add(new Servicio
                {
                    Id = reader.GetInt32("id"),
                    Nombre = reader.GetString("Nombre"),
                    Descripcion = reader.GetString("Descripcion"),
                    Precio = reader.GetDecimal("Precio"),
                    Activo = reader.GetBoolean("Activo")
                });
            }

            return servicios;
        }
    }
}
