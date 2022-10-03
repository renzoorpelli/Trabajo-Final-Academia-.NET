using System;
using System.Collections.Generic;

namespace APIAuth.Models
{
    public partial class Cliente
    {
        public Cliente()
        {
            Pedidos = new HashSet<Pedido>();
        }

        public int Id { get; set; }
        public string RazonSocial { get; set; }
        public string Cuit { get; set; }
        public string Domicilio { get; set; }
        public string Mail { get; set; }
        public int EstadoId { get; set; }

        public virtual ICollection<Pedido> Pedidos { get; set; }
    }
}
