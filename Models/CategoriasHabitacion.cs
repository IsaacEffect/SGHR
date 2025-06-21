using System;
using System.Collections.Generic;

namespace SGHR.Persistance.Models;

public partial class CategoriasHabitacion
{
    public int IdCategoriaHabitacion { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public decimal TarifaBase { get; set; }

    public string? Caracteristicas { get; set; }

    public bool Estado { get; set; }

    public virtual ICollection<Tarifa> Tarifas { get; set; } = new List<Tarifa>();
}
