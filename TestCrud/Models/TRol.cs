using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TestCrud.Models
{
    public partial class TRol
    {
        public TRol()
        {
            TUsers = new HashSet<TUsers>();
        }

        public int CodRol { get; set; }
        public string TxtDesc { get; set; }
        public int? SnActivo { get; set; }

        public virtual ICollection<TUsers> TUsers { get; set; }
    }
}
