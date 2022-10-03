using AlmacenTecnologico.Context;
using AlmacenTecnologico.Models;
using AlmacenTecnologico.Models.ViewModel;
using AlmacenTecnologico.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AlmacenTecnologico.Services
{
    public class FabricanteService : IFabricantesServces
    {
        private readonly TrabajoFinalContext context;

        public FabricanteService(TrabajoFinalContext context)
        {
            this.context = context;
        }

        /// <summary>
        ///  Trae una lista de fabricantes de la base de datos
        /// </summary>
        /// <returns>la lista de fabricantes</returns>
        public async Task<List<FabricanteViewModel>> ListarFabricantes()
        {
            var lista = await (from fabricante in context.Fabricantes
                               where fabricante.EstadoId != 0
                               select new FabricanteViewModel
                               {
                                   Id = fabricante.Id,
                                   Nombre = fabricante.Nombre,
                                   EstadoId = fabricante.EstadoId
                               }).ToListAsync();
            return lista;
        }

        /// <summary>
        ///  Trae una lista de fabricantes de la base de datos
        /// </summary>
        /// <returns>la lista de fabricantes</returns>
        public async Task<List<FabricanteViewModel>> ListarFabricantesEliminados()
        {
            var lista = await (from fabricante in context.Fabricantes
                               where fabricante.EstadoId == 0
                               select new FabricanteViewModel
                               {
                                   Id = fabricante.Id,
                                   Nombre = fabricante.Nombre,
                                   EstadoId = fabricante.EstadoId
                               }).ToListAsync();
            return lista;
        }

        /// <summary>
        /// metodo encargado de traer Un solo fabricante de la lista que corresponda con el que le es pasado por parametro
        /// </summary>
        /// <param name="id">el id del fabricante</param>
        /// <returns></returns>
        public async Task<FabricanteViewModel> GetFabricante(int id)
        {
            var fabricanteEliminar = await (from fabricante in context.Fabricantes
                                            where fabricante.Id == id
                                            select new FabricanteViewModel
                                            {
                                                Id = fabricante.Id,
                                                Nombre = fabricante.Nombre,
                                                EstadoId = fabricante.EstadoId
                                            }).FirstAsync();
            return fabricanteEliminar;
        }

        /// <summary>
        /// metodo encargado de editar el fabricante que recibe por parametro en la base de datos
        /// </summary>
        /// <param name="context">el contexto de la base de datos</param>
        /// <param name="request">el fabricante el cual se quieren modificar los datos</param>
        /// <returns></returns>
        public async Task<bool> EditarFabricante(FabricanteViewModel request)
        {
            if (request != null)
            {
                Fabricante fabricanteModificar = await context.Fabricantes.FindAsync(request.Id);
                fabricanteModificar.Nombre = request.Nombre;
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public bool AgregarFabricante(Fabricante request)
        {
            if(request is not null)
            {
                request.EstadoId = 1;
                context.Add(request);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<bool> EliminarFabricanteLogico(FabricanteViewModel request)
        {
            if (request is not null)
            {
                Fabricante fabricanteEliminar = await context.Fabricantes.FindAsync(request.Id);
                fabricanteEliminar.EstadoId = 0;
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> RecuperarCliente(FabricanteViewModel request)
        {
            if (request is not null)
            {
                Fabricante fabricanteEliminado = await context.Fabricantes.FindAsync(request.Id);
                fabricanteEliminado.EstadoId = 1;//recuperado
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
