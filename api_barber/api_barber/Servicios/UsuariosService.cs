using api_barber.Models;
using MySql.Data.MySqlClient;
using System.Data;


namespace api_barber.Usuarios
{
    public class UsuariosService
    {
        private readonly string ?_context;

        public UsuariosService(IConfiguration configuration)
        {
            _context = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<Usuario>> ObtenerTodosLosUsarios()
        {
            var servicios = new List<Usuario>();

            using var connection = new MySqlConnection(_context);
            await connection.OpenAsync();

            string query = "SELECT id,nombre,telefono,email,rol,fecha_registro,activo FROM Usuarios WHERE ACTIVO <> 0";
            using var command = new MySqlCommand(query, connection);

            using var reader = await command.ExecuteReaderAsync();
            while(await reader.ReadAsync())
            {
                servicios.Add(new Usuario
                {
                    Id = reader.GetInt32("id"),
                    Nombre = reader.GetString("Nombre"),
                    Telefono = reader.GetString("Telefono"),
                    Email = reader.GetString("Email"),
                    Rol = reader.GetString("Rol"),
                    FechaRegistro = reader.GetDateTime("Fecha_registro"),
                    Activo = reader.GetBoolean("Activo")
                });
            }

            return servicios; 
        }

        public async Task<Usuario?> ObtenerUsuariosPorId(int id)
        {
            Usuario? usuario = null;

            using var connection = new MySqlConnection(_context);
            await connection.OpenAsync();

            string query = "SELECT id,nombre,telefono,email,rol,fecha_registro,activo FROM Usuarios WHERE ACTIVO <> 0 and id = @id";
            using var command = new MySqlCommand(query, connection);

            //Evitamos las inyecciones de sql
            command.Parameters.AddWithValue("@id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                usuario = new Usuario
                {
                    Id = reader.GetInt32("id"),
                    Nombre = reader.GetString("Nombre"),
                    Telefono = reader.GetString("Telefono"),
                    Email = reader.GetString("Email"),
                    Rol = reader.GetString("Rol"),
                    FechaRegistro = reader.GetDateTime("Fecha_registro"),
                    Activo = reader.GetBoolean("Activo")
                };
            }

            return usuario;
        }

        public async Task<bool> CrearUsuarios(Usuario? newsusuarios)
        {
            using var connection = new MySqlConnection(_context);
            await connection.OpenAsync();

            string query = "INSERT INTO Usuarios(nombre,telefono,email,rol) VALUES (@nombre,@telefono,@email,@rol)";
            using var command = new MySqlCommand(query, connection);

            //Evitamos las inyecciones de sql
            command.Parameters.AddWithValue("@nombre", newsusuarios?.Nombre);
            command.Parameters.AddWithValue("@telefono", newsusuarios?.Telefono);
            command.Parameters.AddWithValue("@email", newsusuarios?.Email);
            command.Parameters.AddWithValue("@rol", newsusuarios?.Rol);

            var insertados = await command.ExecuteNonQueryAsync();
            return insertados > 0;
        }

        public async Task<bool> EliminarUsuario(int id)
        {
            using var connection = new MySqlConnection(_context);
            await connection.OpenAsync();

            string query = "UPDATE Usuarios SET activo = 0 WHERE id = @id";
            using var command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@id", id);

            var delete = await command.ExecuteNonQueryAsync();
            return delete > 0;
        }

        public async Task<bool> ActualizarUsuarios(Usuario usuario)
        {
            using var connection = new MySqlConnection(_context);
            await connection.OpenAsync();

            string query = @"UPDATE Usuarios
                             SET nombre = @nombre,
                                 telefono = @telefono,
                                 email = @email,
                                 rol = @rol,
                                 activo = @activo
                             WHERE id = @id";

            using var command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@id", usuario?.Id);
            command.Parameters.AddWithValue("@nombre", usuario?.Nombre);
            command.Parameters.AddWithValue("@telefono", usuario?.Telefono);
            command.Parameters.AddWithValue("@email", usuario?.Email);
            command.Parameters.AddWithValue("@rol", usuario?.Rol);
            command.Parameters.AddWithValue("@activo", usuario?.Activo);

            var update = await command.ExecuteNonQueryAsync();
            return update > 0;
        }
    }
}
