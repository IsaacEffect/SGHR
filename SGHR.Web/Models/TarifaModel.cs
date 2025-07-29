using System.ComponentModel.DataAnnotations;

namespace SGHR.Web.Models
{
    public class TarifaModel
    {
        public int CategoriaId{ get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Debe ser un numero valido.")]
        public decimal TarifaBase { get; set; }
    }
}
