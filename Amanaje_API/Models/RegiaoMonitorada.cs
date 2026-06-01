using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Amanaje_API.Models.Externals;

namespace Amanaje_API.Models
{
    [Table("TB_AMANAJE_REGIAO_MONIT")]
    public class RegiaoMonitorada
    {
        [Key]
        [Column("ID_REGIAO")]
        public int IdRegiao { get; set; }

        [Required(ErrorMessage = "O ID do cliente é obrigatório.")]
        [Column("ID_CLIENTE")]
        public int IdCliente { get; set; }

        [Required(ErrorMessage = "O nome da região é obrigatório.")]
        [StringLength(150, MinimumLength = 1, ErrorMessage = "O nome da região deve ter entre 1 e 150 caracteres.")]
        [Column("NM_REGIAO")]
        public string NmRegiao { get; set; } = string.Empty;

        [Required(ErrorMessage = "O nome da cidade é obrigatório.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "O nome da cidade deve ter entre 1 e 100 caracteres.")]
        [Column("NM_CIDADE")]
        public string NmCidade { get; set; } = string.Empty;

        [Required(ErrorMessage = "A sigla do estado é obrigatória.")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "A sigla do estado deve ter exatamente 2 caracteres.")]
        [RegularExpression(@"^[A-Z]{2}$", ErrorMessage = "A sigla do estado deve conter apenas letras maiúsculas (ex: SP, RJ).")]
        [Column("SG_ESTADO")]
        public string SgEstado { get; set; } = string.Empty;

        [Required(ErrorMessage = "A latitude é obrigatória.")]
        [Range(-90, 90, ErrorMessage = "A latitude deve estar entre -90 e 90.")]
        [Column("NR_LATITUDE")]
        public decimal NrLatitude { get; set; }

        [Required(ErrorMessage = "A longitude é obrigatória.")]
        [Range(-180, 180, ErrorMessage = "A longitude deve estar entre -180 e 180.")]
        [Column("NR_LONGITUDE")]
        public decimal NrLongitude { get; set; }

        [Required(ErrorMessage = "O tipo de área é obrigatório.")]
        [RegularExpression(@"^(PONTE|ENCOSTA|AREA_RURAL|COMUNIDADE|PROPRIEDADE_PRIVADA|REGIAO_RIBEIRINHA|AREA_URBANA|OUTRA)$",
            ErrorMessage = "Tipo de área inválido. Valores aceitos: PONTE, ENCOSTA, AREA_RURAL, COMUNIDADE, PROPRIEDADE_PRIVADA, REGIAO_RIBEIRINHA, AREA_URBANA, OUTRA.")]
        [Column("TP_AREA")]
        public string TpArea { get; set; } = string.Empty;

        [Required(ErrorMessage = "O nível de vulnerabilidade é obrigatório.")]
        [Range(0, 100, ErrorMessage = "O nível de vulnerabilidade deve estar entre 0 e 100.")]
        [Column("NR_NIVEL_VULN")]
        public int NrNivelVuln { get; set; }

        [Required(ErrorMessage = "O tipo de visibilidade é obrigatório.")]
        [RegularExpression(@"^(PRIVADA|INSTITUCIONAL|AGREGADA_PUBLICA)$",
            ErrorMessage = "Tipo de visibilidade inválido. Valores aceitos: PRIVADA, INSTITUCIONAL, AGREGADA_PUBLICA.")]
        [Column("TP_VISIB")]
        public string TpVisib { get; set; } = "PRIVADA";

        [Required(ErrorMessage = "O status ativo é obrigatório.")]
        [RegularExpression(@"^[SN]$", ErrorMessage = "O campo ativo deve ser 'S' (ativo) ou 'N' (inativo).")]
        [Column("ST_ATIVO")]
        public string StAtivo { get; set; } = "S";

        [Required]
        [Column("DT_CRIADO_EM")]
        public DateTime DtCriadoEm { get; set; } = DateTime.UtcNow;

        [Column("DT_ATUALIZADO_EM")]
        public DateTime? DtAtualizadoEm { get; set; }

        [Column("DT_DEL_EM")]
        public DateTime? DtDelEm { get; set; }

        [Column("ID_DEL_POR")]
        public int? IdDelPor { get; set; }

        [StringLength(255, ErrorMessage = "O motivo de exclusão deve ter no máximo 255 caracteres.")]
        [Column("DS_MOTIVO_EXCLUSAO")]
        public string? DsMotivoExclusao { get; set; }

        // Navigation properties
        [JsonIgnore]
        [ForeignKey("IdCliente")]
        public ClienteExternal? Cliente { get; set; }

        [JsonIgnore]
        [ForeignKey("IdDelPor")]
        public UsuarioExternal? UsuarioDeletor { get; set; }

        [JsonIgnore]
        public ICollection<ObservacaoClimatica> Observacoes { get; set; } = [];

        [JsonIgnore]
        public ICollection<Processamento> Processamentos { get; set; } = [];
    }
}