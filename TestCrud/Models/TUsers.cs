using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TestCrud.Models
{
    public partial class TUsers
    {
        public TUsers()
        {
            TAlquiler = new HashSet<TAlquiler>();
            TVenta = new HashSet<TVenta>();
        }

        public int CodUsuario { get; set; }
        public string TxtUser { get; set; }
        public string TxtPassword { get; set; }
        public string TxtNombre { get; set; }
        public string TxtApellido { get; set; }
        public string NroDoc { get; set; }
        public int? CodRol { get; set; }

        [Range(-1, 0, ErrorMessage = "Ingrese un número válido")]
        public int? SnActivo { get; set; }

        public virtual TRol CodRolNavigation { get; set; }
        public virtual ICollection<TAlquiler> TAlquiler { get; set; }
        public virtual ICollection<TVenta> TVenta { get; set; }
    }
}
