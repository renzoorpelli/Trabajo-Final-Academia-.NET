using System;
using System.Collections.Generic;

namespace APIAuth.Models
{
    public partial class DetallePedido
    {
        public int PedidoId { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public int PrecioTotal { get; set; }
        public int IdDetalle { get; set; }

        public virtual Pedido Pedido { get; set; }
        public virtual Producto Producto { get; set; }
    }
}
