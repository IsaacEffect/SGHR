using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SGHR.Web.ViewModel.Servicios
{
    public class EditarServiciosViewModel
    {
        [HiddenInput]
        public int IdServicio { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public required string Nombre { get; set; }
        [Required(ErrorMessage = "La descripcion es obligatoria")]
        public required string Descripcion { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "La duración debe ser mayor a 0")]
        public int Duracion { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "La capacidad debe ser mayor a 0")]
        public int Capacidad { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una categoría")]
        public int IdCategoriaServicio { get; set; }
        public bool Activo { get; set; }

    }
}
