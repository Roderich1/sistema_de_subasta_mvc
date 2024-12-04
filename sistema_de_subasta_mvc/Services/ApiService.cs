using sistema_de_subasta_mvc.Models;
using sistema_de_subasta_mvc.Models.ViewModels;
using System.Text.Json;

namespace sistema_de_subasta_mvc.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://4cb4-181-115-215-38.ngrok-free.app/api";

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
        public async Task<Usuario> GetUserByIdAsync(string userId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<Usuario>($"{BaseUrl}/Usuario/{userId}");
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> UpdateWalletAsync(string userId, decimal montoAgregar)
        {
            try
            {
                var user = await GetUserByIdAsync(userId);
                if (user == null) return false;

                // Actualizamos el saldo
                user.Wallet.Saldo += montoAgregar;

                // Asegurarnos de que todos los campos necesarios estén presentes
                var updateUser = new
                {
                    id = user.Id,
                    username = user.Username,
                    email = user.Email,
                    password = user.Password,
                    nombre = user.Nombre,
                    telefono = user.Telefono,
                    direccion = user.Direccion,
                    estado = user.Estado,
                    reputacion = user.Reputacion,
                    wallet = new
                    {
                        saldo = user.Wallet.Saldo,
                        saldoBloqueado = user.Wallet.SaldoBloqueado
                    },
                    fechaRegistro = user.FechaRegistro
                };

                var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/Usuario/{userId}", updateUser);
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error al actualizar wallet: {error}");
                }
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en UpdateWalletAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateDireccionAsync(string userId, DireccionViewModel direccion)
        {
            try
            {
                var user = await GetUserByIdAsync(userId);
                if (user == null) return false;

                user.Direccion = new Direccion
                {
                    Calle = direccion.Calle,
                    Ciudad = direccion.Ciudad,
                    CodigoPostal = direccion.CodigoPostal
                };

                var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/Usuario/{userId}", user);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
        public async Task<List<ValoracionViewModel>> GetValoracionesUsuarioAsync(string userId, string tipo)
        {
            try
            {
                // Determinar el endpoint basado en el tipo (vendedor o comprador)
                var endpoint = tipo.ToLower() == "vendedor"
                    ? $"{BaseUrl}/Valoracion/vendedor/{userId}"
                    : $"{BaseUrl}/Valoracion/comprador/{userId}";

                var valoraciones = await _httpClient.GetFromJsonAsync<List<Valoracion>>(endpoint);
                if (valoraciones == null) return new List<ValoracionViewModel>();

                // Convertir y enriquecer los datos
                var valoracionesVM = new List<ValoracionViewModel>();
                foreach (var val in valoraciones)
                {
                    var valoracionVM = new ValoracionViewModel
                    {
                        Id = val.Id,
                        VendedorId = val.VendedorId,
                        CompradorId = val.CompradorId,
                        VentaId = val.VentaId,
                        Puntuacion = val.Puntuacion,
                        Comentario = val.Comentario,
                        Fecha = val.Fecha
                    };

                    // Obtener información adicional
                    var comprador = await GetUserByIdAsync(val.CompradorId);
                    var vendedor = await GetUserByIdAsync(val.VendedorId);

                    valoracionVM.NombreComprador = comprador?.Nombre ?? "Usuario Desconocido";
                    valoracionVM.NombreVendedor = vendedor?.Nombre ?? "Usuario Desconocido";

                    valoracionesVM.Add(valoracionVM);
                }

                return valoracionesVM;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error obteniendo valoraciones: {ex.Message}");
                return new List<ValoracionViewModel>();
            }
        }

        public async Task<double> GetPromedioValoracionesAsync(string userId)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<double>($"{BaseUrl}/Valoracion/vendedor/{userId}/promedio");
                return response;
            }
            catch
            {
                return 0;
            }
        }

        
        public async Task<bool> UpdateReputacionAsync(string userId, double nuevaReputacion)
        {
            try
            {
                var user = await GetUserByIdAsync(userId);
                if (user == null) return false;

                // Actualizamos la reputación manteniendo los demás datos
                var updateUser = new
                {
                    id = user.Id,
                    username = user.Username,
                    email = user.Email,
                    password = user.Password,
                    nombre = user.Nombre,
                    telefono = user.Telefono,
                    direccion = user.Direccion,
                    estado = user.Estado,
                    reputacion = nuevaReputacion,
                    wallet = new
                    {
                        saldo = user.Wallet.Saldo,
                        saldoBloqueado = user.Wallet.SaldoBloqueado
                    },
                    fechaRegistro = user.FechaRegistro
                };

                var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/Usuario/{userId}", updateUser);
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error al actualizar reputación: {error}");
                }
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en UpdateReputacionAsync: {ex.Message}");
                return false;
            }
        }

        // Modificar el método CreateValoracionAsync existente
        public async Task<ValoracionViewModel> CreateValoracionAsync(ValoracionViewModel model)
        {
            try
            {
                var valoracion = new Valoracion
                {
                    VendedorId = model.VendedorId,
                    CompradorId = model.CompradorId,
                    VentaId = model.VentaId,
                    Puntuacion = model.Puntuacion,
                    Comentario = model.Comentario
                };

                var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}/Valoracion", valoracion);
                if (response.IsSuccessStatusCode)
                {
                    var createdValoracion = await response.Content.ReadFromJsonAsync<Valoracion>();
                    if (createdValoracion != null)
                    {
                        // Obtener todas las valoraciones del vendedor y calcular el nuevo promedio
                        var valoracionesVendedor = await GetValoracionesUsuarioAsync(model.VendedorId, "vendedor");
                        double nuevaReputacion = valoracionesVendedor.Average(v => v.Puntuacion);

                        // Actualizar la reputación del vendedor
                        await UpdateReputacionAsync(model.VendedorId, nuevaReputacion);

                        return new ValoracionViewModel
                        {
                            Id = createdValoracion.Id,
                            VendedorId = createdValoracion.VendedorId,
                            CompradorId = createdValoracion.CompradorId,
                            VentaId = createdValoracion.VentaId,
                            Puntuacion = createdValoracion.Puntuacion,
                            Comentario = createdValoracion.Comentario,
                            Fecha = createdValoracion.Fecha
                        };
                    }
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error al crear la valoración: {errorContent}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al crear la valoración: {ex.Message}");
            }
        }
    }
}
