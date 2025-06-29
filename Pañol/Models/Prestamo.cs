using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Pañol.Models
{
    [Table("Prestamos")]
    public class Prestamo
    {
        public int Id { get; set; }

        public int ProfesorId { get; set; }
        public Profesor Profesor { get; set; }

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public string Curso { get; set; }
        public DateTime FechaHora_E { get; set; }
        public DateTime? FechaHora_D { get; set; }

        public string Retira { get; set; }
        public bool Cancela { get; set; }
        public string Observacion { get; set; }
        public ICollection<PrestamoItem> PrestamoItems { get; set; }
    }
}