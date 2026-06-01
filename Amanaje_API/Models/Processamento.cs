using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Amanaje_API.Models.Externals;

namespace Amanaje_API.Models
{
    [Table("TB_AMANAJE_PROCESS")]
    public class Processamento
    {
        [Key]
        [Column("ID_PROCESSAMENTO")]
        public int IdProcessamento { get; set; }

        [Column("ID_REGIAO")]
        public int? IdRegiao { get; set; }

        [Column("ID_USUARIO")]
        public int? IdUsuario { get; set; }

        [Required(ErrorMessage = "O tipo de processamento é obrigatório.")]
        [RegularExpression(@"^(SINCRONIZACAO_CLIM|CALCULO_RISCO|GERACAO_IND|GERACAO_ALERTA|CARGA_DADOS|ROTINA_PL_SQL|OUTRO)$",
            ErrorMessage = "Tipo de processamento inválido. Valores aceitos: SINCRONIZACAO_CLIM, CALCULO_RISCO, GERACAO_IND, GERACAO_ALERTA, CARGA_DADOS, ROTINA_PL_SQL, OUTRO.")]
        [Column("TP_PROCESS")]
        public string TpProcess { get; set; } = string.Empty;

        [Required(ErrorMessage = "O status do processamento é obrigatório.")]
        [RegularExpression(@"^(INICIADO|EM_EXECUCAO|CONCLUIDO|FALHOU|CANCELADO)$",
            ErrorMessage = "Status inválido. Valores aceitos: INICIADO, EM_EXECUCAO, CONCLUIDO, FALHOU, CANCELADO.")]
        [Column("ST_PROCESS")]
        public string StProcess { get; set; } = "INICIADO";

        [Required(ErrorMessage = "A origem do processamento é obrigatória.")]
        [StringLength(120, MinimumLength = 1, ErrorMessage = "A origem deve ter entre 1 e 120 caracteres.")]
        [Column("DS_ORIGEM")]
        public string DsOrigem { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Os parâmetros devem ter no máximo 1000 caracteres.")]
        [Column("DS_PARAM")]
        public string? DsParam { get; set; }

        [StringLength(1000, ErrorMessage = "O resultado deve ter no máximo 1000 caracteres.")]
        [Column("DS_RESULT")]
        public string? DsResult { get; set; }

        [Required]
        [Column("DT_INICIO")]
        public DateTime DtInicio { get; set; } = DateTime.UtcNow;

        [Column("DT_FIM")]
        public DateTime? DtFim { get; set; }

        // Navigation properties
        [JsonIgnore]
        [ForeignKey("IdRegiao")]
        public RegiaoMonitorada? Regiao { get; set; }

        [JsonIgnore]
        [ForeignKey("IdUsuario")]
        public UsuarioExternal? Usuario { get; set; }
    }
}