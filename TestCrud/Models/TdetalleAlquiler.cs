using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TestCrud.Models
{
    public partial class TdetalleAlquiler
    {
        public int CodAlquiler { get; set; }
        public int CodDetalleAlquiler { get; set; }
        public int? CodPelicula { get; set; }
        public decimal? Precio { get; set; }
        public DateTime? FechaDevolucion { get; set; }

        public virtual TAlquiler CodAlquilerNavigation { get; set; }
        public virtual TPelicula CodPeliculaNavigation { get; set; }
    }
}
