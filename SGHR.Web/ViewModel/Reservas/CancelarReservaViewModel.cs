using System.ComponentModel.DataAnnotations;

namespace SGHR.Web.ViewModel.Reservas
{
    public class CancelarReservaViewModel
    {
       public int Id { get; set; }

       [Required(ErrorMessage = "Debe ingresar un motivo")]
       [StringLength(500, ErrorMessage = "El motivo no puede exceder los 500 caracteres.")]
       public string MotivoCancelacion { get; set; } = string.Empty;      
    }
}
