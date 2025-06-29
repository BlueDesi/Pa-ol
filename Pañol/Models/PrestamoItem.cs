using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pañol.Models
{
    public class PrestamoItem
    {
        public int Id { get; set; }

        public int PrestamoId { get; set; }
        public Prestamo Prestamo { get; set; }

        public int ItemId { get; set; }
        public Item Item { get; set; }

       
    }
}