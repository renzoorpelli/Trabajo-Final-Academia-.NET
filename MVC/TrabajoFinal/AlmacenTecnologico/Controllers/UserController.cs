using AlmacenTecnologico.Models;
using AlmacenTecnologico.Models.ViewModel;
using AlmacenTecnologico.Services;
using AlmacenTecnologico.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlmacenTecnologico.Controllers
{
    [Authorize(Roles = "superadmin")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            this._userService = userService;
        }

        /// <summary>
        /// metodo index que muestra todos los usuarios activos al administrador
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            List<UsuarioViewModel> listaUsuarios = await _userService.ListarUsuarios(UsuarioViewModel.Estado.Activo);
            return View(listaUsuarios);
        }

        [Authorize(Roles = "superadmin")]
        public async Task<IActionResult> Add()
        {
            ViewData["ListarEmpleados"] = await _userService.ListarEmpleados();
            ViewData["ListarRoles"] = await _userService.ListarRoles();
            return View();
        }

        /// <summary>
        /// metodo action el cual es recibido por el formulario de la vista en metodo POST el cual trae en la request todos los datos 
        /// del nuevo usuario a agregar a la lista
        /// </summary>
        /// <param name="request">el usuario que se desea agregar a la lista</param>
        /// <returns>redirecciona al dashboard principal (metodo action index)</returns>
        [Authorize(Roles = "superadmin")]
        [HttpPost]
        public IActionResult Add(Usuario request)
        {
            bool agregarUsuario = _userService.AgregarUsuario(request);
            return agregarUsuario ? RedirectToAction("index") : View(request);
        }

        /// <summary>
        /// metodo actionencargado de mostrar la vista del formulario editar con los datos del usuario cargado
        /// </summary>
        /// <param name="id">el identificador del usuario</param>
        /// </summary>
        /// <returns>retorna el cliente a la vista</returns>
        [Authorize(Roles = "superadmin")]
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["ListarEmpleados"] = await _userService.ListarEmpleados();
            ViewData["ListarRoles"] = await _userService.ListarRoles();
            UsuarioViewModel usuarioEditar = await _userService.GetUsuario(id);
            return View(usuarioEditar);
        }

        /// <summary>
        /// metodo action el cual es recibido por el formulario de la vista en metodo POST el cual trae en la request todos los datos 
        /// del usuario a editar de la lista
        /// </summary>
        /// <param name="request">el cliente el cual cuenta con los nuevos datos</param>
        /// </summary>
        /// <returns>redirecciona al dashboard principal (metodo action index)</returns>

        [Authorize(Roles = "superadmin")]
        [HttpPost]
        public async Task<IActionResult> Edit(UsuarioViewModel request)
        {
            bool actualizarUsuario = await _userService.EditarUsuario(request);
            return actualizarUsuario ? RedirectToAction("index") : View(request);
        }


        /// <summary>
        /// metodo action encargado de mostrar una vista con todos los datos del cliente que se quiere 
        /// eliminar, con el fin de darle la oportunidad al usuario  si quiere confirmar la eliminacion del cliente
        /// </summary>
        /// <param name="id">el numero identificador que posee el cliente</param>
        /// <returns></returns>
        [Authorize(Roles = "superadmin")]
        public async Task<IActionResult> Delete(int id)
        {
            UsuarioViewModel usuarioEliminar = await _userService.GetUsuario(id);
            return View(usuarioEliminar);
        }

        /// <summary>
        /// metodo action el cual es recibido por el formulario de la vista en metodo POST el cual trae en la request todos los datos 
        /// del cliente a eliminar de la lista
        /// </summary>
        /// <param name="request">el cliente a eliminar con todos sus datos</param>
        /// <returns>redirecciona al dashboard principal (metodo action index)</returns>
        [HttpPost]
        [Authorize(Roles = "superadmin")]
        public async Task<IActionResult> Delete(UsuarioViewModel request)
        {
            bool usuarioEliminado = await _userService.EliminarUsuarioLogico(request);
            return usuarioEliminado ? RedirectToAction("index") : View(request);
        }

        /// <summary>
        /// metodo action encargado de mostrar la lista de cliente que fueron eliminados o que tienen un estado inactivo en el sistema
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "superadmin")]
        public async Task<IActionResult> Recover()
        {
            List<UsuarioViewModel> clientesEliminados = await _userService.ListarUsuarios(UsuarioViewModel.Estado.Eliminado);
            return View(clientesEliminados);
        }

        /// <summary>
        /// metodo action el cual es recibido por el formulario de la vista en metodo POST el cual trae en la request todos los datos 
        /// del cliente del cual se quiere incorporar nuevamente al sistema
        /// </summary>
        /// <param name="request">el cliente a recuperar con todos sus datos</param>
        /// <returns>redirecciona al dashboard principal (metodo action index)</returns>
        [Authorize(Roles = "superadmin")]
        [HttpPost]
        public async Task<IActionResult> Recover(UsuarioViewModel request)
        {
            bool usuarioRecuperar = await _userService.RecuperarUsuario(request);
            return usuarioRecuperar ? RedirectToAction("Index") : View();
        }

        /// <summary>
        /// metodo action encargado de mostrar la lista de cliente que fueron eliminados o que tienen un estado inactivo en el sistema
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "superadmin")]
        public async Task<IActionResult> RegisterRequest()
        {
            List<UsuarioViewModel> usuariosRegistrados = await _userService.ListarUsuariosPendientes(UsuarioViewModel.Estado.Pendiente);
            return View(usuariosRegistrados);
        }

        /// <summary>
        /// metodo action encargado de eliminar definitivamente un empleado registrado de la lista
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "superadmin")]
        public async Task<IActionResult> DeleteDefinitive(int id)
        {
            bool usuarioEliminado = await _userService.EliminarDefinitivo(id);
            return usuarioEliminado ? RedirectToAction("Index") : View();
        }

        /// <summary>
        /// metodo action encargado de eliminar definitivamente un empleado registrado de la lista
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "superadmin")]
        public async Task<IActionResult> AllowAccess(int id)
        {
            bool usuarioEliminado = await _userService.IntegrarUsuarioSistema(id);
            return usuarioEliminado ? RedirectToAction("Index") : View();
        }

    }
}
