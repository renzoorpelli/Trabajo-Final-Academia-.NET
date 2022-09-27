using AlmacenTecnologico.Context;
using AlmacenTecnologico.Models;
using AlmacenTecnologico.Models.ViewModel;
using AlmacenTecnologico.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AlmacenTecnologico.Services
{
    public class ClienteService : IClienteService
    {
        /// <summary>
        /// metodo encargado de retornar la lista de clientes activos en el sistema
        /// </summary>
        /// <param name="context">el contexto de la base de datos</param>
        /// <returns></returns>
        public async Task<List<ClienteViewModel>> ListarClientes(TrabajoFinalContext context)
        {
            var lista = await (from cliente in context.Clientes
                               where cliente.EstadoId != 0
                               select new ClienteViewModel
                               {
                                   Id = cliente.Id,
                                   RazonSocial = cliente.RazonSocial,
                                   Cuit = cliente.Cuit,
                                   Domicilio = cliente.Domicilio,
                                   Mail = cliente.Mail,
                                   EstadoId = cliente.EstadoId

                               }).ToListAsync();
            return lista;
        }
        /// <summary>
        /// metodo encargado de retornar la lista de clientes eliminados/inactivos del sistema
        /// </summary>
        /// <param name="context">el contexto de la base de datos</param>
        /// <returns></returns>
        public async Task<List<ClienteViewModel>> ListarClientesEliminados(TrabajoFinalContext context)
        {
            var lista = await (from cliente in context.Clientes
                               where cliente.EstadoId == 0
                               select new ClienteViewModel
                               {
                                   Id = cliente.Id,
                                   RazonSocial = cliente.RazonSocial,
                                   Cuit = cliente.Cuit,
                                   Domicilio = cliente.Domicilio,
                                   Mail = cliente.Mail,
                                   EstadoId = cliente.EstadoId

                               }).ToListAsync();
            return lista;
        }
        /// <summary>
        /// metodo encargado de devolver el cliente con todos sus datos el cual tenga el mismo indentificado que le pasan por parametro
        /// </summary>
        /// <param name="id">el identificador del cliente</param>
        /// <param name="context">el contexto de la base de datos</param>
        /// <returns></returns>
        public async Task<ClienteViewModel> GetCliente(int id, TrabajoFinalContext context)
        {
            var clienteSeleccionado = await (from cliente in context.Clientes
                                             where cliente.Id == id
                                             select new ClienteViewModel
                                             {
                                                 Id = cliente.Id,
                                                 RazonSocial = cliente.RazonSocial,
                                                 Cuit = cliente.Cuit,
                                                 Domicilio = cliente.Domicilio,
                                                 Mail = cliente.Mail

                                             }).FirstAsync();
            return clienteSeleccionado;
        }

        /// <summary>
        /// metodo encargado de editar un cliente de la base de datos, recibe los datos que se le quieren aplicar al cliente (request)
        /// </summary>
        /// <param name="context">el contexto de la base de datos</param>
        /// <param name="request">los datos que recibe de la peticion de tipo POST del formulario editar</param>
        /// <returns>retorna true si pudo modificar al cliente</returns>
        public async Task<bool> EditarCliente(ClienteViewModel request, TrabajoFinalContext context)
        {
            if(request is not null)
            {
                Cliente clienteEditar = await context.Clientes.FindAsync(request.Id);
                clienteEditar.RazonSocial = request.RazonSocial;
                clienteEditar.Cuit = request.Cuit;
                clienteEditar.Domicilio = request.Domicilio;
                clienteEditar.Mail = request.Mail;
                return true;
            }
            return false;
        }
    }
}
