using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TestCrud.Models
{
    public partial class TGenero
    {
        public TGenero()
        {
            TGeneroPelicula = new HashSet<TGeneroPelicula>();
        }

        public int CodGenero { get; set; }
        public string TxtDesc { get; set; }

        public virtual ICollection<TGeneroPelicula> TGeneroPelicula { get; set; }
    }
}
