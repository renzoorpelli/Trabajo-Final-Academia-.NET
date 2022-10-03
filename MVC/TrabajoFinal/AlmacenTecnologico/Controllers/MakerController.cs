using AlmacenTecnologico.Context;
using AlmacenTecnologico.Models;
using AlmacenTecnologico.Models.ViewModel;
using AlmacenTecnologico.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlmacenTecnologico.Controllers
{
    [Authorize(Roles= "superadmin,admin,empleado")]
    public class MakerController : Controller
    {
        private readonly IFabricantesServces _fabricanteServices;

        public MakerController(IFabricantesServces fabricantesServices)
        {
            _fabricanteServices = fabricantesServices;
        }

        /// <summary>
        /// metodo action encargado de mostrar el dashboard principal del controlador con la lista de fabricantes disponibles
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            List<FabricanteViewModel> fabricantes = await _fabricanteServices.ListarFabricantes();
            return View(fabricantes);
        }

        /// <summary>
        /// metodo action encargado de mostrar el formulario de carga de fabricantes 
        /// </summary>
        /// <returns></returns>
        public IActionResult Add()
        {
            return View();
        }

        /// <summary>
        /// metodo action el cual es recibido por el formulario de la vista en metodo POST el cual trae en la request todos los datos 
        /// del nuevo Fabricante a agregar a la lista
        /// </summary>
        /// <param name="request">el fabricante que se desea agregar a la lista</param>
        /// <returns>redirecciona al dashboard principal (metodo action index)</returns>
        [HttpPost]
        public IActionResult Add(Fabricante request)
        {
            bool agregarFabricante = _fabricanteServices.AgregarFabricante(request);
            return agregarFabricante ? RedirectToAction("Index") : View(request);
        }


        /// <summary>
        ///metodo encargado de mostrar la vista del formulario editar con los datos del fabricante cargado
        /// </summary>
        /// <param name="id">el identificador del fabricante</param>
        /// <returns>retorna el fabricante a la vista</returns>
        [Authorize(Roles = "admin,superadmin")]
        public async Task<IActionResult> Edit(int id)
        {
            FabricanteViewModel fabricante = await _fabricanteServices.GetFabricante(id);
            return View(fabricante);
        }


        /// <summary>
        /// metodo action el cual es recibido por el formulario de la vista en metodo POST el cual trae en la request todos los datos 
        /// del Fabricante a editar de la lista
        /// </summary>
        /// <param name="request">el fabricante el cual cuenta con los nuevos datos</param>
        /// <returns>redirecciona al dashboard principal (metodo action index)</returns>
        [Authorize(Roles = "admin,superadmin")]
        [HttpPost]
        public async Task<IActionResult> Edit(FabricanteViewModel request)
        {
            bool actulizarFabricante = await _fabricanteServices.EditarFabricante(request);
            return actulizarFabricante ? RedirectToAction("Index") : View(request);
        }

        /// <summary>
        /// metodo action encargado de mostrar una vista con todos los datos del fabricante que se quiere 
        /// eliminar, con el fin de darle la oportunidad al usuario  si quiere confirmar la eliminacion del fabricante
        /// </summary>
        /// <param name="id">el numero identificador que posee el fabricante</param>
        /// <returns></returns>
        [Authorize(Roles = "admin,superadmin")]
        public async Task<IActionResult> Delete(int id)
        {
            FabricanteViewModel fabricante = await _fabricanteServices.GetFabricante(id);
            return View(fabricante);
        }

        /// <summary>
        /// metodo action el cual es recibido por el formulario de la vista en metodo POST el cual trae en la request todos los datos 
        /// del Fabricante a eliminar de la lista
        /// </summary>
        /// <param name="request">el fabricante a eliminar con todos sus datos</param>
        /// <returns>redirecciona al dashboard principal (metodo action index)</returns>
        [Authorize(Roles = "admin,superadmin")]
        [HttpPost]
        public async Task<IActionResult> Delete(FabricanteViewModel request)
        {
            bool eliminarFabricante = await _fabricanteServices.EliminarFabricanteLogico(request);
            return eliminarFabricante  ? RedirectToAction("Index") : View(request);
        }

        /// <summary>
        /// metodo action encargado de mostrar la lista de fabricantes que fueron eliminados o que tienen un estado inactivo en el sistema
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "admin,superadmin")]
        public async Task<IActionResult> Recover()
        {
            List<FabricanteViewModel> fabricantesEliminados = await _fabricanteServices.ListarFabricantesEliminados();
            return View(fabricantesEliminados);
        }

        /// <summary>
        /// metodo action el cual es recibido por el formulario de la vista en metodo POST el cual trae en la request todos los datos 
        /// del Fabricante del cual se quiere incorporar nuevamente al sistema
        /// </summary>
        /// <param name="request">el fabricante a recuperar con todos sus datos</param>
        /// <returns>redirecciona al dashboard principal (metodo action index)</returns>
        [Authorize(Roles = "admin,superadmin")]
        [HttpPost]
        public async Task<IActionResult> Recover(FabricanteViewModel request)
        {
            bool recuperarFabricante = await _fabricanteServices.RecuperarCliente(request);
            return recuperarFabricante ? RedirectToAction("Index") : View(request);
        }

    }
}
