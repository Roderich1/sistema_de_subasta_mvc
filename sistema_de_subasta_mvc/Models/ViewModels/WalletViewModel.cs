
using System.ComponentModel.DataAnnotations;

namespace sistema_de_subasta_mvc.Models.ViewModels

{
    public class WalletViewModel
    {
        [Required(ErrorMessage = "El monto a agregar es requerido")]
        [Range(1, 10000, ErrorMessage = "El monto debe estar entre 1 y 10000")]
        [Display(Name = "Monto a agregar")]
        public decimal MontoAgregar { get; set; }

        [Required(ErrorMessage = "El método de pago es requerido")]
        [Display(Name = "Método de Pago")]
        public string MetodoPago { get; set; }

        public decimal SaldoActual { get; set; }
        public decimal SaldoBloqueado { get; set; }
    }
}

