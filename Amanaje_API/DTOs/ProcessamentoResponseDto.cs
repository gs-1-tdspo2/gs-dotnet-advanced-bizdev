namespace Amanaje_API.DTOs
{
    public class ProcessamentoResponseDto
    {
        public int IdProcessamento { get; set; }
        public int? IdRegiao { get; set; }
        public int? IdUsuario { get; set; }
        public string TpProcess { get; set; } = string.Empty;
        public string StProcess { get; set; } = string.Empty;
        public string DsOrigem { get; set; } = string.Empty;
        public string? DsParam { get; set; }
        public string? DsResult { get; set; }
        public DateTime DtInicio { get; set; }
        public DateTime? DtFim { get; set; }
    }
}