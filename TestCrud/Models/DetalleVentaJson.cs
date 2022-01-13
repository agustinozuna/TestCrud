using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestCrud.Models
{
    public class DetalleVentaJson
    {

        /* codusuario*/
        public int CodUsuario { get; set; }
        /*todas las peliculas cargadas*/
        public int[] CodPelicula { get; set; }

        /*cantidad de peliculas*/
        public int[] Cantidad { get; set; }
    }
}
