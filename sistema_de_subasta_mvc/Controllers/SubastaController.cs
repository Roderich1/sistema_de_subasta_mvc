using Microsoft.AspNetCore.Mvc;
using sistema_de_subasta_mvc.Services;
using sistema_de_subasta_mvc.Models;

namespace sistema_de_subasta_mvc.Controllers
{
    public class SubastaController : Controller
    {
        private readonly ISubastaService _subastaService;

        public SubastaController(ISubastaService subastaService)
        {
            _subastaService = subastaService;
        }

        // Listar productos: subastas, ventas directas o todos
        public async Task<IActionResult> Index(string tipo = "todos")
        {
            List<ProductoViewModel> productos;

            switch (tipo.ToLower())
            {
                case "subasta":
                    productos = await _subastaService.GetSubastasAsync();
                    break;
                case "ventadirecta":
                    productos = await _subastaService.GetVentaDirectaAsync();
                    break;
                default:
                    productos = await _subastaService.GetAllProductosAsync();
                    break;
            }

            ViewBag.TipoActual = tipo;
            return View(productos);
        }

        // Agregar a favoritos
        public async Task<IActionResult> AgregarAFavoritos(int productoId)
        {
            bool resultado = await _subastaService.AgregarAFavoritosAsync(productoId);
            if (resultado)
            {
                TempData["Mensaje"] = "Producto agregado a favoritos.";
            }
            else
            {
                TempData["Mensaje"] = "Error al agregar a favoritos.";
            }
            return RedirectToAction(nameof(Index));
        }

        // Ver detalles de un producto en un card
        public async Task<IActionResult> VerProducto(int productoId)
        {
            var producto = await _subastaService.GetProductoByIdAsync(productoId);
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }

        // Participar en una subasta o comprar directamente
        public async Task<IActionResult> Participar(int productoId)
        {
            bool resultado = await _subastaService.ParticiparEnSubastaAsync(productoId);
            if (resultado)
            {
                TempData["Mensaje"] = "Participación en subasta exitosa.";
            }
            else
            {
                TempData["Mensaje"] = "Error al participar en la subasta.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
