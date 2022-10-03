using System;
using System.Collections.Generic;

namespace APIAuth.Models
{
    public partial class Pedido
    {
        public Pedido()
        {
            DetallePedidos = new HashSet<DetallePedido>();
        }

        public int Id { get; set; }
        public int EmpleadoId { get; set; }
        public int ClienteId { get; set; }
        public DateTime FechaPedido { get; set; }
        public int EstadoId { get; set; }

        public virtual Cliente Cliente { get; set; }
        public virtual Persona Empleado { get; set; }
        public virtual ICollection<DetallePedido> DetallePedidos { get; set; }
    }
}
