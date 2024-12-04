using System.Text.Json;
using sistema_de_subasta_mvc.Models;
using sistema_de_subasta_mvc.Models.ViewModels;

namespace sistema_de_subasta_mvc.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://4cb4-181-115-215-38.ngrok-free.app/api";

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ProductoViewModel>> GetProductosByVendedorAsync(string vendedorId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<ProductoViewModel>>($"{BaseUrl}/Producto/vendedor/{vendedorId}")
                    ?? new List<ProductoViewModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error obteniendo productos: {ex.Message}");
                throw;
            }
        }

        public async Task<ProductoViewModel> CreateProductoAsync(ProductoViewModel model)
        {
            try
            {
                var productoRequest = new
                {
                    id = string.Empty,
                    titulo = model.Titulo,
                    descripcion = model.Descripcion,
                    vendedorId = model.VendedorId,
                    categoria = model.Categoria,
                    subcategoria = "gf",
                    tipo = model.Tipo,
                    precio = model.Precio,
                    imagenes = new List<string> { "fgfg" },
                    estado = "activo",
                    condicion = model.Condicion,
                    visitas = 0,
                    fechaPublicacion = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
                };

                var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/Producto", productoRequest);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error al crear el producto: {error}");
                }

                return await response.Content.ReadFromJsonAsync<ProductoViewModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en CreateProductoAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<SubastaViewModel> CreateSubastaAsync(SubastaViewModel model)
        {
            try
            {
                var subastaRequest = new
                {
                    productoId = model.ProductoId,
                    vendedorId = model.VendedorId,
                    precioInicial = model.PrecioInicial,
                    precioActual = model.PrecioInicial,
                    incrementoMinimo = model.IncrementoMinimo,
                    precioReserva = model.PrecioReserva,
                    fechaInicio = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                    fechaFin = model.FechaFin.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                    estado = "activa",
                    cantidadPujas = 0
                };

                var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/Subasta", subastaRequest);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error al crear la subasta: {error}");
                }

                return await response.Content.ReadFromJsonAsync<SubastaViewModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en CreateSubastaAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateEstadoAsync(string id, string estado)
        {
            try
            {
                var producto = await _httpClient.GetFromJsonAsync<ProductoViewModel>($"{BaseUrl}/Producto/{id}");
                if (producto == null) return false;

                producto.Estado = estado;
                var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/Producto/{id}", producto);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error actualizando estado: {ex.Message}");
                return false;
            }
        }
    }
}
