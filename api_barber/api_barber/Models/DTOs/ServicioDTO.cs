namespace api_barber.Models.DTOs
{
    public class ServicioDTO
    {
        public string Nombre { get; set; } = null!;

        public string? Descripcion { get; set; }

        public decimal Precio { get; set; }
    }
}
