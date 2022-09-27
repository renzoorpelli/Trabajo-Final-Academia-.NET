using AlmacenTecnologico.Context;
using AlmacenTecnologico.Models.ViewModel;
using AlmacenTecnologico.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AlmacenTecnologico.Services
{
    public class TipoProductoService : ITipoProductoService
    {
        /// <summary>
        /// metodo encargado de traer todos los tipos de productos de la base de datos, cada uno de estos tiene una relacion con la categoria
        /// </summary>
        /// <param name="context">el contexto de la base de datos</param>
        /// <returns>retorna una lista con todos los elementos</returns>
        public async Task<List<TipoProductoViewModel>> ListarTipoProductos(TrabajoFinalContext context)
        {
            var lista = await (from tipoProductos in context.TipoProductos
                               select new TipoProductoViewModel
                               {
                                   Id = tipoProductos.Id,
                                   Nombre = tipoProductos.Nombre,
                                   Categoria = tipoProductos.IdCategoriaNavigation,
                                   NombreCategoria = tipoProductos.IdCategoriaNavigation.Nombre
                               }).ToListAsync();
            return lista;
        }

        /// <summary>
        /// metodo encargado de seleccionar todas las cateegorias de productos de la base de datos conviertiendola en una lista de tipo SelecListItem que 
        /// permite darle la funcionalidad a cada elemento de la lista en un objeto clave valor dicha funcionalidad sera utilizada para un dropdownlist
        /// que se cargara dinamicamente depende los resultados de este metodo
        /// </summary>
        /// <param name="context">el contexto de la base de datos</param>
        /// <returns>retorna una lista con todos los elementos</returns>
        public async Task<List<SelectListItem>> ListarCategorias(TrabajoFinalContext context)
        {
            var lista = await (from categoria in context.Categoria
                               select new SelectListItem
                               {
                                   Text = categoria.Nombre,
                                   Value = categoria.Id.ToString(),
                               }).ToListAsync();
            return lista;
        }

    }
}
