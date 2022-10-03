using System;
using System.Collections.Generic;

namespace APIAuth.Models
{
    public partial class Fabricante
    {
        public Fabricante()
        {
            Productos = new HashSet<Producto>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public int EstadoId { get; set; }

        public virtual ICollection<Producto> Productos { get; set; }
    }
}
