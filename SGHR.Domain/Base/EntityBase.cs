namespace SGHR.Domain.Base
{
    public abstract class EntityBase
    {
        public int Id { get; protected set; }
        

        protected EntityBase() { }
    }
}
