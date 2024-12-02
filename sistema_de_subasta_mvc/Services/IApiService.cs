using sistema_de_subasta_mvc.Models;
using sistema_de_subasta_mvc.Models.ViewModels;

namespace sistema_de_subasta_mvc.Services
{
    public interface IApiService
    {
        Task<Usuario> ValidateUserAsync(string username, string password);
        Task<Usuario> RegisterUserAsync(RegisterViewModel model);
    }
}
