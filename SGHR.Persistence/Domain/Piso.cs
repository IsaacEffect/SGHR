using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGHR.Persistence.Domain
{
    [Table("Piso")] 
    public class Piso
    {
        [Key]
        [Column("ID_Piso")]
        public int Id { get; set; }

        [Column("NumeroPiso")]
        public int NumeroPiso { get; set; }

        [Column("Descripcion")]
        public string Descripcion { get; set; }
    }
}
