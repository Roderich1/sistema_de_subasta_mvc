using sistema_de_subasta_mvc.Models;

namespace sistema_de_subasta_mvc.Services
{
    public interface ISubastaService
    {
        Task<List<ProductoViewModel>> GetAllProductosAsync();
        Task<List<ProductoViewModel>> GetSubastasAsync();
        Task<List<ProductoViewModel>> GetVentaDirectaAsync();
        Task<bool> AgregarAFavoritosAsync(int productoId);
        Task<ProductoViewModel> GetProductoByIdAsync(int productoId);
        Task<bool> ParticiparEnSubastaAsync(int productoId);
    }
}
