using AlmacenTecnologico.Context;
using AlmacenTecnologico.Models;
using AlmacenTecnologico.Models.ViewModel;
using AlmacenTecnologico.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.PortableExecutable;

namespace AlmacenTecnologico.Controllers
{
    [Authorize(Roles = "admin,superadmin")]
    public class EmployeeController : Controller
    {
        private readonly IPersonaService _personaService;

        public EmployeeController(IPersonaService personaService)
        {
            this._personaService = personaService;
        }
        /// <summary>
        /// metodo action encargado de mostrar todos los empleados del sistema que esten activos
        /// </summary>
        /// <returns>retorna la lista de todos los empleados activos</returns>
        public async Task<IActionResult> Index()
        {
            List<EmpleadoViewModel> listaEmpleado = await _personaService.ListarEmpleados();
            return View(listaEmpleado);
        }


        /// <summary>
        /// metodo action encargado de mostrar el formulario de carga de empleados
        /// </summary>
        /// <returns></returns>
        public IActionResult Add()
        {
            return View();
        }

        /// <summary>
        /// metodo action el cual es recibido por el formulario de la vista en metodo POST el cual trae en la request todos los datos 
        /// del nuevo Empleado a agregar a la lista
        /// </summary>
        /// <param name="request">el empleado que se desea agregar a la lista</param>
        /// <returns>redirecciona al dashboard principal (metodo action index)</returns>

        [HttpPost]
        public async Task<IActionResult> Add(Persona request)
        {
            bool agregarEmpleado = await _personaService.AgregarEmpleado(request);
            return agregarEmpleado ? RedirectToAction("Index") : View(request);
        }

        /// <summary>
        /// metodo actionencargado de mostrar la vista del formulario editar con los datos del empleado cargado
        /// </summary>
        /// <param name="id">el identificador del empleado</param>
        /// </summary>
        /// <returns>retorna el empleado a la vista</returns>
        public async Task<IActionResult> Edit(int id)
        {
            EmpleadoViewModel empleado = await _personaService.GetEmpleado(id);
            return View(empleado);
        }
        /// <summary>
        /// metodo action el cual es recibido por el formulario de la vista en metodo POST el cual trae en la request todos los datos 
        /// del empleado a editar de la lista
        /// </summary>
        /// <param name="request">el empleado el cual cuenta con los nuevos datos</param>
        /// </summary>
        /// <returns>redirecciona al dashboard principal (metodo action index)</returns>
        [HttpPost]
        public async Task<IActionResult> Edit(EmpleadoViewModel request)
        {
            bool actulizarEmpleado = await _personaService.EditarEmpleado(request);
            return actulizarEmpleado == true ? RedirectToAction("Index") : View(request);
        }

        /// <summary>
        /// metodo action encargado de mostrar una vista con todos los datos del empleado que se quiere 
        /// eliminar, con el fin de darle la oportunidad al usuario  si quiere confirmar la eliminacion del empleado
        /// </summary>
        /// <param name="id">el numero identificador que posee el empleado</param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(int id)
        {
            EmpleadoViewModel empleado = await _personaService.GetEmpleado(id);
            return View(empleado);
        }
        /// <summary>
        /// metodo action el cual es recibido por el formulario de la vista en metodo POST el cual trae en la request todos los datos 
        /// del empleado a eliminar de la lista
        /// </summary>
        /// <param name="request">el empleado a eliminar con todos sus datos</param>
        /// <returns>redirecciona al dashboard principal (metodo action index)</returns>
        [HttpPost]
        public async Task<IActionResult> Delete(EmpleadoViewModel request)
        {
            bool EmpleadoEliminar = await _personaService.EliminarEmpleadoLogico(request);
            return EmpleadoEliminar ? RedirectToAction("Index") : View(request);
        }

        /// <summary>
        /// metodo action encargado de mostrar la lista de empleados que fueron eliminados o que tienen un estado inactivo en el sistema
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Recover()
        {
            List<EmpleadoViewModel> empleadosEliminados = await _personaService.ListarEmpleadosEliminados();
            return View(empleadosEliminados);
        }

        /// <summary>
        /// metodo action el cual es recibido por el formulario de la vista en metodo POST el cual trae en la request todos los datos 
        /// del empleado del cual se quiere incorporar nuevamente al sistema
        /// </summary>
        /// <param name="request">el empleado a recuperar con todos sus datos</param>
        /// <returns>redirecciona al dashboard principal (metodo action index)</returns>
        [HttpPost]
        public async Task<IActionResult> Recover(EmpleadoViewModel request)
        {
            bool EmpleadoRecuperar = await _personaService.RecuperarEmpleado(request);
            return EmpleadoRecuperar ? RedirectToAction("Index") : View(request);
        }

    }
}
