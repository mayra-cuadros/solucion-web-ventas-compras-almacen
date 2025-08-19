using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace proyectoWEBSITESmeall.Models;

public partial class GuiaSalidum
{
    public int IdGuiaSalida { get; set; }

    public DateOnly FechaSalida { get; set; }

    public string Responsable { get; set; } = null!;

    public string Destino { get; set; } = null!;

    public int IdAlmacen { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public virtual ICollection<DetalleGuiaSalidum> DetalleGuiaSalida { get; set; } = new List<DetalleGuiaSalidum>();

    [ValidateNever]
    public virtual Almacen IdAlmacenNavigation { get; set; } = null!;
}
