using Microsoft.AspNetCore.Mvc;
using sistema_de_subasta_mvc.Models.ViewModels;
using sistema_de_subasta_mvc.Services;

namespace sistema_de_subasta_mvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly IApiService _apiService;

        public AccountController(IApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            // Si ya está autenticado, redirigir al Home
            if (HttpContext.Session.GetString("UserId") != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _apiService.ValidateUserAsync(model.Username, model.Password);

                if (user != null)
                {
                    HttpContext.Session.SetString("UserId", user.Id);
                    HttpContext.Session.SetString("Username", user.Username);

                    // Redireccionar según el tipo de usuario
                    if (user.Username.ToLower() == "admin")
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Usuario o contraseña incorrectos");
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _apiService.RegisterUserAsync(model);
                    if (user != null)
                    {
                        TempData["SuccessMessage"] = "Registro exitoso. Por favor, inicia sesión.";
                        return RedirectToAction("Login");
                    }
                }
                catch (Exception ex)
                {
                    // Loguear el error
                    Console.WriteLine($"Error durante el registro: {ex.Message}");
                    ModelState.AddModelError("", "Error al registrar el usuario. Por favor, intente nuevamente.");
                }
            }
            return View(model);
        }


    }
}