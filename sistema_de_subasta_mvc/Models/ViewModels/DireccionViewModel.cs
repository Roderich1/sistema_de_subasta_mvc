using System.ComponentModel.DataAnnotations;

namespace sistema_de_subasta_mvc.Models.ViewModels
{
    public class DireccionViewModel
    {
        [Required(ErrorMessage = "La calle es requerida")]
        [Display(Name = "Calle")]
        public string Calle { get; set; }

        [Required(ErrorMessage = "La ciudad es requerida")]
        [Display(Name = "Ciudad")]
        public string Ciudad { get; set; }

        [Required(ErrorMessage = "El código postal es requerido")]
        [Display(Name = "Código Postal")]
        public string CodigoPostal { get; set; }
    }
}