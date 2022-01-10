using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TestCrud.Models
{
    public partial class TPelicula
    {
        public TPelicula()
        {
            TGeneroPelicula = new HashSet<TGeneroPelicula>();
            TdetalleAlquiler = new HashSet<TdetalleAlquiler>();
            TdetalleVenta = new HashSet<TdetalleVenta>();
        }

        public int CodPelicula { get; set; }
        public string TxtDesc { get; set; }
        public int? CantDisponiblesAlquiler { get; set; }
        public int? CantDisponiblesVenta { get; set; }
        public decimal? PrecioAlquiler { get; set; }
        public decimal? PrecioVenta { get; set; }

        public virtual ICollection<TGeneroPelicula> TGeneroPelicula { get; set; }
        public virtual ICollection<TdetalleAlquiler> TdetalleAlquiler { get; set; }
        public virtual ICollection<TdetalleVenta> TdetalleVenta { get; set; }
    }
}
