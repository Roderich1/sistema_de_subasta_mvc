using System.ComponentModel.DataAnnotations;

namespace sistema_de_subasta_mvc.Models.ViewModels
{
    public class SubastaViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "El ID del producto es requerido")]
        public string ProductoId { get; set; }

        public string VendedorId { get; set; }

        [Required(ErrorMessage = "El precio inicial es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio inicial debe ser mayor que 0")]
        public decimal PrecioInicial { get; set; }

        public decimal PrecioActual { get; set; }

        [Required(ErrorMessage = "El incremento mínimo es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El incremento mínimo debe ser mayor que 0")]
        public decimal IncrementoMinimo { get; set; }

        [Required(ErrorMessage = "El precio de reserva es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio de reserva debe ser mayor que 0")]
        public decimal PrecioReserva { get; set; }

        public DateTime FechaInicio { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "La fecha de finalización es requerida")]
        public DateTime FechaFin { get; set; }

        public string Estado { get; set; } = "activa";

        public string GanadorId { get; set; }

        public int CantidadPujas { get; set; }
    }
}