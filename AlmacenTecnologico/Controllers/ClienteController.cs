using AlmacenTecnologico.Context;
using AlmacenTecnologico.Models.ViewModel;
using AlmacenTecnologico.Models;
using AlmacenTecnologico.Services;
using AlmacenTecnologico.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlmacenTecnologico.Controllers
{
    [Authorize]
    public class ClienteController : Controller
    {
        private readonly TrabajoFinalContext _context;
        private readonly IClienteService _clienteService;

        public ClienteController(TrabajoFinalContext context, IClienteService clienteService)
        {
            this._context = context;
            this._clienteService = clienteService;
        }
        /// <summary>
        /// metodo action encargado de mostrar todos los clientes del sistema que esten activos
        /// </summary>
        /// <returns>retorna la lista de todos los clientes activos</returns>
        public async Task<IActionResult> Index()
        {
            List<ClienteViewModel> clientes = await _clienteService.ListarClientes(_context);
            return View(clientes);
        }

        /// <summary>
        /// metodo action encargado de mostrar el formulario de carga de clientes
        /// </summary>
        /// <returns></returns>
        public IActionResult Add()
        {
            return View();
        }

        /// <summary>
        /// metodo action el cual es recibido por el formulario de la vista en metodo POST el cual trae en la request todos los datos 
        /// del nuevo cliente a agregar a la lista
        /// </summary>
        /// <param name="request">el cliente que se desea agregar a la lista</param>
        /// <returns>redirecciona al dashboard principal (metodo action index)</returns>
        [HttpPost]
        public IActionResult Add(Cliente request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            request.EstadoId = 1;
            _context.Add(request);
            _context.SaveChanges();
            return RedirectToAction("index");
        }

        /// <summary>
        /// metodo actionencargado de mostrar la vista del formulario editar con los datos del cliente cargado
        /// </summary>
        /// <param name="id">el identificador del cliente</param>
        /// </summary>
        /// <returns>retorna el cliente a la vista</returns>
        [Authorize(Roles = "admin,superadmin")]
        public async Task<IActionResult> Edit(int id)
        {
            ClienteViewModel clienteEditar = await _clienteService.GetCliente(id, _context);
            return View(clienteEditar);
        }

        /// <summary>
        /// metodo action el cual es recibido por el formulario de la vista en metodo POST el cual trae en la request todos los datos 
        /// del cliente a editar de la lista
        /// </summary>
        /// <param name="request">el cliente el cual cuenta con los nuevos datos</param>
        /// </summary>
        /// <returns>redirecciona al dashboard principal (metodo action index)</returns>

        [Authorize(Roles = "admin,superadmin")]
        [HttpPost]
        public async Task<IActionResult> Edit(ClienteViewModel request)
        {
            bool actualizarClientes = await _clienteService.EditarCliente(request, _context);
            await _context.SaveChangesAsync();
            return actualizarClientes ? RedirectToAction("index") : View(request);
        }


        /// <summary>
        /// metodo action encargado de mostrar una vista con todos los datos del cliente que se quiere 
        /// eliminar, con el fin de darle la oportunidad al usuario  si quiere confirmar la eliminacion del cliente
        /// </summary>
        /// <param name="id">el numero identificador que posee el cliente</param>
        /// <returns></returns>
        [Authorize(Roles = "admin,superadmin")]
        public async Task<IActionResult> Delete(int id)
        {
            ClienteViewModel clienteEliminar = await _clienteService.GetCliente(id, _context);
            return View(clienteEliminar);
        }

        /// <summary>
        /// metodo action el cual es recibido por el formulario de la vista en metodo POST el cual trae en la request todos los datos 
        /// del cliente a eliminar de la lista
        /// </summary>
        /// <param name="request">el cliente a eliminar con todos sus datos</param>
        /// <returns>redirecciona al dashboard principal (metodo action index)</returns>
        [HttpPost]
        [Authorize(Roles = "admin,superadmin")]
        public async Task<IActionResult> Delete(ClienteViewModel request)
        {
            Cliente clienteEliminado = await _context.Clientes.FindAsync(request.Id);
            clienteEliminado.EstadoId = 0;
            await _context.SaveChangesAsync();
            return RedirectToAction("index");
        }

        /// <summary>
        /// metodo action encargado de mostrar la lista de cliente que fueron eliminados o que tienen un estado inactivo en el sistema
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "admin,superadmin")]
        public async Task<IActionResult> Recover()
        {
            List<ClienteViewModel> clientesEliminados = await _clienteService.ListarClientesEliminados(_context);
            return View(clientesEliminados);
        }

        /// <summary>
        /// metodo action el cual es recibido por el formulario de la vista en metodo POST el cual trae en la request todos los datos 
        /// del cliente del cual se quiere incorporar nuevamente al sistema
        /// </summary>
        /// <param name="request">el cliente a recuperar con todos sus datos</param>
        /// <returns>redirecciona al dashboard principal (metodo action index)</returns>
        [Authorize(Roles = "admin,superadmin")]
        [HttpPost]
        public async Task<IActionResult> Recover(ClienteViewModel request)
        {
            Cliente clienteEliminado = await _context.Clientes.FindAsync(request.Id);
            clienteEliminado.EstadoId = 1;//recuperado
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
