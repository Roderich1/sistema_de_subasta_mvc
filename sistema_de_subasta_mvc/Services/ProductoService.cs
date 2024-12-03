using System.Text.Json;
using sistema_de_subasta_mvc.Models;

namespace sistema_de_subasta_mvc.Services
{
    public class ProductoService : IProductoService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://6265-181-115-215-38.ngrok-free.app/api";

        public ProductoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ProductoViewModel>> GetAllProductosAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<ProductoViewModel>>($"{BaseUrl}/Producto");
                return response ?? new List<ProductoViewModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetAllProductosAsync: {ex.Message}");
                return new List<ProductoViewModel>();
            }
        }

        public async Task<List<ProductoViewModel>> GetSubastasAsync()
        {
            try
            {
                var productos = await GetAllProductosAsync();
                return productos.Where(p => p.Tipo?.ToLower() == "subasta").ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetSubastasAsync: {ex.Message}");
                return new List<ProductoViewModel>();
            }
        }

        public async Task<List<ProductoViewModel>> GetVentaDirectaAsync()
        {
            try
            {
                var productos = await GetAllProductosAsync();
                return productos.Where(p => p.Tipo?.ToLower() == "ventadirecta").ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetVentaDirectaAsync: {ex.Message}");
                return new List<ProductoViewModel>();
            }
        }
    }
}