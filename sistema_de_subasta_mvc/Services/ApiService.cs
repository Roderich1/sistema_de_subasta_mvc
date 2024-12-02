using sistema_de_subasta_mvc.Models;
using sistema_de_subasta_mvc.Models.ViewModels;
using System.Text.Json;

namespace sistema_de_subasta_mvc.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://3c41-177-222-46-173.ngrok-free.app/api";

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Usuario> ValidateUserAsync(string username, string password)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<Usuario>>($"{BaseUrl}/Usuario");
                return response?.FirstOrDefault(u =>
                    u.Username == username &&
                    u.Password == password &&
                    u.Estado == "activo");
            }
            catch
            {
                return null;
            }
        }

        public async Task<Usuario> RegisterUserAsync(RegisterViewModel model)
        {
            try
            {
                var newUser = new
                {
                    id = string.Empty, // MongoDB generará el ID
                    username = model.Username,
                    email = model.Email,
                    password = model.Password,
                    nombre = model.Nombre,
                    telefono = model.Telefono,
                    direccion = new
                    {
                        calle = model.Calle,
                        ciudad = model.Ciudad,
                        codigoPostal = model.CodigoPostal
                    },
                    estado = "activo",
                    reputacion = 0.0,
                    wallet = new
                    {
                        saldo = 0,
                        saldoBloqueado = 0
                    },
                    fechaRegistro = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
                };

                var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/Usuario", newUser);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Usuario>();
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al registrar usuario: {errorContent}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en RegisterUserAsync: {ex.Message}");
                throw; // Relanzar la excepción para manejarla en el controller
            }
        }
    }
}
