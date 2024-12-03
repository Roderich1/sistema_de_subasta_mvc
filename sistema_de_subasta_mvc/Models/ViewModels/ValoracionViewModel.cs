using System.ComponentModel.DataAnnotations;

namespace sistema_de_subasta_mvc.Models.ViewModels
{
    public class ValoracionViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "El vendedor es requerido")]
        public string VendedorId { get; set; }

        [Required(ErrorMessage = "El comprador es requerido")]
        public string CompradorId { get; set; }

        [Required(ErrorMessage = "La venta es requerida")]
        public string VentaId { get; set; }

        [Required(ErrorMessage = "La puntuación es requerida")]
        [Range(1, 5, ErrorMessage = "La puntuación debe estar entre 1 y 5")]
        [Display(Name = "Puntuación")]
        public int Puntuacion { get; set; }

        [Display(Name = "Comentario")]
        [StringLength(500, ErrorMessage = "El comentario no puede exceder los 500 caracteres")]
        public string Comentario { get; set; }

        public DateTime Fecha { get; set; }

        // Propiedades adicionales para la vista
        public string NombreVendedor { get; set; }
        public string NombreComprador { get; set; }
        public string ProductoTitulo { get; set; }
    }
}