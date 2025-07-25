using System.ComponentModel.DataAnnotations;

namespace SGHR.Web.ViewModel.Reservas
{
    public class CancelarReservaViewModel
    {
       public int Id { get; set; }

       [Required(ErrorMessage = "Debe ingresar un motivo")]
       public string? MotivoCancelacion { get; set; }      
    }
}
