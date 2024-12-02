using sistema_de_subasta_mvc.Models;

namespace sistema_de_subasta_mvc.Services
{
    public interface IProductoService
    {
        Task<List<ProductoViewModel>> GetAllProductosAsync();
        Task<List<ProductoViewModel>> GetSubastasAsync();
        Task<List<ProductoViewModel>> GetVentaDirectaAsync();
    }
}