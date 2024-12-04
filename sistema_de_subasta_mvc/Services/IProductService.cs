using sistema_de_subasta_mvc.Models;
using sistema_de_subasta_mvc.Models.ViewModels;

namespace sistema_de_subasta_mvc.Services
{
    public interface IProductService
    {
        Task<List<ProductoViewModel>> GetProductosByVendedorAsync(string vendedorId);
        Task<ProductoViewModel> CreateProductoAsync(ProductoViewModel model);
        Task<SubastaViewModel> CreateSubastaAsync(SubastaViewModel model);
        Task<bool> UpdateEstadoAsync(string id, string estado);
    }
}
