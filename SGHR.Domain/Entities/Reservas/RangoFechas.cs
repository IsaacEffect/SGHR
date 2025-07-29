namespace SGHR.Domain.Entities.Reservas
{
    public class RangoFechas
    {
        public DateTime Entrada { get; }
        public DateTime Salida { get; }

        public RangoFechas(DateTime entrada, DateTime salida)
        {
            if (entrada > salida)
            {
                throw new ArgumentException("Entrada no puede ser mayor que salida.");
            }
            Entrada = entrada;
            Salida = salida;
        }
    }
}
