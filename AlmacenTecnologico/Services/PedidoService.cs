using AlmacenTecnologico.Context;
using AlmacenTecnologico.Models;
using AlmacenTecnologico.Models.ViewModel;
using AlmacenTecnologico.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AlmacenTecnologico.Services
{
    public class PedidoService : IPedidoService
    {
        /// <summary>
        /// metodo encargado de listar todos los pedidos del sistemas que estan activos
        /// </summary>
        /// <param name="context">el contexto de la base de datos</param>
        /// <returns></returns>
        public async Task<List<PedidoViewModel>> ListarPedidos(TrabajoFinalContext context)
        {
            var lista = await (from pedido in context.Pedidos
                               where pedido.EstadoId != 0
                               select new PedidoViewModel
                               {
                                   Id = pedido.Id,
                                   EmpleadoId = pedido.EmpleadoId,
                                   ClienteId = pedido.ClienteId,
                                   FechaPedido = pedido.FechaPedido,
                                   EstadoId = pedido.EstadoId,
                                   Cliente = pedido.Cliente,
                                   Empleado = pedido.Empleado,
                                   DetallePedido = pedido.DetallePedidos

                               }).ToListAsync();
            return lista;
        }

        /// <summary>
        /// metodo encargado de listar todos los pedidos que fueron eliminados para que estos sean vistos
        /// en el panel recover y puedan ser recuperados
        /// </summary>
        /// <param name="context">el contexto de la base de datos</param>
        /// <returns></returns>
        public async Task<List<PedidoViewModel>> ListarPedidosEliminados(TrabajoFinalContext context)
        {
            var lista = await (from pedido in context.Pedidos
                               where pedido.EstadoId == 0
                               select new PedidoViewModel
                               {
                                   Id = pedido.Id,
                                   EmpleadoId = pedido.EmpleadoId,
                                   ClienteId = pedido.ClienteId,
                                   FechaPedido = pedido.FechaPedido,
                                   EstadoId = pedido.EstadoId,
                                   Cliente = pedido.Cliente,
                                   Empleado = pedido.Empleado,
                                   DetallePedido = pedido.DetallePedidos

                               }).ToListAsync();
            return lista;
        }
        /// <summary>
        /// metodo encargado de devolver la lista de Clientes disponibles en una lista tipo SelectListItem utilizada
        /// para los dropdownlist dinamicamente para que cada elemento tome un tipo clave=>valor
        /// solo se mostraran los Clientes activos del sistema
        /// </summary>
        /// <param name="context">el contexto de la base de datos</param>
        /// <returns></returns>
        public async Task<List<SelectListItem>> ListarClientes(TrabajoFinalContext context)
        {
            var lista = await (from clientes in context.Clientes
                               where clientes.EstadoId != 0
                               select new SelectListItem
                               {
                                   Text = clientes.RazonSocial,
                                   Value = clientes.Id.ToString()
                               }).ToListAsync();
            return lista;
        }

        /// <summary>
        /// metodo encargado de devolver la lista de Empleados disponibles en una lista tipo SelectListItem utilizada
        /// para los dropdownlist dinamicamente para que cada elemento tome un tipo clave=>valor
        /// solo se mostraran los empleados activos del sistema
        /// </summary>
        /// <param name="context">el contexto de la base de datos</param>
        /// <returns></returns>
        public async Task<List<SelectListItem>> ListarEmpleados(TrabajoFinalContext context)
        {
            var lista = await (from empleado in context.Personas
                               where empleado.EstadoId != 0
                               select new SelectListItem
                               {
                                   Text = empleado.Nombre + " " + empleado.Dni,
                                   Value = empleado.Id.ToString()
                               }).ToListAsync();
            return lista;
        }


        /// <summary>
        /// metodo encargado de traer un pedido de la base de datos el cual coincida con el id pasado por parametro
        /// </summary>
        /// <param name="id">el id del pedido</param>
        /// <param name="context">el contexto de la base de datos</param>
        /// <returns></returns>
        public async Task<PedidoViewModel> GetPedido(int id, TrabajoFinalContext context)
        {
            var producto = await (from pedido in context.Pedidos
                                  where pedido.Id == id
                                  select new PedidoViewModel
                                  {
                                      Id = pedido.Id,
                                      EmpleadoId = pedido.EmpleadoId,
                                      ClienteId = pedido.ClienteId,
                                      FechaPedido = pedido.FechaPedido,
                                      EstadoId = pedido.EstadoId,
                                      Cliente = pedido.Cliente,
                                      Empleado = pedido.Empleado,
                                      DetallePedido = pedido.DetallePedidos

                                  }).FirstAsync();
            return producto;
        }

        /// <summary>
        /// metodo encargado de editar los datos del pedido generado
        /// </summary>
        /// <param name="request">el pedido con los datos actualizados proveniente del metodo action POST Edit</param>
        /// <param name="context">el contexto de la base de datos</param>
        /// <returns>true si pudo editarlo, false de lo contrario</returns>
        public async Task<bool> EditarPedido(PedidoViewModel request, TrabajoFinalContext context)
        {
            if (request is not null)
            {
                Pedido pedidoEditar = await context.Pedidos.FindAsync(request.Id);
                pedidoEditar.EmpleadoId = request.EmpleadoId;
                pedidoEditar.ClienteId = request.ClienteId;
                return true;
            }
            return false;
        }

        /// <summary>
        /// retornara la lista de todo el detalle correspondiente al pedido pasado como parametro
        /// </summary>
        /// <param name="idPedido">el id del pedido generado</param>
        /// <param name="context">el contexto de la base de datos</param>
        /// <returns></returns>
        public async Task<List<DetallePedidoViewModel>> ListarDetallePedido(int idPedido, TrabajoFinalContext context)
        {
            var lista = await (from pedido in context.DetallePedidos
                               where pedido.PedidoId == idPedido
                               select new DetallePedidoViewModel
                               {
                                   IdDetalle = pedido.IdDetalle,
                                   PedidoId = pedido.PedidoId,
                                   ProductoId = pedido.ProductoId,
                                   Cantidad = pedido.Cantidad,
                                   PrecioTotal = pedido.PrecioTotal,
                                   Pedido = pedido.Pedido,
                                   Producto = pedido.Producto

                               }).ToListAsync();
            return lista;
        }

        /// <summary>
        /// metodo el cual retornara el detalle completo a un objeto de tipo ViewModel
        /// </summary>
        /// <param name="idDetalle">el id del detalle el cual se quiere obtener toda la informacion</param>
        /// <param name="context">el contexto de la base de datos</param>
        /// <returns></returns>
        public async Task<DetallePedidoViewModel> GetDetallePedido(int idDetalle, TrabajoFinalContext context)
        {
            var detalle = await (from pedido in context.DetallePedidos
                                 where pedido.IdDetalle == idDetalle
                                 select new DetallePedidoViewModel
                                 {
                                     IdDetalle = pedido.IdDetalle,
                                     PedidoId = pedido.PedidoId,
                                     ProductoId = pedido.ProductoId,
                                     Cantidad = pedido.Cantidad,
                                     PrecioTotal = pedido.PrecioTotal,
                                     Pedido = pedido.Pedido,
                                     Producto = pedido.Producto

                                 }).FirstAsync();
            return detalle;
        }


        /// <summary>
        /// metodo utilizado para actualizar los datos del producto que fue utilizado para agregar al detalle de la compra
        /// Si el pedido fue agregado desde el metodo action 'Add' el parametro opcional requestFromAdd tomara valor distion de null
        /// y se editara el producto que se agregue al detalle.
        /// Si el pedido fue editado desde el metodo action 'Edit' el parametro opcional requestFromeEdit tomara valor distion de null
        /// y se editaran los parametros del producto seleccionado volviendo a calcular la suma del producto disponible
        /// </summary>
        /// <param name="context">el contexto de la base de datos</param>
        /// <param name="requestFromAdd">el producto que se quiere agregar al detalle, utilizado solamente desde el metodo action Add</param>
        /// <param name="requestFromEdit">el producto que se quiere editar del detalle, utilizado solamente desde el metodo action Edit</param>
        /// <param name="isUpdate">parametro opcional que indica si la accion realizada proviene de una edicion</param>
        /// <returns>retorna el total del pedido en caso de ser valido, caso contrario retornara 0</returns>
        public async Task<int> ActualizarProducto(TrabajoFinalContext context, DetallePedido requestFromAdd = null, DetallePedidoViewModel requestFromEdit = null, bool isUpdate = false)
        {
            Producto productoEditar = null;
            int total = 0;
            if (requestFromAdd != null)
            {
                productoEditar = await context.Productos.FindAsync(requestFromAdd.ProductoId);
                if (requestFromAdd?.Cantidad > productoEditar.UnidadesDisponibles)
                {
                    return total;
                }
                productoEditar.UnidadesDisponibles -= requestFromAdd.Cantidad;
                total = (int)productoEditar.Precio * requestFromAdd.Cantidad;
                return total;
            }
            else
            {
                productoEditar = await context.Productos.FindAsync(requestFromEdit.ProductoId);
                if (isUpdate)
                {
                    if (requestFromEdit?.Cantidad > productoEditar.UnidadesDisponibles)
                    {
                        return 0;
                    }
                    DetallePedido pedidoEditar = await context.DetallePedidos.FindAsync(requestFromEdit.IdDetalle);
                    pedidoEditar.ProductoId = requestFromEdit.ProductoId;
                    pedidoEditar.Cantidad = requestFromEdit.Cantidad;
                    pedidoEditar.PedidoId = requestFromEdit.PedidoId;
                    //si edito la cantidad del detalle, tengo que volver a la cantidad de unidades disponibles original para hacer la nueva cuenta
                    int? unidadesDisponiblesOriginal = productoEditar.CantidadStock - productoEditar.UnidadesDisponibles;
                    // le sumo al producto indicado el producto de la resta anterior para restablecer la cantidad de productos
                    productoEditar.UnidadesDisponibles += unidadesDisponiblesOriginal;
                    // y se la vuelvo a restar 
                    productoEditar.UnidadesDisponibles -= requestFromEdit.Cantidad;

                    total = (int)productoEditar.Precio * requestFromEdit.Cantidad;
                    pedidoEditar.PrecioTotal = total;
                    return total;
                }
            }
            return 0;
        }

        /// <summary>
        /// metodo encargado de devolver la lista de productos disponibles en una lista tipo SelectListItem utilizada
        /// para los dropdownlist dinamicamente para que cada elemento tome un tipo clave=>valor
        /// solo se mostraran los productos que no esten eliminados y cuyas unidades disponibles sean mayor a 0
        /// </summary>
        /// <param name="context">el contexto de la base de datos</param>
        /// <returns></returns>
        public async Task<List<SelectListItem>> ListarProductos(TrabajoFinalContext context)
        {
            var lista = await (from producto in context.Productos
                               where producto.CantidadStock > 0
                               && producto.EstadoId != 0 &&
                               producto.UnidadesDisponibles > 0
                               select new SelectListItem
                               {
                                   Text = producto.Modelo + " cantidad en stock: " + producto.UnidadesDisponibles,
                                   Value = producto.Id.ToString()

                               }).ToListAsync();
            return lista;
        }

        /// <summary>
        /// metodo utilizado al momento de eliminar un producto del detalle, vuelve a agregar las unidades que fueron utilizadas
        /// al producto original
        /// </summary>
        /// <param name="requestFromDelete">el prodcuto que  se quiere eliminar del detalle</param>
        /// <param name="context">el contexto de la base de datos</param>
        /// <returns></returns>
        public async Task<bool> RecuperarProductos(DetallePedido requestFromDelete, TrabajoFinalContext context)
        {
            if (requestFromDelete is not null)
            {
                Producto productoRecuperarUnidades = await context.Productos.FindAsync(requestFromDelete.ProductoId);
                productoRecuperarUnidades.UnidadesDisponibles += requestFromDelete.Cantidad;
                return true;
            }
            return false;
        }

    }


}
