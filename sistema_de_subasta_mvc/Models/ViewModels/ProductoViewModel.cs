using System;
using System.Collections.Generic;

namespace sistema_de_subasta_mvc.Models
{
    public class ProductoViewModel
    {
        public string Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string VendedorId { get; set; }
        public string Categoria { get; set; }
        public string Subcategoria { get; set; }
        public string Tipo { get; set; }
        public decimal Precio { get; set; }
        public List<string> Imagenes { get; set; }
        public string Estado { get; set; }
        public string Condicion { get; set; }
        public int Visitas { get; set; }
        public DateTime FechaPublicacion { get; set; }
    }
}