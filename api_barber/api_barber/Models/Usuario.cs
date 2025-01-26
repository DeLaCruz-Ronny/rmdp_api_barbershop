using System;
using System.Collections.Generic;

namespace api_barber.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Telefono { get; set; }

    public string? Email { get; set; }

    public string Rol { get; set; } = null!;

    public DateTime? FechaRegistro { get; set; }

    public bool? Activo { get; set; }

    public virtual ICollection<Turno> TurnoIdBarberoNavigations { get; set; } = new List<Turno>();

    public virtual ICollection<Turno> TurnoIdClienteNavigations { get; set; } = new List<Turno>();
}
