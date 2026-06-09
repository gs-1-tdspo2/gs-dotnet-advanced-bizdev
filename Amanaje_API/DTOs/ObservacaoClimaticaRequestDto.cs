using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Amanaje_API.DTOs
{
    public class ObservacaoClimaticaRequestDto
    {
        [Required(ErrorMessage = "O ID da região é obrigatório.")]
        [SwaggerSchema(Description = "ID da região monitorada ativa")]
        public int IdRegiao { get; set; }

        [Required(ErrorMessage = "A fonte dos dados climáticos é obrigatória.")]
        [StringLength(80, MinimumLength = 1, ErrorMessage = "A fonte deve ter entre 1 e 80 caracteres.")]
        [RegularExpression(@".*\S.*", ErrorMessage = "A fonte não pode conter apenas espaços.")]
        [SwaggerSchema(Description = "Nome da fonte dos dados climáticos")]
        public string NmFonte { get; set; } = "Manual";

        [SwaggerSchema(Description = "Temperatura em graus Celsius")]
        public decimal? NrTemperaturaC { get; set; }

        [Range(0, 100, ErrorMessage = "A umidade relativa deve estar entre 0 e 100%.")]
        [SwaggerSchema(Description = "Umidade relativa do ar em % (0 a 100)")]
        public decimal? NrUmidadePct { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "A precipitação não pode ser negativa.")]
        [SwaggerSchema(Description = "Precipitação em milímetros (>= 0)")]
        public decimal? NrPrecipMm { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "A velocidade do vento não pode ser negativa.")]
        [SwaggerSchema(Description = "Velocidade do vento em km/h (>= 0)")]
        public decimal? NrVentoKmh { get; set; }

        [Range(800, 1200, ErrorMessage = "A pressão atmosférica deve estar entre 800 e 1200 hPa.")]
        [SwaggerSchema(Description = "Pressão atmosférica em hPa (800 a 1200)")]
        public decimal? NrPressaoHpa { get; set; }

        [SwaggerSchema(Description = "Radiação solar (sem restrição de range)")]
        public decimal? NrRadiacaoSolar { get; set; }

        [Range(0, 20, ErrorMessage = "O índice UV deve estar entre 0 e 20.")]
        [SwaggerSchema(Description = "Índice UV (0 a 20)")]
        public decimal? NrIndiceUv { get; set; }

        [Required(ErrorMessage = "A data da observação é obrigatória.")]
        [SwaggerSchema(Description = "Data e hora da observação climática")]
        public DateTime DtObs { get; set; } = DateTime.UtcNow;
    }
}