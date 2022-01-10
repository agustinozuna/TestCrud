using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TestCrud.Models
{
    public partial class TdetalleVenta
    {
        public int CodVenta { get; set; }
        public int CodDetalleVenta { get; set; }
        public int? CodPelicula { get; set; }
        public int? Cantidad { get; set; }
        public decimal? PrecioUnidad { get; set; }
        public decimal? PrecioTotal { get; set; }

        public virtual TPelicula CodPeliculaNavigation { get; set; }
        public virtual TVenta CodVentaNavigation { get; set; }
    }
}
