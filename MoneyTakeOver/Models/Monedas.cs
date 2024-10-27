using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyTakeOver.Models
{
    public class Monedas
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        public string? Nombre { get; set; }

        public bool ActivoDivisa { get; set; }

        public override string ToString()
            {
                return $"{Nombre} - {Id} - {ActivoDivisa}"; // Cambia esto para incluir las propiedades relevantes.
            }

    }
}
