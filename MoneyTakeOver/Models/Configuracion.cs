﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyTakeOver.Models
{
    public class Configuracion
    {
        [Key]
        public int Id { get; set; }

        public int TipoCambioBaseId { get; set; }

        [ForeignKey("TipoCambioBaseId")]
        public TiposCambio? TipoCambioBase { get; set; }
    }
}
