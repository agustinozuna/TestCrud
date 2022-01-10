using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TestCrud.Models
{
    public partial class TAlquiler
    {
        public TAlquiler()
        {
            TdetalleAlquiler = new HashSet<TdetalleAlquiler>();
        }

        public int CodAlquiler { get; set; }
        public int? CodUsuario { get; set; }
        public decimal? Total { get; set; }
        public DateTime? Fecha { get; set; }

        public virtual TUsers CodUsuarioNavigation { get; set; }
        public virtual ICollection<TdetalleAlquiler> TdetalleAlquiler { get; set; }
    }
}
