using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SGHR.Persistance.Models;


public partial class Tarifa
{
    [Key]
    public int IdTarifa { get; set; }

    public int IdCategoriaHabitacion { get; set; }

    public DateTime? FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }

    public string? TipoTemporada { get; set; } = null!;

    public decimal Precio { get; set; }

    public bool Estado { get; set; }

    public virtual CategoriasHabitacion IdCategoriaHabitacionNavigation { get; set; } = null!;

   
}
