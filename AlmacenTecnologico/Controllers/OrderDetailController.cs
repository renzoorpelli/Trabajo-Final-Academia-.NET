using AlmacenTecnologico.Context;
using AlmacenTecnologico.Models;
using AlmacenTecnologico.Models.ViewModel;
using AlmacenTecnologico.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlmacenTecnologico.Controllers
{
    [Authorize]
    public class OrderDetailController : Controller
    {
        private readonly TrabajoFinalContext _context;
        private readonly IPedidoService _pedidoService;
        public OrderDetailController(TrabajoFinalContext context, IPedidoService pedidoService)
        {
            _context = context;
            _pedidoService = pedidoService;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Index(int id)
        {
            ViewBag.Id = id;
            List<DetallePedidoViewModel> pedidosCliente = await _pedidoService.ListarDetallePedido(id, _context);
            return View(pedidosCliente);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Add(int id)
        {
            DetallePedido detalleAsignar = new DetallePedido();
            detalleAsignar.PedidoId = id;
            ViewData["Productos"] = await _pedidoService.ListarProductos(_context);
            return View(detalleAsignar);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Add(DetallePedido request)
        {
            int actualizarProducto = await _pedidoService.ActualizarProducto(_context, request);
            if (actualizarProducto == 0)
            {
                ViewBag.message = "los datos son invalidos";
                return View(request);
            }
            request.PrecioTotal = actualizarProducto;
            await _context.DetallePedidos.AddAsync(request);
            await _context.SaveChangesAsync();

            return RedirectToAction("index", "orderdetail", new { id = request.PedidoId });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(int id)
        {
            DetallePedidoViewModel detalleEditar = await _pedidoService.GetDetallePedido(id, _context);
            ViewData["Productos"] = await _pedidoService.ListarProductos(_context);
            return View(detalleEditar);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Edit(DetallePedidoViewModel request)
        {
            int actualizarProducto = await _pedidoService.ActualizarProducto(_context, null, request, true);
            if (actualizarProducto != 0)
            {
                await _context.SaveChangesAsync();
                return RedirectToAction("index", "orderdetail", new {id = request.PedidoId});
            }
            ViewBag.message = "los datos son invalidos";
            return View(request);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(int id)
        {
            DetallePedido pedidoEliminar = await _context.DetallePedidos.FindAsync(id);
            int retorno = pedidoEliminar.PedidoId;
            bool actualizarProducto = await _pedidoService.RecuperarProductos(pedidoEliminar, _context);
            if (actualizarProducto)
            {
                _context.DetallePedidos.Remove(pedidoEliminar);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("index", "orderdetail", new { id = retorno });
        }
    }
}
