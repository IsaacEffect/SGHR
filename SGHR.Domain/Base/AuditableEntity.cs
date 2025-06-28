namespace SGHR.Domain.Base
{
    public class AuditableEntity : EntityBase
    {
        public DateTime FechaCreacion { get; protected set; }
        public DateTime? FechaModificacion { get; protected set; }
        public bool Activo { get; protected set; }
        protected AuditableEntity()
        {
            FechaCreacion = DateTime.UtcNow;
            FechaModificacion = null;
            Activo = true;
        }

        public void SetFechaUltimaModificacion()
        {
            FechaModificacion = DateTime.UtcNow;
        }

        public void Activar()
        {
            if (!Activo)
            {
                Activo = true;
                SetFechaUltimaModificacion();
            }
        }

        public void Desactivar()
        {
            if (Activo) 
            { 
                Activo = false;
                SetFechaUltimaModificacion();
            }
        }
    }
}
