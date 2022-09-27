using System;
using System.Collections.Generic;

namespace AlmacenTecnologico.Models
{
    public partial class Categorium
    {
        public Categorium()
        {
            TipoProductos = new HashSet<TipoProducto>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }

        public virtual ICollection<TipoProducto> TipoProductos { get; set; }
    }
}
