using Microsoft.AspNetCore.Mvc;
using sistema_de_subasta_mvc.Models.ViewModels;
using sistema_de_subasta_mvc.Services;

namespace sistema_de_subasta_mvc.Controllers
{
    // Controllers/ProfileController.cs
    public class ProfileController : Controller
    {
        private readonly IApiService _apiService;

        public ProfileController(IApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            var user = await _apiService.GetUserByIdAsync(userId);
            if (user == null)
                return NotFound();

            var viewModel = new ProfileViewModel
            {
                Username = user.Username,
                Email = user.Email,
                Nombre = user.Nombre,
                Telefono = user.Telefono,
                Reputacion = user.Reputacion,
                FechaRegistro = user.FechaRegistro,
                Direccion = new DireccionViewModel
                {
                    Calle = user.Direccion.Calle,
                    Ciudad = user.Direccion.Ciudad,
                    CodigoPostal = user.Direccion.CodigoPostal
                },
                Wallet = new WalletViewModel
                {
                    SaldoActual = user.Wallet.Saldo,
                    SaldoBloqueado = user.Wallet.SaldoBloqueado
                }
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Wallet()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            var user = await _apiService.GetUserByIdAsync(userId);
            if (user == null)
                return NotFound();

            var viewModel = new WalletViewModel
            {
                SaldoActual = user.Wallet.Saldo,
                SaldoBloqueado = user.Wallet.SaldoBloqueado
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddFunds(WalletViewModel model)
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            try
            {
                // Obtener el usuario actual para mantener los valores del wallet
                var user = await _apiService.GetUserByIdAsync(userId);
                if (user == null)
                    return NotFound();

                // Si el ModelState no es válido, mantener los valores actuales
                if (!ModelState.IsValid)
                {
                    model.SaldoActual = user.Wallet.Saldo;
                    model.SaldoBloqueado = user.Wallet.SaldoBloqueado;
                    return View("Wallet", model);
                }

                var result = await _apiService.UpdateWalletAsync(userId, model.MontoAgregar);
                if (result)
                {
                    TempData["SuccessMessage"] = $"Se han agregado ${model.MontoAgregar} a tu cartera correctamente";
                    return RedirectToAction("Wallet");
                }

                // Si hay error, mantener los valores actuales
                model.SaldoActual = user.Wallet.Saldo;
                model.SaldoBloqueado = user.Wallet.SaldoBloqueado;
                ModelState.AddModelError("", "Error al procesar el pago");
                return View("Wallet", model);
            }
            catch (Exception ex)
            {
                // En caso de excepción, obtener los valores actuales
                var user = await _apiService.GetUserByIdAsync(userId);
                if (user != null)
                {
                    model.SaldoActual = user.Wallet.Saldo;
                    model.SaldoBloqueado = user.Wallet.SaldoBloqueado;
                }
                ModelState.AddModelError("", $"Error: {ex.Message}");
                return View("Wallet", model);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Reputacion()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            try
            {
                // Obtener solo las valoraciones como vendedor
                var valoracionesVendedor = await _apiService.GetValoracionesUsuarioAsync(userId, "vendedor");
                var promedioValoraciones = await _apiService.GetPromedioValoracionesAsync(userId);

                var viewModel = new ReputacionViewModel
                {
                    ValoracionesComoVendedor = valoracionesVendedor,
                    PromedioValoraciones = promedioValoraciones,
                    TotalValoraciones = valoracionesVendedor.Count
                };

                // Actualizar la reputación del usuario si es necesario
                var user = await _apiService.GetUserByIdAsync(userId);
                if (user != null && Math.Abs(user.Reputacion - promedioValoraciones) > 0.01)
                {
                    await _apiService.UpdateReputacionAsync(userId, promedioValoraciones);
                }

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al cargar las valoraciones: {ex.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}
