namespace SGHR.Application.Interfaces.Servicios
{
    public interface IServicioRules
    {
        Task ValidarExistenciaSerivicioAsync(int idServicio);
        Task ValidarDatosBasicosAsync(string nombre, string descripcion);
        void ValidarPrecioServicio(decimal precio);
        void ValidarIdPositivo(int idServicio);
        void ValidarEntidadNula(object? entidad, string mensaje);
    }
}
