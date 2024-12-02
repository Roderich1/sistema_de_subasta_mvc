using Microsoft.AspNetCore.Mvc;
using sistema_de_subasta_mvc.Services;
using sistema_de_subasta_mvc.Models;

namespace sistema_de_subasta_mvc.Controllers
{
    public class ProductoController : Controller
    {
        private readonly IProductoService _productoService;

        public ProductoController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        public async Task<IActionResult> Index(string tipo = "todos")
        {
            List<ProductoViewModel> productos;

            switch (tipo.ToLower())
            {
                case "subasta":
                    productos = await _productoService.GetSubastasAsync();
                    break;
                case "ventadirecta":
                    productos = await _productoService.GetVentaDirectaAsync();
                    break;
                default:
                    productos = await _productoService.GetAllProductosAsync();
                    break;
            }

            ViewBag.TipoActual = tipo;
            return View(productos);
        }
    }
}
