using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Pañol.Models
{
   
     [Table("Roles")]
    public class Rol
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string RolNombre { get; set; }

        // Opcional: colección de usuarios, aunque tu Usuario no tiene propiedad de navegación
        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
    
    
}