using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TestCrud.Models
{
    public partial class TVenta
    {
        public int CodVenta { get; set; }
        public int? CodUsuario { get; set; }
        public int? CodPelicula { get; set; }
        public decimal? Precio { get; set; }
        public DateTime? Fecha { get; set; }

        public virtual TPelicula CodPeliculaNavigation { get; set; }
        public virtual TUsers CodUsuarioNavigation { get; set; }
    }
}
