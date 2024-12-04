using System.ComponentModel.DataAnnotations;

namespace sistema_de_subasta_mvc.Models.ViewModels
{
    public class ProductoWithSubastaViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "El título es requerido")]
        public string Titulo { get; set; }

        public string Descripcion { get; set; }

        public string VendedorId { get; set; }

        [Required(ErrorMessage = "La categoría es requerida")]
        public string Categoria { get; set; }

        public string Subcategoria { get; set; }

        [Required(ErrorMessage = "El tipo es requerido")]
        public string Tipo { get; set; } // "ventaDirecta" o "subasta"

        [Required(ErrorMessage = "El precio es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que 0")]
        public decimal Precio { get; set; }

        public List<string> Imagenes { get; set; } = new List<string>();

        public string Estado { get; set; } = "activo";

        [Required(ErrorMessage = "La condición es requerida")]
        public string Condicion { get; set; }

        public int Visitas { get; set; }

        public DateTime FechaPublicacion { get; set; } = DateTime.UtcNow;

        // Propiedades de Subasta
        [Display(Name = "Precio Inicial")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio inicial debe ser mayor que 0")]
        public decimal PrecioInicial { get; set; }

        public decimal PrecioActual { get; set; }

        [Display(Name = "Incremento Mínimo")]
        public decimal IncrementoMinimo { get; set; }

        [Display(Name = "Precio de Reserva")]
        public decimal PrecioReserva { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha de Finalización")]
        public DateTime FechaFin { get; set; }

        public int CantidadPujas { get; set; }
    }
}