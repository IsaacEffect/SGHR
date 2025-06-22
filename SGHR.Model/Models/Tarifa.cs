using System;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SGHR.Persistance.Models;

public partial class Tarifa
{
    public int IdTarifa { get; set; }

    public int IdCategoriaHabitacion { get; set; }

    public Date FechaInicio { get; set; }

    public Date FechaFin { get; set; }

    public string TipoTemporada { get; set; } = null!;

    public decimal Precio { get; set; }

    public bool Estado { get; set; }

    public virtual CategoriasHabitacion IdCategoriaHabitacionNavigation { get; set; } = null!;
}
