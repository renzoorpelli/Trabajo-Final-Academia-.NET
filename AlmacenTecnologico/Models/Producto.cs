using System;
using System.Collections.Generic;

namespace AlmacenTecnologico.Models
{
    public partial class Producto
    {
        public Producto()
        {
            DetallePedidos = new HashSet<DetallePedido>();
        }

        public int Id { get; set; }
        public string Modelo { get; set; }
        public int IdFabricante { get; set; }
        public int IdTipoProducto { get; set; }
        public double Precio { get; set; }
        public string UrlImagen { get; set; }
        public int EstadoId { get; set; }
        public int? CantidadStock { get; set; }
        public int? UnidadesDisponibles { get; set; }

        public virtual Fabricante IdFabricanteNavigation { get; set; }
        public virtual TipoProducto IdTipoProductoNavigation { get; set; }
        public virtual ICollection<DetallePedido> DetallePedidos { get; set; }
    }
}
