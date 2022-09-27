using AlmacenTecnologico.Context;
using AlmacenTecnologico.Models;
using AlmacenTecnologico.Models.ViewModel;
using AlmacenTecnologico.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AlmacenTecnologico.Services
{
    public class ProductoService : IProductoService
    {
        /// <summary>
        /// metodo encargado de traer todos los productos desde la base de datos
        /// </summary>
        /// <param name="context">contexto de la base de datos</param>
        /// <returns>retorna una lista de productos</returns>
        public async Task<List<ProductoViewModel>> ListarProductos(TrabajoFinalContext context)
        {
            var lista = await (from producto in context.Productos
                               where producto.EstadoId != 0
                               select new ProductoViewModel
                               {
                                   Id = producto.Id,
                                   Modelo = producto.Modelo,
                                   Precio = producto.Precio,
                                   UrlImagen = producto.UrlImagen,
                                   EstadoId = producto.EstadoId,
                                   Cantidad = producto.CantidadStock,
                                   UnidadesDisponibles = producto.UnidadesDisponibles,
                                   IdFabricante = producto.IdFabricante,
                                   IdTipoProducto = producto.IdTipoProducto,
                                   Fabricante = producto.IdFabricanteNavigation,
                                   TipoProducto = producto.IdTipoProductoNavigation
                               }).ToListAsync();

            return lista;
        }

        /// <summary>
        /// metodo encargado de retornar la lista de productos eliminados/inactivos del sistema
        /// </summary>
        /// <param name="context">el contexto de la base de datos</param>
        /// <returns></returns>
        public async Task<List<ProductoViewModel>> ListarProductosEliminados(TrabajoFinalContext context)
        {
            var lista = await (from producto in context.Productos
                               where producto.EstadoId == 0
                               select new ProductoViewModel
                               {
                                   Id = producto.Id,
                                   Modelo = producto.Modelo,
                                   Precio = producto.Precio,
                                   UrlImagen = producto.UrlImagen,
                                   EstadoId = producto.EstadoId,
                                   Cantidad = producto.CantidadStock,
                                   IdFabricante = producto.IdFabricante,
                                   IdTipoProducto = producto.IdTipoProducto,
                                   Fabricante = producto.IdFabricanteNavigation,
                                   TipoProducto = producto.IdTipoProductoNavigation
                               }).ToListAsync();

            return lista;
        }


        /// <summary>
        ///  metodo encargador de traer todos los tipos de producto de la base de datos y convertilos 
        ///  en un tipo SelectListItem que otroga al objecto una propiedad clave=>valor lo cual permitira
        ///  cargar dinamicamente los dropdown de el formulario de carga y edición
        /// </summary>
        /// <param name="context">el contexto de la base de datos</param>
        /// <returns></returns>
        public async Task<List<SelectListItem>> ListarTipoProducto(TrabajoFinalContext context)
        {
            var lista = await (from tipoProducto in context.TipoProductos
                               select new SelectListItem
                               {
                                   Text = tipoProducto.Nombre,
                                   Value = tipoProducto.Id.ToString()
                               }).ToListAsync();
            return lista;
        }

        /// <summary>
        ///  metodo encargador de traer todos los fabricantes de producto de la base de datos y convertilos 
        ///  en un tipo SelectListItem que otroga al objecto una propiedad clave=>valor lo cual permitira
        ///  cargar dinamicamente los dropdown de el formulario de carga y edición
        /// </summary>
        /// <param name="context">el contexto de la base de datos</param>
        /// <returns></returns>
        public async Task<List<SelectListItem>> ListarFabricantes(TrabajoFinalContext context)
        {
            var lista = await (from fabricantes in context.Fabricantes
                               where fabricantes.Id != 0
                               select new SelectListItem
                               {
                                   Text = fabricantes.Nombre,
                                   Value = fabricantes.Id.ToString()
                               }).ToListAsync();
            return lista;
        }

        /// <summary>
        /// metodo encargado de devolver el producto con todos sus datos el cual tenga el mismo indentificado que le pasan por parametro
        /// </summary>
        /// <param name="id">el identificador del producto</param>
        /// <param name="context">el contexto de la base de datos</param>
        /// <returns></returns>
        public async Task<ProductoViewModel> GetProducto(int id, TrabajoFinalContext context)
        {
            var producto = await (from product in context.Productos
                                  where product.Id == id
                                  select new ProductoViewModel
                                  {
                                      Id = product.Id,
                                      Modelo = product.Modelo,
                                      Precio = product.Precio,
                                      UrlImagen = product.UrlImagen,
                                      EstadoId = product.EstadoId,
                                      Cantidad = product.CantidadStock,
                                      UnidadesDisponibles = product.UnidadesDisponibles,
                                      IdFabricante = product.IdFabricante,
                                      IdTipoProducto = product.IdTipoProducto,
                                      Fabricante = product.IdFabricanteNavigation,
                                      TipoProducto = product.IdTipoProductoNavigation

                                  }).FirstAsync();
            return producto;
        }

        /// <summary>
        /// metodo encargado de editar un producto de la base de datos, recibe los datos que se le quieren aplicar al producto (request)
        /// </summary>
        /// <param name="context">el contexto de la base de datos</param>
        /// <param name="request">los datos que recibe de la peticion de tipo POST del formulario editar</param>
        /// <returns>retorna true si pudo modificar al producto</returns>
        public async Task<bool> EditarProducto(ProductoViewModel request, TrabajoFinalContext context)
        {
            if(request is not null)
            {
                Producto productoEditar = await context.Productos.FindAsync(request.Id);
                productoEditar.Modelo = request.Modelo;
                productoEditar.Precio = request.Precio;
                productoEditar.UrlImagen = request.UrlImagen;
                productoEditar.CantidadStock = request.Cantidad;
                productoEditar.IdTipoProducto = request.IdTipoProducto;
                productoEditar.IdFabricante = request.IdFabricante;
                return true;
            }
            return false;
        }
    }
}
