using AlmacenTecnologico.Context;
using AlmacenTecnologico.Models.ViewModel;
using AlmacenTecnologico.Models;
using AlmacenTecnologico.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlmacenTecnologico.Controllers
{
    [Authorize(Roles = "superadmin,admin,empleado")]
    public class OrderController : Controller
    {
        private readonly IPedidoService _pedidoService;
        public OrderController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        /// <summary>
        /// metodo action encargado de mostrar todos los pedidos del sistema que esten activos
        /// </summary>
        /// <returns>retorna la lista de todos los pedidos activos</returns>
        public async Task<IActionResult> Index()
        {
            List<PedidoViewModel> pedidos = await _pedidoService.ListarPedidos();
            return View(pedidos);
        }

        /// <summary>
        /// metodo action encargado de mostrar el formulario de carga de pedidos, los viewData
        /// permitiran la lista dinamica de clientes activos  y empleados que gestionaron el pedido al momento de la carga
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Add()
        {
            ViewData["Empleados"] = await _pedidoService.ListarEmpleados();
            ViewData["Clientes"] = await _pedidoService.ListarClientes();
            return View();
        }

        /// <summary>
        /// metodo action el cual es recibido por el formulario de la vista en metodo POST el cual trae en la request todos los datos 
        /// del nuevo pedido a agregar a la lista
        /// </summary>
        /// <param name="request">el pedido que se desea agregar a la lista</param>
        /// <returns>redirecciona al dashboard principal (metodo action index)</returns>
        [HttpPost]
        public async Task<IActionResult> Add(Pedido request)
        {
            bool agregarPedido = await _pedidoService.AgregarPedido(request);
            return agregarPedido ? RedirectToAction("index") : View(request);
        }

        /// <summary>
        /// metodo actionencargado de mostrar la vista del formulario editar con los datos del pedido cargado
        /// </summary>
        /// <param name="id">el identificador del pedido</param>
        /// </summary>
        /// <returns>retorna el pedido a la vista</returns>
        [Authorize(Roles = "admin,superadmin")]
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["Empleados"] = await _pedidoService.ListarEmpleados();
            ViewData["Clientes"] = await _pedidoService.ListarClientes();
            PedidoViewModel pedidoEditar = await _pedidoService.GetPedido(id);
            return View(pedidoEditar);
        }

        /// <summary>
        /// metodo action el cual es recibido por el formulario de la vista en metodo POST el cual trae en la request todos los datos 
        /// del pedido a editar de la lista
        /// </summary>
        /// <param name="request">el pedido el cual cuenta con los nuevos datos</param>
        /// </summary>
        /// <returns>redirecciona al dashboard principal (metodo action index)</returns>

        [Authorize(Roles = "admin,superadmin")]
        [HttpPost]
        public async Task<IActionResult> Edit(PedidoViewModel request)
        {
            bool actualizarProducto = await _pedidoService.EditarPedido(request);
            return actualizarProducto ? RedirectToAction("index") : View(request);
        }


        /// <summary>
        /// metodo action encargado de mostrar una vista con todos los datos del pedido que se quiere 
        /// eliminar, con el fin de darle la oportunidad al usuario  si quiere confirmar la eliminacion del pedido
        /// </summary>
        /// <param name="id">el numero identificador que posee el pedido</param>
        /// <returns></returns>
        [Authorize(Roles = "admin,superadmin")]
        public async Task<IActionResult> Delete(int id)
        {
            PedidoViewModel pedidoEliminar = await _pedidoService.GetPedido(id);
            return View(pedidoEliminar);
        }

        /// <summary>
        /// metodo action el cual es recibido por el formulario de la vista en metodo POST el cual trae en la request todos los datos 
        /// del pedido a eliminar de la lista
        /// </summary>
        /// <param name="request">el pedido a eliminar con todos sus datos</param>
        /// <returns>redirecciona al dashboard principal (metodo action index)</returns>
        [HttpPost]
        [Authorize(Roles = "admin,superadmin")]
        public async Task<IActionResult> Delete(PedidoViewModel request)
        {
            bool eliminarLogico = await _pedidoService.EliminarPedidoLogico(request);
            return eliminarLogico ? RedirectToAction("index") : View(request);
        }

        /// <summary>
        /// metodo action encargado de mostrar la lista de pedidos que fueron eliminados o que tienen un estado inactivo en el sistema
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "admin,superadmin")]
        public async Task<IActionResult> Recover()
        {
            List<PedidoViewModel> pedidosEliminados = await _pedidoService.ListarPedidosEliminados();
            return View(pedidosEliminados);
        }

        /// <summary>
        /// metodo action el cual es recibido por el formulario de la vista en metodo POST el cual trae en la request todos los datos 
        /// del pedido del cual se quiere incorporar nuevamente al sistema
        /// </summary>
        /// <param name="request">el pedido a recuperar con todos sus datos</param>
        /// <returns>redirecciona al dashboard principal (metodo action index)</returns>
        [Authorize(Roles = "admin,superadmin")]
        [HttpPost]
        public async Task<IActionResult> Recover(PedidoViewModel request)
        {
            bool recuperarPedido = await _pedidoService.RecuperarPedido(request);
            return recuperarPedido ? RedirectToAction("index") : View(request);
        }

    }
}
