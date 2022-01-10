using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TestCrud.Models
{
    public partial class TGeneroPelicula
    {
        public int CodPelicula { get; set; }
        public int CodGenero { get; set; }

        public virtual TGenero CodGeneroNavigation { get; set; }
        public virtual TPelicula CodPeliculaNavigation { get; set; }
    }
}
