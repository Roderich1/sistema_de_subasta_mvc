namespace sistema_de_subasta_mvc.Models.ViewModels
{
    public class ReputacionViewModel
    {
        public List<ValoracionViewModel> ValoracionesComoVendedor { get; set; }
        public double PromedioValoraciones { get; set; }
        public int TotalValoraciones { get; set; }

        public ReputacionViewModel()
        {
            ValoracionesComoVendedor = new List<ValoracionViewModel>();
        }
    }
}