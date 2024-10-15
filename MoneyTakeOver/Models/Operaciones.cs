using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MoneyTakeOver.Models
{
    public class Operaciones
    {
        [Key]
        public int Id { get; set; }

        public int ConfiguracionId { get; set; }

        [ForeignKey("ConfiguracionId")]
        public Configuracion? Configuracion { get; set; }
    }
}
