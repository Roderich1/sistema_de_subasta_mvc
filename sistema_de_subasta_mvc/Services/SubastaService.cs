using System.Text.Json;
using sistema_de_subasta_mvc.Models;

namespace sistema_de_subasta_mvc.Services
{
    public class SubastaService : ISubastaService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://4cb4-181-115-215-38.ngrok-free.app/api";

        public SubastaService(HttpClient httpClient)
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

        public async Task<bool> AgregarAFavoritosAsync(int productoId)
        {
            try
            {
                // Lógica para agregar a favoritos
                var response = await _httpClient.PostAsync($"{BaseUrl}/Favoritos/{productoId}", null);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al agregar a favoritos: {ex.Message}");
                return false;
            }
        }

        public async Task<ProductoViewModel> GetProductoByIdAsync(int productoId)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<ProductoViewModel>($"{BaseUrl}/Producto/{productoId}");
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener producto: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> ParticiparEnSubastaAsync(int productoId)
        {
            try
            {
                // Lógica para participar en la subasta
                var response = await _httpClient.PostAsync($"{BaseUrl}/Subasta/{productoId}/Participar", null);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al participar en la subasta: {ex.Message}");
                return false;
            }
        }
    }
}
