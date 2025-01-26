using System;
using System.Collections.Generic;

namespace api_barber.Models;

public partial class Turno
{
    public int Id { get; set; }

    public int IdCliente { get; set; }

    public int IdBarbero { get; set; }

    public int IdServicio { get; set; }

    public DateTime FechaHora { get; set; }

    public string? Estado { get; set; }

    public bool? Activo { get; set; }

    public virtual Usuario IdBarberoNavigation { get; set; } = null!;

    public virtual Usuario IdClienteNavigation { get; set; } = null!;

    public virtual Servicio IdServicioNavigation { get; set; } = null!;

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
