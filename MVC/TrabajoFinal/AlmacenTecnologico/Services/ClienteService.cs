using AlmacenTecnologico.Context;
using AlmacenTecnologico.Models;
using AlmacenTecnologico.Models.ViewModel;
using AlmacenTecnologico.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AlmacenTecnologico.Services
{
    public class ClienteService : IClienteService
    {
        private readonly TrabajoFinalContext context;

        public ClienteService(TrabajoFinalContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// metodo encargado de retornar la lista de clientes activos en el sistema
        /// </summary>
        /// <param name="context">el contexto de la base de datos</param>
        /// <returns></returns>
        public async Task<List<ClienteViewModel>> ListarClientes()
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
        public async Task<List<ClienteViewModel>> ListarClientesEliminados()
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
        public async Task<ClienteViewModel> GetCliente(int id)
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
        /// metodo encargado de agregar un cliente a la lista
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool AgregarCliente(Cliente request)
        {
            if (request is not null)
            {
                request.EstadoId = 1;
                this.context.Clientes.Add(request);
                this.context.SaveChanges();
                return true;
            }
            return false;
        }

        /// <summary>
        /// metodo encargado de editar un cliente de la base de datos, recibe los datos que se le quieren aplicar al cliente (request)
        /// </summary>
        /// <param name="context">el contexto de la base de datos</param>
        /// <param name="request">los datos que recibe de la peticion de tipo POST del formulario editar</param>
        /// <returns>retorna true si pudo modificar al cliente</returns>
        public async Task<bool> EditarCliente(ClienteViewModel request)
        {
            if (request is not null)
            {
                Cliente clienteEditar = await context.Clientes.FindAsync(request.Id);
                clienteEditar.RazonSocial = request.RazonSocial;
                clienteEditar.Cuit = request.Cuit;
                clienteEditar.Domicilio = request.Domicilio;
                clienteEditar.Mail = request.Mail;
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<bool> EliminarClienteLogico(ClienteViewModel request)
        {
            if(request is not null)
            {
                Cliente clienteEliminado = await context.Clientes.FindAsync(request.Id);
                clienteEliminado.EstadoId = 0;
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }


        public async Task<bool> RecuperarCliente(ClienteViewModel request)
        {
            if (request is not null)
            {
                Cliente clienteEliminado = await context.Clientes.FindAsync(request.Id);
                clienteEliminado.EstadoId = 1;//recuperado
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
