using SGHR.WebApp.Api;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGHR.Persistance.Models;

public partial class CategoriasHabitacion
{
    [Key]
    public int IdCategoriaHabitacion { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public decimal? TarifaBase { get; set; }

    public string? Caracteristicas { get; set; }

    public bool Estado { get; set; }

    public virtual ICollection<Tarifa> Tarifas { get; set; } = new List<Tarifa>();

    public virtual ICollection<Habitaciones> Habitaciones { get; set; } = new List<Habitaciones>();


    public CategoriasHabitacion(string nombre, string? descripcion, string? caracteristicas)
    {
        Nombre = nombre;
        Descripcion = descripcion;
        Caracteristicas = caracteristicas;
        
    }

   
}
