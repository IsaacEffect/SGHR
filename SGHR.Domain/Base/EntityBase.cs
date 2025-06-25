using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Domain.Base
{
    public abstract class EntityBase
    {
        public int Id { get; protected set; }
        

        protected EntityBase() { }
    }
}
