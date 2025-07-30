using SGHR.Web.ViewModel.ServicioCategoria;

public class ServicioConPreciosViewModel : ServicioCategoriaViewModel
{
    public int IdServicio { get; set; }
    public string? Nombre { get; set; } // Nombre del servicio principal
    public string? Descripcion { get; set; }
    public bool Activo { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaModificacion { get; set; } // Asegúrate de que el nombre coincida con tu ServiciosViewModel
    public bool Eliminado { get; set; }
    public DateTime? FechaEliminacion { get; set; }
    public List<ServicioCategoriaViewModel>? PreciosPorCategoria { get; set; }

}