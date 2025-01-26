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

        public async Task<Servicio?> ObtenerServiciosPorId(int id)
        {
            Servicio? servicios = null;

            using var connection = new MySqlConnection(_context);
            await connection.OpenAsync();

            string query = "SELECT id,nombre,descripcion,precio,activo FROM Servicios WHERE ACTIVO <> 0 and id = @id";
            using var command = new MySqlCommand(query, connection);

            //Evitamos las inyecciones de sql
            command.Parameters.AddWithValue("@id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                servicios = new Servicio
                {
                    Id = reader.GetInt32("id"),
                    Nombre = reader.GetString("Nombre"),
                    Descripcion = reader.GetString("Descripcion"),
                    Precio = reader.GetDecimal("Precio"),
                    Activo = reader.GetBoolean("Activo")
                };
            }

            return servicios;
        }

        public async Task<bool> CrearServicio(Servicio? newservicios)
        {
            using var connection = new MySqlConnection(_context);
            await connection.OpenAsync();

            string query = "INSERT INTO Servicios(nombre,descripcion,precio) VALUES (@nombre,@descripcion,@precio)";
            using var command = new MySqlCommand(query, connection);

            //Evitamos las inyecciones de sql
            command.Parameters.AddWithValue("@nombre", newservicios?.Nombre);
            command.Parameters.AddWithValue("@descripcion", newservicios?.Descripcion);
            command.Parameters.AddWithValue("@precio", newservicios?.Precio);

            var insertados = await command.ExecuteNonQueryAsync();
            return insertados > 0;
        }

        public async Task<bool> EliminarServicio(int id)
        {
            using var connection = new MySqlConnection(_context);
            await connection.OpenAsync();

            string query = "UPDATE Servicios SET activo = 0 WHERE id = @id";
            using var command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@id", id);

            var delete = await command.ExecuteNonQueryAsync();
            return delete > 0;
        }

        public async Task<bool> ActualizarServicios(Servicio servicio)
        {
            using var connection = new MySqlConnection(_context);
            await connection.OpenAsync();

            string query = @"UPDATE Servicios
                             SET nombre = @nombre,
                                 descripcion = @descripcion,
                                 precio = @precio,
                                 activo = @activo
                             WHERE id = @id";

            using var command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@id", servicio.Id);
            command.Parameters.AddWithValue("@nombre", servicio.Nombre);
            command.Parameters.AddWithValue("@descripcion", servicio.Descripcion);
            command.Parameters.AddWithValue("@precio", servicio.Precio);
            command.Parameters.AddWithValue("@activo", servicio.Activo);

            var update = await command.ExecuteNonQueryAsync();
            return update > 0;
        }
    }
}
