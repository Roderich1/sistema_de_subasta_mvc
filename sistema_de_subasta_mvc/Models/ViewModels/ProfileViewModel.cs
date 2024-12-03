namespace sistema_de_subasta_mvc.Models.ViewModels
{
    public class ProfileViewModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public double Reputacion { get; set; }
        public int TotalVentas { get; set; }
        public int TotalCompras { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DireccionViewModel Direccion { get; set; }
        public WalletViewModel Wallet { get; set; }
    }
}
