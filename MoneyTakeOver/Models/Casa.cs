using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyTakeOver.Models
{
    public class Casa
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        public string? Nombre { get; set; }

        [MaxLength(50)]
        public string? Direccion { get; set; }

        [MaxLength(10)]
        public string? Ciudad { get; set; }

        [MaxLength(10)]
        public string? Estado { get; set; }

        [MaxLength(10)]
        public string? HInicio { get; set; }

        [MaxLength(10)]
        public string? HCierre { get; set; }
        
    }
}
