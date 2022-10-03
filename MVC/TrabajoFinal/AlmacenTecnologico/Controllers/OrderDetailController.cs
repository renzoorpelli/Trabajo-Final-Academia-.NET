using AlmacenTecnologico.Context;
using AlmacenTecnologico.Models;
using AlmacenTecnologico.Models.ViewModel;
using AlmacenTecnologico.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace AlmacenTecnologico.Controllers
{
    [Authorize(Roles = "superadmin,admin,empleado")]
    public class OrderDetailController : Controller
    {
        private readonly IPedidoService _pedidoService;
        public OrderDetailController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }
        /// <summary>
        /// metodo action encargado de mosotrar todos los detalles de un pedido
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Index(int id)
        {
            ViewBag.Id = id;
            List<DetallePedidoViewModel> pedidosCliente = await _pedidoService.ListarDetallePedido(id);
            return View(pedidosCliente);
        }
        /// <summary>
        /// metodo action encargado de agregar un detalle al pedido del usuario
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Add(int id)
        {
            DetallePedido detalleAsignar = new DetallePedido();
            detalleAsignar.PedidoId = id;
            ViewData["Productos"] = await _pedidoService.ListarProductos();
            return detalleAsignar is not null ? View(detalleAsignar) : View();
        }

        /// <summary>
        /// metodo action encargado de agregar un detalle (producto) al pedido, para agregar un detalle se tendran que descontar
        /// de las unidaddes disponibles del pedido que se esta agregando
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Add(DetallePedido request)
        {
            bool agregarDetalle = await _pedidoService.AgregarDetallePedido(request);
            return agregarDetalle ? RedirectToAction("index", "orderdetail", new { id = request.PedidoId }) : View(request);
        }

        /// <summary>
        /// metodo action encargado de mostrar el detalle del pedido que se le pase por parametro
        /// </summary>
        /// <param name="id">el id del detalle</param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(int id)
        {
            DetallePedidoViewModel detalleEditar = await _pedidoService.GetDetallePedido(id);
            ViewData["Productos"] = await _pedidoService.ListarProductos();
            return View(detalleEditar);
        }

        /// <summary>
        /// metodo encargado de editar el detalle del producto reemplazando este con los nuevos datos
        /// </summary>
        /// <param name="request">el detalle del pedido que se quiere editar</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Edit(DetallePedidoViewModel request)
        {
            bool editarDetalle = await _pedidoService.EditarDetallePedido(request);
            return editarDetalle ? RedirectToAction("index", "orderdetail", new { id = request.PedidoId }) : View(request);
        }

        /// <summary>
        /// metodo encargado de eliminar un detalle del producto
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(int id)
        {
            int eliminarDetalle = await _pedidoService.EliminarDetallePedido(id);
            if (eliminarDetalle is not 0)
            {
                return RedirectToAction("index", "orderdetail", new { id = eliminarDetalle });
            }
            return View();
        }
    }
}
