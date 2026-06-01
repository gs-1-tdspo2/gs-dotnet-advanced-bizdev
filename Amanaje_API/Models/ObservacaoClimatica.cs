using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Amanaje_API.Models
{
    [Table("TB_AMANAJE_OBS_CLIM")]
    public class ObservacaoClimatica
    {
        [Key]
        [Column("ID_OBSERVACAO")]
        public int IdObservacao { get; set; }

        [Required(ErrorMessage = "O ID da região é obrigatório.")]
        [Column("ID_REGIAO")]
        public int IdRegiao { get; set; }

        [Required(ErrorMessage = "A fonte dos dados climáticos é obrigatória.")]
        [StringLength(80, MinimumLength = 1, ErrorMessage = "A fonte deve ter entre 1 e 80 caracteres.")]
        [Column("NM_FONTE")]
        public string NmFonte { get; set; } = string.Empty;

        [Column("NR_TEMPERATURA_C")]
        public decimal? NrTemperaturaC { get; set; }

        [Range(0, 100, ErrorMessage = "A umidade relativa deve estar entre 0 e 100%.")]
        [Column("NR_UMIDADE_PCT")]
        public decimal? NrUmidadePct { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "A precipitação não pode ser negativa.")]
        [Column("NR_PRECIP_MM")]
        public decimal? NrPrecipMm { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "A velocidade do vento não pode ser negativa.")]
        [Column("NR_VENTO_KMH")]
        public decimal? NrVentoKmh { get; set; }

        [Range(800, 1200, ErrorMessage = "A pressão atmosférica deve estar entre 800 e 1200 hPa.")]
        [Column("NR_PRESSAO_HPA")]
        public decimal? NrPressaoHpa { get; set; }

        [Column("NR_RADIACAO_SOLAR")]
        public decimal? NrRadiacaoSolar { get; set; }

        [Range(0, 20, ErrorMessage = "O índice UV deve estar entre 0 e 20.")]
        [Column("NR_INDICE_UV")]
        public decimal? NrIndiceUv { get; set; }

        [Required(ErrorMessage = "A data da observação é obrigatória.")]
        [Column("DT_OBS")]
        public DateTime DtObs { get; set; }

        [Required]
        [Column("DT_CRIADO_EM")]
        public DateTime DtCriadoEm { get; set; } = DateTime.UtcNow;

        // Navigation property
        [JsonIgnore]
        [ForeignKey("IdRegiao")]
        public RegiaoMonitorada? Regiao { get; set; }
    }
}