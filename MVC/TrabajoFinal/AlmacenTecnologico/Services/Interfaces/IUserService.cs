using AlmacenTecnologico.Context;
using AlmacenTecnologico.Models;
using AlmacenTecnologico.Models.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AlmacenTecnologico.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<UsuarioViewModel>> ListarUsuarios(UsuarioViewModel.Estado estado);

        Task<UsuarioViewModel> GetUsuario(int id);
        Task<bool> DecryptRequest(string stream, HttpContext controllerBase);
        Task SetClaims(UsuarioViewModel user, HttpContext ControllerBase);

        //CRUD Methods

        Task<List<SelectListItem>> ListarEmpleados();
        Task<List<SelectListItem>> ListarRoles();
        Task<List<UsuarioViewModel>> ListarUsuariosPendientes(UsuarioViewModel.Estado estado);
        bool AgregarUsuario(Usuario request);
        Task<bool> EditarUsuario(UsuarioViewModel request);
        Task<bool> EliminarUsuarioLogico(UsuarioViewModel request);
        Task<bool> RecuperarUsuario(UsuarioViewModel request);
        Task<bool> EliminarDefinitivo(int id);
        Task<bool> IntegrarUsuarioSistema(int id);

    }
}
