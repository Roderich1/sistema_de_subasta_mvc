using Microsoft.AspNetCore.Mvc;
using sistema_de_subasta_mvc.Models;
using sistema_de_subasta_mvc.Models.ViewModels;
using sistema_de_subasta_mvc.Services;

namespace sistema_de_subasta_mvc.Controllers
{
    public class ProductosController : Controller
    {
        private readonly IProductService _productService;

        public ProductosController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> MisProductos()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            try
            {
                var productos = await _productService.GetProductosByVendedorAsync(userId);
                return View(productos);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al cargar los productos: {ex.Message}";
                return View(new List<ProductoViewModel>());
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            return View(new ProductoViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductoViewModel model)
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            try
            {
                model.VendedorId = userId;

                if (model.Tipo == "subasta")
                {
                    model.Precio = model.PrecioInicial;
                    var producto = await _productService.CreateProductoAsync(model);

                    var subastaModel = new SubastaViewModel
                    {
                        ProductoId = producto.Id,
                        VendedorId = userId,
                        PrecioInicial = model.PrecioInicial,
                        PrecioActual = model.PrecioInicial,
                        IncrementoMinimo = model.IncrementoMinimo,
                        PrecioReserva = model.PrecioReserva,
                        FechaFin = model.FechaFin
                    };

                    await _productService.CreateSubastaAsync(subastaModel);
                }
                else
                {
                    await _productService.CreateProductoAsync(model);
                }

                TempData["SuccessMessage"] = "Producto publicado exitosamente";
                return RedirectToAction(nameof(MisProductos));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al crear el producto: {ex.Message}");
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CambiarEstado(string id, string estado)
        {
            try
            {
                var resultado = await _productService.UpdateEstadoAsync(id, estado);
                return Json(new { success = resultado });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}