using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyTakeOver.Models
{
    public class Configuracion
    {
        [Key]
        public int Id { get; set; } // Clave primaria

        public int? TipoCambioBaseId { get; set; } // Clave foránea opcional para el tipo de cambio base

        [ForeignKey(nameof(TipoCambioBaseId))]
        public TiposCambio TipoCambioBase { get; set; } // Relación con la entidad TiposCambio

        // Aquí puedes agregar otras propiedades relevantes para la configuración
        public string NombreConfiguracion { get; set; } // Ejemplo de una propiedad de configuración
        public DateTime FechaCreacion { get; set; } // Ejemplo de una propiedad de fecha
        public bool Activo { get; set; } // Ejemplo de una propiedad booleana
    }
}
