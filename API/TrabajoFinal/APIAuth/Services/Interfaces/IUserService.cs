using APIAuth.Models;
using APIAuth.Models.ViewModel;

namespace APIAuth.Services.Interfaces
{
    public interface IUserService
    {
        Task<Usuario> VerificarUsuario(UserLoginViewModel user);

        string CreateToken(Usuario user);

        Task<bool> CrearUsuario(UserLoginViewModel user);
    }
}
