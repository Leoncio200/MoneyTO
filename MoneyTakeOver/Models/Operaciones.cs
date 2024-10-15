using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyTakeOver.Models
{
    public class Operaciones
    {
        [Key] // Indica que esta propiedad es la clave primaria
        public int Id { get; set; }

        // Relación con la entidad Configuracion
        public int ConfiguracionId { get; set; } // Llave foránea que hace referencia a Configuracion

        [ForeignKey("ConfiguracionId")] // Indica que esta propiedad es una clave foránea
        public Configuracion Configuracion { get; set; } // Navegación a Configuracion

        // Agrega otras propiedades que sean necesarias para tu entidad Operaciones
        public decimal Monto { get; set; } // Ejemplo: Monto de la operación
        public DateTime Fecha { get; set; } // Ejemplo: Fecha de la operación
        public string TipoOperacion { get; set; } // Ejemplo: Tipo de operación (compra/venta)
    }
}
