using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MoneyTakeOver.Models
{
    public class TiposCambio
    {
       [Key]
        public int Id { get; set; }

        public int MonedaId { get; set; }

        [ForeignKey("MonedaId")]
        public Monedas? Moneda { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal TipoCambioCompra { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal TipoCambioVenta { get; set; }   
    }
}