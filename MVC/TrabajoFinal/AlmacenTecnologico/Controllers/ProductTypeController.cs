using AlmacenTecnologico.Context;
using AlmacenTecnologico.Models;
using AlmacenTecnologico.Models.ViewModel;
using AlmacenTecnologico.Services;
using AlmacenTecnologico.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlmacenTecnologico.Controllers
{
    [Authorize(Roles = "superadmin,admin,empleado")]
    public class ProductTypeController : Controller
    {
        private readonly ITipoProductoService _tProductoService;

        public ProductTypeController(ITipoProductoService tProductoService)
        {
            this._tProductoService = tProductoService;
        }

        /// <summary>
        /// metodo action encargado de mostrar el dashboard principal con todos los tipos de productos con su categoria correspondiente
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            List<TipoProductoViewModel> lista = await _tProductoService.ListarTipoProductos();
            return View(lista);
        }

        /// <summary>
        /// metodo action encargado de mostrar la vista de formulario de carga de tipos de producto,
        /// la lista de categorias sera utilizada para cargar el dropdownlist que se encuentra en el formulario
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Add()
        {
            ViewData["ListaCategorias"] = await _tProductoService.ListarCategorias();
            return View();
        }

        /// <summary>
        /// metodo action el cual es recibido por el formulario de la vista en metodo POST el cual trae en la request todos los datos 
        /// del nuevo tipo de proucto a agregar a la lista
        /// </summary>
        /// <param name="request">el fabricante que se desea agregar a la vista</param>
        /// <returns>redirecciona al dashboard principal (metodo action index)</returns>
        [HttpPost]
        public IActionResult Add(TipoProducto request)
        {
            bool agregarProducto = _tProductoService.AgregarTipoProducto(request);
            return agregarProducto ?  RedirectToAction("Index") : View(request);
        }

        [Authorize(Roles = "superadmin,admin")]
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["ListaCategorias"] = await _tProductoService.ListarCategorias();
            TipoProductoViewModel tipoProducto = await _tProductoService.GetTipoProducto(id);
            return View(tipoProducto);
        }

        [Authorize(Roles = "superadmin,admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(TipoProductoViewModel request)
        {
            bool editarProducto = await _tProductoService.EditarTipoProducto(request);
            return editarProducto ? RedirectToAction("Index") : View(request);
        }

        [Authorize(Roles = "admin,superadmin")]
        public async Task<IActionResult> Delete(int id)
        {
            TipoProductoViewModel productoEliminar = await _tProductoService.GetTipoProducto(id);
            return productoEliminar is not null ?  View(productoEliminar): RedirectToAction("index");
        }

       
        [HttpPost]
        [Authorize(Roles = "admin,superadmin")]
        public async Task<IActionResult> Delete(TipoProductoViewModel request)
        {
            bool eliminarProducto = await _tProductoService.EliminarTipoProductoLogico(request);
            return eliminarProducto ? RedirectToAction("index") : View(request);
        }

        
        [Authorize(Roles = "admin,superadmin")]
        public async Task<IActionResult> Recover()
        {
            List<TipoProductoViewModel> productosEliminados = await _tProductoService.ListarTipoProductosEliminados();
            return View(productosEliminados);
        }

        
        [Authorize(Roles = "admin,superadmin")]
        [HttpPost]
        public async Task<IActionResult> Recover(TipoProductoViewModel request)
        {
            bool recuperartProducto = await _tProductoService.RecuperarTipoProducto(request);
            return recuperartProducto ? RedirectToAction("index") : View();
        }
    }
}
