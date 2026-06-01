namespace Amanaje_API.DTOs
{
    public class RegiaoMonitoradaRequestDto
    {
        public int IdCliente { get; set; }
        public string NmRegiao { get; set; } = string.Empty;
        public string NmCidade { get; set; } = string.Empty;
        public string SgEstado { get; set; } = string.Empty;
        public decimal NrLatitude { get; set; }
        public decimal NrLongitude { get; set; }
        public string TpArea { get; set; } = string.Empty;
        public int NrNivelVuln { get; set; }
        public string TpVisib { get; set; } = "PRIVADA";
    }
}