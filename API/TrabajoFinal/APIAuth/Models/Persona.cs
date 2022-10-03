using System;
using System.Collections.Generic;

namespace APIAuth.Models
{
    public partial class Persona
    {
        public Persona()
        {
            Pedidos = new HashSet<Pedido>();
            Usuarios = new HashSet<Usuario>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Dni { get; set; }
        public int EstadoId { get; set; }

        public virtual ICollection<Pedido> Pedidos { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
