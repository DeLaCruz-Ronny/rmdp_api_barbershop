using System;
using System.Collections.Generic;

namespace api_barber.Models;

public partial class Servicio
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public decimal Precio { get; set; }

    public bool? Activo { get; set; }

    public virtual ICollection<Turno> Turnos { get; set; } = new List<Turno>();
}
