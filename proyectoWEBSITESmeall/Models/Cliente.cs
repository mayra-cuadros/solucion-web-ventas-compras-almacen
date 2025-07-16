using System;
using System.Collections.Generic;

namespace proyectoWEBSITESmeall.Models;

public partial class Cliente
{
    public int IdCliente { get; set; }

    public string Nombres { get; set; } = null!;

    public string? Correo { get; set; }

    public string? Telefono { get; set; }

    public string? Direccion { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public virtual ICollection<Ventum> Venta { get; set; } = new List<Ventum>();
}
