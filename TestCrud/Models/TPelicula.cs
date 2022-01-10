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
            TAlquiler = new HashSet<TAlquiler>();
            TGeneroPelicula = new HashSet<TGeneroPelicula>();
            TVenta = new HashSet<TVenta>();
        }

        public int CodPelicula { get; set; }
        public string TxtDesc { get; set; }
        public int? CantDisponiblesAlquiler { get; set; }
        public int? CantDisponiblesVenta { get; set; }
        public decimal? PrecioAlquiler { get; set; }
        public decimal? PrecioVenta { get; set; }

        public virtual ICollection<TAlquiler> TAlquiler { get; set; }
        public virtual ICollection<TGeneroPelicula> TGeneroPelicula { get; set; }
        public virtual ICollection<TVenta> TVenta { get; set; }
    }
}
