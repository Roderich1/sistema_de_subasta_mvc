namespace sistema_de_subasta_mvc.Models
{
    public class Valoracion
    {
        public string Id { get; set; }
        public string VendedorId { get; set; }
        public string CompradorId { get; set; }
        public string VentaId { get; set; }
        public int Puntuacion { get; set; }
        public string Comentario { get; set; }
        public DateTime Fecha { get; set; }
    }
}