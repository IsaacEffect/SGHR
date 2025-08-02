namespace SGHR.Web.ViewModel.Reservas
{
    public class ReservasIndexViewModel
    {
        public List<ReservasViewModel> Reservas { get; set; } = [];
        public string? FechaDesde { get; set; }
        public string? FechaHasta { get; set; }
        public bool MostrandoFiltro { get; set; }
        public string? MensajeError { get; set; }
    }
}
