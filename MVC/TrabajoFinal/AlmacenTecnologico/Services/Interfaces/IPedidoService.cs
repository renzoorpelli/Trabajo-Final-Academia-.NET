using AlmacenTecnologico.Context;
using AlmacenTecnologico.Models;
using AlmacenTecnologico.Models.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlmacenTecnologico.Services.Interfaces
{
    public interface IPedidoService
    {
        Task<List<PedidoViewModel>> ListarPedidos();

        Task<List<PedidoViewModel>> ListarPedidosEliminados();

        Task<List<SelectListItem>> ListarClientes();

        Task<List<SelectListItem>> ListarEmpleados();

        Task<PedidoViewModel> GetPedido(int id);

        Task<bool> AgregarPedido(Pedido request);
        Task<bool> EditarPedido(PedidoViewModel request);
        Task<bool> EliminarPedidoLogico(PedidoViewModel request);
        Task<bool> RecuperarPedido(PedidoViewModel request);

        // detalle pedidos
        Task<List<DetallePedidoViewModel>> ListarDetallePedido(int idPedido);

        Task<List<SelectListItem>> ListarProductos();

        Task<bool> AgregarDetallePedido(DetallePedido request);

        Task<bool> EditarDetallePedido(DetallePedidoViewModel request);

        Task<int> EliminarDetallePedido(int id);

        Task<int> ActualizarProducto(DetallePedido requestFromAdd = null, DetallePedidoViewModel requestFromEdit = null, bool isUpdate = false);

        Task<DetallePedidoViewModel> GetDetallePedido(int idDetalle);

        Task<bool> RecuperarProductos(DetallePedido requestFromDelete);

    }
}
