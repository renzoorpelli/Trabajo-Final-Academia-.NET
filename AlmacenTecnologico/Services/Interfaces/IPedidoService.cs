using AlmacenTecnologico.Context;
using AlmacenTecnologico.Models;
using AlmacenTecnologico.Models.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlmacenTecnologico.Services.Interfaces
{
    public interface IPedidoService
    {
        Task<List<PedidoViewModel>> ListarPedidos(TrabajoFinalContext context);

        Task<List<PedidoViewModel>> ListarPedidosEliminados(TrabajoFinalContext context);

        Task<List<SelectListItem>> ListarClientes(TrabajoFinalContext context);

        Task<List<SelectListItem>> ListarEmpleados(TrabajoFinalContext context);

        Task<PedidoViewModel> GetPedido(int id, TrabajoFinalContext context);

        Task<bool> EditarPedido(PedidoViewModel request, TrabajoFinalContext context);

        // detalle pedidos
        Task<List<DetallePedidoViewModel>> ListarDetallePedido(int idPedido, TrabajoFinalContext context);

        Task<List<SelectListItem>> ListarProductos(TrabajoFinalContext context);

        Task<int> ActualizarProducto(TrabajoFinalContext context, DetallePedido requestFromAdd = null, DetallePedidoViewModel requestFromEdit = null, bool isUpdate = false);

        Task<DetallePedidoViewModel> GetDetallePedido(int idDetalle, TrabajoFinalContext context);

        Task<bool> RecuperarProductos(DetallePedido requestFromDelete, TrabajoFinalContext context);

    }
}
