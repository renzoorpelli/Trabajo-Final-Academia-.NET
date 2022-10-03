using AlmacenTecnologico.Context;
using AlmacenTecnologico.Models;
using AlmacenTecnologico.Models.ViewModel;

namespace AlmacenTecnologico.Services.Interfaces
{
    public interface IPersonaService
    {
        Task<List<EmpleadoViewModel>> ListarEmpleados();

        Task<List<EmpleadoViewModel>> ListarEmpleadosEliminados();

        Task<EmpleadoViewModel> GetEmpleado(int id);

        Task<bool> AgregarEmpleado(Persona request);

        Task<bool> EditarEmpleado(EmpleadoViewModel request);

        Task<bool> EliminarEmpleadoLogico(EmpleadoViewModel request);

        Task<bool> RecuperarEmpleado(EmpleadoViewModel request);

        Task<bool> ValidarDatosEmpleado(Persona request,bool editando = false);

        Task<bool> ValidarNombreCompleto(string nombre);

        Task<bool> ValidarDNI(string dni, bool editando = false);
    }
}
