using sistema_de_subasta_mvc.Models;
using sistema_de_subasta_mvc.Models.ViewModels;

namespace sistema_de_subasta_mvc.Services
{
    public interface IApiService
    {
        Task<Usuario> ValidateUserAsync(string username, string password);
        Task<Usuario> RegisterUserAsync(RegisterViewModel model);
        Task<bool> UpdateWalletAsync(string userId, decimal montoAgregar);
        Task<bool> UpdateDireccionAsync(string userId, DireccionViewModel direccion);
        Task<Usuario> GetUserByIdAsync(string userId);
        Task<List<ValoracionViewModel>> GetValoracionesUsuarioAsync(string userId, string tipo);
        Task<double> GetPromedioValoracionesAsync(string userId);
        Task<ValoracionViewModel> CreateValoracionAsync(ValoracionViewModel valoracion);
        Task<bool> UpdateReputacionAsync(string userId, double nuevaReputacion);
    }
}