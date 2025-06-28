using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Web;

namespace Pañol.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        [Column("IdRol")]
        public int Rol { get; set; }
        [Column("DniId")]
        public string Dni { get; set; }
       

        public Usuario() { }
        public Usuario(string User, string Password)
        {
            this.Username = User;
            this.Password = Password;

        }

      
    }
}