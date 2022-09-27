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
    public class MakerController : Controller
    {
        private readonly TrabajoFinalContext _context;
        private readonly IFabricantesServces _fabricantesServces;

        public MakerController(TrabajoFinalContext context, IFabricantesServces fabricantesServces)
        {
            _context = context;
            _fabricantesServces = fabricantesServces;
        }

        /// <summary>
        /// metodo action encargado de mostrar el dashboard principal del controlador con la lista de fabricantes disponibles
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            List<FabricanteViewModel> fabricantes = await _fabricantesServces.ListarFabricantes(_context);
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
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            request.EstadoId = 1;
            _context.Add(request);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }


        /// <summary>
        ///metodo encargado de mostrar la vista del formulario editar con los datos del fabricante cargado
        /// </summary>
        /// <param name="id">el identificador del fabricante</param>
        /// <returns>retorna el fabricante a la vista</returns>
        [Authorize(Roles = "admin,superadmin")]
        public async Task<IActionResult> Edit(int id)
        {
            FabricanteViewModel fabricante = await _fabricantesServces.GetFabricante(_context, id);
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
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            bool actulizarFabricante = await _fabricantesServces.EditarFabricante(_context, request);
            await _context.SaveChangesAsync();
            return actulizarFabricante == true ? RedirectToAction("Index") : StatusCode(404);
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
            FabricanteViewModel fabricante = await _fabricantesServces.GetFabricante(_context, id);
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
            Fabricante fabricanteEliminar = await _context.Fabricantes.FindAsync(request.Id);
            fabricanteEliminar.EstadoId = 0;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// metodo action encargado de mostrar la lista de fabricantes que fueron eliminados o que tienen un estado inactivo en el sistema
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "admin,superadmin")]
        public async Task<IActionResult> Recover()
        {
            List<FabricanteViewModel> fabricantesEliminados = await _fabricantesServces.ListarFabricantesEliminados(_context);
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
            Fabricante fabricanteEliminado = await _context.Fabricantes.FindAsync(request.Id);
            fabricanteEliminado.EstadoId = 1;//recuperado
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
