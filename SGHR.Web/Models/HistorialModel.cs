namespace SGHR.Web.Models
{
    public class HistorialModel
    {
        public int id { get; set; }
        public DateTime fechaEntrada { get; set; }
        public DateTime fechaSalida { get; set; }
        public string estado { get; set; }
        public decimal? tarifa { get; set; }
        public string tipoHabitacion { get; set; }
        public string serviciosAdicionales { get; set; }
    }


    public class ObtenerFullHistorialResponse
    {
        public bool success { get; set; }
        public object message { get; set; }
        public List<HistorialModel> data { get; set; }
    }

    public class ObtenerHistorialResponse
    {
        public bool success { get; set; }
        public object message { get; set; }
        public HistorialModel data { get; set; }
    }


}
