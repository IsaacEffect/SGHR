using SGHR.Persistance.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SGHR.WebApp.Api;

public partial class Habitaciones
{
    [Key]
    public int IdHabitacion { get; set; }

    public string NumeroHabitacion { get; set; } = null!;

    public int IdCategoriaHabitacion { get; set; }

    public int IdPiso { get; set; }

    public bool EstadoHabitacion { get; set; }

    public virtual CategoriasHabitacion CategoriaHabitacion { get; set; } = null!;
}
