namespace AlmacenTecnologico.Models.ViewModel
{
    public class DetallePedidoViewModel
    {
        public int IdDetalle { get; set; }
        public int PedidoId { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public int PrecioTotal { get; set; }
        public  Pedido Pedido { get; set; }
        public  Producto Producto { get; set; }
    }
}
