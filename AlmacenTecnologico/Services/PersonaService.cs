using AlmacenTecnologico.Context;
using AlmacenTecnologico.Models;
using AlmacenTecnologico.Models.ViewModel;
using AlmacenTecnologico.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AlmacenTecnologico.Services
{
    public class PersonaService : IPersonaService
    {
        /// <summary>
        /// metodo encargado de retornar la lista de empeados activos en el sistema
        /// </summary>
        /// <param name="context">el contexto de la base de datos</param>
        /// <returns></returns>
        public async Task<List<EmpleadoViewModel>> ListarEmpleados(TrabajoFinalContext context)
        {
            var lista = await (from empleados in context.Personas
                               where empleados.EstadoId == 1
                               select new EmpleadoViewModel
                               {
                                   Id = empleados.Id,
                                   Nombre = empleados.Nombre,
                                   Apellido = empleados.Apellido,
                                   DNI = empleados.Dni,
                                   Estadoid = empleados.EstadoId
                               }).ToListAsync();
            return lista;
        }
        /// <summary>
        /// metodo encargado de retornar la lista de empleados eliminados/inactivos del sistema
        /// </summary>
        /// <param name="context">el contexto de la base de datos</param>
        /// <returns></returns>
        public async Task<List<EmpleadoViewModel>> ListarEmpleadosEliminados(TrabajoFinalContext context)
        {
            var lista = await (from empleados in context.Personas
                               where empleados.EstadoId == 0
                               select new EmpleadoViewModel
                               {
                                   Id = empleados.Id,
                                   Nombre = empleados.Nombre,
                                   Apellido = empleados.Apellido,
                                   DNI = empleados.Dni,
                                   Estadoid = empleados.EstadoId
                               }).ToListAsync();
            return lista;
        }

        /// <summary>
        /// metodo encargado de devolver el empleado con todos sus datos el cual tenga el mismo indentificado que le pasan por parametro
        /// </summary>
        /// <param name="id">el identificador del empleado</param>
        /// <param name="context">el contexto de la base de datos</param>
        /// <returns></returns>
        public async Task<EmpleadoViewModel> GetEmpleado(int id, TrabajoFinalContext context)
        {
            var empleado = await (from empleados in context.Personas
                               where empleados.Id == id
                               select new EmpleadoViewModel
                               {
                                   Id = empleados.Id,
                                   Nombre = empleados.Nombre,
                                   Apellido = empleados.Apellido,
                                   DNI = empleados.Dni,
                                   Estadoid = empleados.EstadoId
                               }).FirstAsync();
            return empleado;
        }

        /// <summary>
        /// metodo encargado de editar un empleado de la base de datos, recibe los datos que se le quieren aplicar al empleado (request),
        /// valida que estos sean validos y se los asigna
        /// </summary>
        /// <param name="context">el contexto de la base de datos</param>
        /// <param name="request">los datos que recibe de la peticion de tipo POST del formulario editar</param>
        /// <returns>retorna true si pudo modificar al empleado</returns>
        public async Task<bool> EditarEmpleado(TrabajoFinalContext context, EmpleadoViewModel request)
        {
            if (request != null)
            {
                Persona empleadoModificar = await context.Personas.FindAsync(request.Id);
                if(await ValidarDatosEmpleado(empleadoModificar, context, true))
                {
                    empleadoModificar.Nombre = request.Nombre;
                    empleadoModificar.Apellido = request.Apellido;
                    empleadoModificar.Dni = request.DNI;
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// metodo encargado de validar los datos del empleado, tanto cuando se este agregando uno nuevo como cuando se este editando uno ya existente
        /// 
        /// </summary>
        /// <param name="request">los datos que recibe de la peticion de tipo POST del formulario editar o del agregar</param>
        /// <param name="context">el contexto de la base de datos </param>
        /// <param name="editando">parametro utilizado con la finalidad de indicarle al metodo si la accion que se esta realizando
        /// es por una edicion de datos existentes en la base de datos o un dato nuevo que se ingresa a la tabla empleados
        /// sirve para modificar la funcionalidad del metodo ValidarDni</param>
        /// <returns></returns>
        public async Task<bool> ValidarDatosEmpleado(Persona request, TrabajoFinalContext context, bool editando = false)
        {
            var tareas = new List<Task<bool>>
            {
                ValidarNombreCompleto(request.Nombre),
                ValidarNombreCompleto(request.Apellido),
                ValidarDNI(request.Dni, context, editando)
            };
            while(tareas.Count > 0)
            {
                var validacion = await Task.WhenAny(tareas);
                if(await validacion == false)
                {
                    return false;
                }
                tareas.Remove(validacion);
            }
            return true;
        }

        /// <summary>
        /// metodo encargado de verificar una cadena de string, fijandose que todos los elementos que contengan sean LETRAS
        /// </summary>
        /// <param name="nombre">el string/cadena que se quiere verficar</param>
        /// <returns>retorna true si la cadena es correcta</returns>
        public async Task<bool> ValidarNombreCompleto(string nombre)
        {
            foreach (char letra in nombre)
            {
                if (letra.GetType() != typeof(char))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// metodo encarcagado de validar un dni, como los nombres y apellidos pueden coincidir pero el dni es unico, este metodo permitira
        /// al momento de ingresar un nuevo dato a la tabla Persona verificar si este dato ya existe
        /// </summary>
        /// <param name="dni">el dni del empleado que se quiere ingresar</param>
        /// <param name="context">el contexto de la base de datos</param>
        /// <param name="editando">parametro opcional que permite cambiar la funcionalidad del metodo
        /// si este es true, sera utilizado al momento de editar un empleado existente en la base de datos
        /// es decir, ya sabemos que el dato existe en ela base de datos y solo corroborara que la cadena indicada sean solo numeros</param>
        /// <returns></returns>
        public async Task<bool> ValidarDNI(string dni, TrabajoFinalContext context, bool editando = false)
        {
            bool parseo = int.TryParse(dni, out int id);
            if (editando)
            {
                return parseo;
            }
            return await context.Personas.AnyAsync(empleado => empleado.Dni == dni) == false && parseo;
        }
    }
}
