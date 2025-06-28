using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pañol.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Cod_Barras { get; set; }
        public string Detalle { get; set; }
        public DateTime F_Alta { get; set; }
        public DateTime? F_Baja { get; set; }
        public bool Disponible { get; set; }

    }
}