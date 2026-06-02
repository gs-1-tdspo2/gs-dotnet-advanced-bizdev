using Amanaje_API.Enums;

namespace Amanaje_API.DTOs
{
    public class ProcessamentoRequestDto
    {
        public int? IdRegiao { get; set; }
        public int? IdUsuario { get; set; }
        public TipoProcessamento TpProcess { get; set; }
        public string DsOrigem { get; set; } = string.Empty;
        public string? DsParam { get; set; }
    }
}