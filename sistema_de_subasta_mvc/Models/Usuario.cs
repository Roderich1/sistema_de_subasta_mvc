namespace sistema_de_subasta_mvc.Models
{
    public class Usuario
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public Direccion Direccion { get; set; }
        public string Estado { get; set; }
        public double Reputacion { get; set; }
        public Wallet Wallet { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}