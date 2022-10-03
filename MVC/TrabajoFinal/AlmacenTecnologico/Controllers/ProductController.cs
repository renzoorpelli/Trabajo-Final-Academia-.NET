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
    public class ProductController : Controller
    {
        private readonly IProductoService _productoService;
        public ProductController(IProductoService productoService)
        {
            _productoService = productoService;
        }


        /// <summary>
        /// metodo action encargado de mostrar todos los productos del sistema que esten activos
        /// </summary>
        /// <returns>retorna la lista de todos los clientes activos</returns>
        public async Task<IActionResult> Index()
        {
            List<ProductoViewModel> productos = await _productoService.ListarProductos();
            return View(productos);
        }

        /// <summary>
        /// metodo action encargado de mostrar el formulario de carga de productos, los viewData
        /// permitiran la lista dinamica de tipos de productos y fabricantes al momento de la carga
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Add()
        {
            ViewData["TipoProducto"] = await _productoService.ListarTipoProducto();
            ViewData["Fabricantes"] = await _productoService.ListarFabricantes();
            return View();
        }

        /// <summary>
        /// metodo action el cual es recibido por el formulario de la vista en metodo POST el cual trae en la request todos los datos 
        /// del nuevo producto a agregar a la lista
        /// </summary>
        /// <param name="request">el producto que se desea agregar a la lista</param>
        /// <returns>redirecciona al dashboard principal (metodo action index)</returns>
        [HttpPost]
        public async Task<IActionResult> Add(Producto request)
        {
            bool agregarProducto = await _productoService.AgregarProducto(request);
            return agregarProducto ?  RedirectToAction("index") : View(request);
        }

        /// <summary>
        /// metodo actionencargado de mostrar la vista del formulario editar con los datos del producto cargado
        /// </summary>
        /// <param name="id">el identificador del producto</param>
        /// </summary>
        /// <returns>retorna el producto a la vista</returns>
        [Authorize(Roles = "admin,superadmin")]
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["TipoProducto"] = await _productoService.ListarTipoProducto();
            ViewData["Fabricantes"] = await _productoService.ListarFabricantes();
            ProductoViewModel productoEditar = await _productoService.GetProducto(id);
            return View(productoEditar);
        }

        /// <summary>
        /// metodo action el cual es recibido por el formulario de la vista en metodo POST el cual trae en la request todos los datos 
        /// del producto a editar de la lista
        /// </summary>
        /// <param name="request">el producto el cual cuenta con los nuevos datos</param>
        /// </summary>
        /// <returns>redirecciona al dashboard principal (metodo action index)</returns>

        [Authorize(Roles = "admin,superadmin")]
        [HttpPost]
        public async Task<IActionResult> Edit(ProductoViewModel request)
        {
            bool actualizarProducto = await _productoService.EditarProducto( request);
            return actualizarProducto ? RedirectToAction("index") : View(request);
        }


        /// <summary>
        /// metodo action encargado de mostrar una vista con todos los datos del producto que se quiere 
        /// eliminar, con el fin de darle la oportunidad al usuario  si quiere confirmar la eliminacion del producto
        /// </summary>
        /// <param name="id">el numero identificador que posee el producto</param>
        /// <returns></returns>
        [Authorize(Roles = "admin,superadmin")]
        public async Task<IActionResult> Delete(int id)
        {
            ProductoViewModel productoEliminar = await _productoService.GetProducto(id);
            return View(productoEliminar);
        }

        /// <summary>
        /// metodo action el cual es recibido por el formulario de la vista en metodo POST el cual trae en la request todos los datos 
        /// del producto a eliminar de la lista
        /// </summary>
        /// <param name="request">el producto a eliminar con todos sus datos</param>
        /// <returns>redirecciona al dashboard principal (metodo action index)</returns>
        [HttpPost]
        [Authorize(Roles = "admin,superadmin")]
        public async Task<IActionResult> Delete(ProductoViewModel request)
        {
            bool eliminarProducto = await _productoService.EliminarProductoLogico(request);
            return eliminarProducto ? RedirectToAction("index") : View(request);
        }

        /// <summary>
        /// metodo action encargado de mostrar la lista de productos que fueron eliminados o que tienen un estado inactivo en el sistema
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "admin,superadmin")]
        public async Task<IActionResult> Recover()
        {
            List<ProductoViewModel> productosEliminados = await _productoService.ListarProductosEliminados();
            return View(productosEliminados);
        }

        /// <summary>
        /// metodo action el cual es recibido por el formulario de la vista en metodo POST el cual trae en la request todos los datos 
        /// del producto del cual se quiere incorporar nuevamente al sistema
        /// </summary>
        /// <param name="request">el producto a recuperar con todos sus datos</param>
        /// <returns>redirecciona al dashboard principal (metodo action index)</returns>
        [Authorize(Roles = "admin,superadmin")]
        [HttpPost]
        public async Task<IActionResult> Recover(ProductoViewModel request)
        {
            bool recuperarProducto = await _productoService.RecuperarProducto(request);
            return recuperarProducto ? RedirectToAction("index") : View(request);
        }
    }
}
