using AlmacenTecnologico.Context;
using AlmacenTecnologico.Models;
using AlmacenTecnologico.Models.ViewModel;
using AlmacenTecnologico.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AlmacenTecnologico.Services
{
    public class TipoProductoService : ITipoProductoService
    {
        private readonly TrabajoFinalContext context;

        public TipoProductoService(TrabajoFinalContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// metodo encargado de traer todos los tipos de productos de la base de datos, cada uno de estos tiene una relacion con la categoria
        /// </summary>
        /// <param name="context">el contexto de la base de datos</param>
        /// <returns>retorna una lista con todos los elementos</returns>
        public async Task<List<TipoProductoViewModel>> ListarTipoProductos()
        {
            var lista = await (from tipoProductos in context.TipoProductos
                               where tipoProductos.EstadoId != 0
                               select new TipoProductoViewModel
                               {
                                   Id = tipoProductos.Id,
                                   Nombre = tipoProductos.Nombre,
                                   Categoria = tipoProductos.IdCategoriaNavigation,
                                   NombreCategoria = tipoProductos.IdCategoriaNavigation.Nombre,
                                   EstadoId = tipoProductos.EstadoId
                               }).ToListAsync();
            return lista;
        }

        public async Task<List<TipoProductoViewModel>> ListarTipoProductosEliminados()
        {
            var lista = await (from tipoProductos in context.TipoProductos
                               where tipoProductos.EstadoId == 0
                               select new TipoProductoViewModel
                               {
                                   Id = tipoProductos.Id,
                                   Nombre = tipoProductos.Nombre,
                                   Categoria = tipoProductos.IdCategoriaNavigation,
                                   NombreCategoria = tipoProductos.IdCategoriaNavigation.Nombre,
                                   EstadoId = tipoProductos.EstadoId
                               }).ToListAsync();
            return lista;
        }

        public async Task<TipoProductoViewModel> GetTipoProducto(int id)
        {
            var tipoProducto = await (from tipoProductos in context.TipoProductos
                               where tipoProductos.Id == id
                               select new TipoProductoViewModel
                               {
                                   Id = tipoProductos.Id,
                                   Nombre = tipoProductos.Nombre,
                                   Categoria = tipoProductos.IdCategoriaNavigation,
                                   NombreCategoria = tipoProductos.IdCategoriaNavigation.Nombre,
                                   EstadoId = tipoProductos.EstadoId
                               }).FirstAsync();
            return tipoProducto;
        }

        /// <summary>
        /// metodo encargado de seleccionar todas las cateegorias de productos de la base de datos conviertiendola en una lista de tipo SelecListItem que 
        /// permite darle la funcionalidad a cada elemento de la lista en un objeto clave valor dicha funcionalidad sera utilizada para un dropdownlist
        /// que se cargara dinamicamente depende los resultados de este metodo
        /// </summary>
        /// <param name="context">el contexto de la base de datos</param>
        /// <returns>retorna una lista con todos los elementos</returns>
        public async Task<List<SelectListItem>> ListarCategorias()
        {
            var lista = await (from categoria in context.Categoria
                               select new SelectListItem
                               {
                                   Text = categoria.Nombre,
                                   Value = categoria.Id.ToString(),
                               }).ToListAsync();
            return lista;
        }

        public bool AgregarTipoProducto(TipoProducto request)
        {
            if (request is not null)
            {
                request.EstadoId = 1;
                context.TipoProductos.Add(request);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<bool> EditarTipoProducto(TipoProductoViewModel request)
        {
            if (request is not null)
            {
                TipoProducto tipoProductoEditar = await context.TipoProductos.FindAsync(request.Id);
                tipoProductoEditar.Nombre = request.Nombre;
                tipoProductoEditar.IdCategoria = request.IdCategoria;
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> EliminarTipoProductoLogico(TipoProductoViewModel request)
        {
            if (request is not null)
            {
                TipoProducto tipoProductoEditar = await context.TipoProductos.FindAsync(request.Id);
                tipoProductoEditar.EstadoId = 0;
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> RecuperarTipoProducto(TipoProductoViewModel request)
        {
            if (request is not null)
            {
                TipoProducto tipoProductoEditar = await context.TipoProductos.FindAsync(request.Id);
                tipoProductoEditar.EstadoId = 1;
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
