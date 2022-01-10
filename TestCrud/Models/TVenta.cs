using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TestCrud.Models
{
    public partial class TVenta
    {
        public TVenta()
        {
            TdetalleVenta = new HashSet<TdetalleVenta>();
        }

        public int CodVenta { get; set; }
        public int? CodUsuario { get; set; }
        public decimal? Total { get; set; }
        public DateTime? Fecha { get; set; }

        public virtual TUsers CodUsuarioNavigation { get; set; }
        public virtual ICollection<TdetalleVenta> TdetalleVenta { get; set; }
    }
}
