using System;
using System.Collections.Generic;

namespace api_barber.Models;

public partial class Pago
{
    public int Id { get; set; }

    public int IdTurno { get; set; }

    public decimal Monto { get; set; }

    public string MetodoPago { get; set; } = null!;

    public DateTime? FechaPago { get; set; }

    public bool? Activo { get; set; }

    public virtual Turno IdTurnoNavigation { get; set; } = null!;
}
