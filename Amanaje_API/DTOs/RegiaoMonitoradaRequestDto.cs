using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Amanaje_API.DTOs
{
    public class RegiaoMonitoradaRequestDto
    {
        [Required(ErrorMessage = "O ID do cliente é obrigatório.")]
        [SwaggerSchema(Description = "ID do cliente responsável pela região")]
        public int IdCliente { get; set; }

        [Required(ErrorMessage = "O nome da região é obrigatório.")]
        [StringLength(150, MinimumLength = 1, ErrorMessage = "O nome da região deve ter entre 1 e 150 caracteres.")]
        [RegularExpression(@".*\S.*", ErrorMessage = "O nome da região não pode conter apenas espaços.")]
        [SwaggerSchema(Description = "Nome operacional da região monitorada")]
        public string NmRegiao { get; set; }

        [Required(ErrorMessage = "O nome da cidade é obrigatório.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "O nome da cidade deve ter entre 1 e 100 caracteres.")]
        [RegularExpression(@".*\S.*", ErrorMessage = "O nome da cidade não pode conter apenas espaços.")]
        [SwaggerSchema(Description = "Cidade onde a região está localizada")]
        public string NmCidade { get; set; }

        [Required(ErrorMessage = "A sigla do estado é obrigatória.")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "A sigla do estado deve ter exatamente 2 caracteres.")]
        [RegularExpression(@"^[A-Za-z]{2}$", ErrorMessage = "A sigla do estado deve conter apenas letras (ex: SP, RJ).")]
        [SwaggerSchema(Description = "Sigla do estado (UF) — ex: SP, RJ, MG")]
        public string SgEstado { get; set; }

        [Required(ErrorMessage = "A latitude é obrigatória.")]
        [Range(-90.0, 90.0, ErrorMessage = "A latitude deve estar entre -90 e 90.")]
        [SwaggerSchema(Description = "Latitude geográfica entre -90 e 90")]
        public decimal NrLatitude { get; set; }

        [Required(ErrorMessage = "A longitude é obrigatória.")]
        [Range(-180.0, 180.0, ErrorMessage = "A longitude deve estar entre -180 e 180.")]
        [SwaggerSchema(Description = "Longitude geográfica entre -180 e 180")]
        public decimal NrLongitude { get; set; }

        [Required(ErrorMessage = "O tipo de área é obrigatório.")]
        [RegularExpression(@"^(?i)(PONTE|ENCOSTA|AREA_RURAL|COMUNIDADE|PROPRIEDADE_PRIVADA|REGIAO_RIBEIRINHA|AREA_URBANA|OUTRA)$",
            ErrorMessage = "Tipo de área inválido. Valores aceitos: PONTE, ENCOSTA, AREA_RURAL, COMUNIDADE, PROPRIEDADE_PRIVADA, REGIAO_RIBEIRINHA, AREA_URBANA, OUTRA.")]
        [SwaggerSchema(Description = "Tipo da área — valores aceitos: PONTE, ENCOSTA, AREA_RURAL, COMUNIDADE, PROPRIEDADE_PRIVADA, REGIAO_RIBEIRINHA, AREA_URBANA, OUTRA")]
        public string TpArea { get; set; }

        [Required(ErrorMessage = "O nível de vulnerabilidade é obrigatório.")]
        [Range(0, 100, ErrorMessage = "O nível de vulnerabilidade deve estar entre 0 e 100.")]
        [SwaggerSchema(Description = "Índice de vulnerabilidade entre 0 e 100")]
        public int NrNivelVuln { get; set; }

        [Required(ErrorMessage = "O tipo de visibilidade é obrigatório.")]
        [RegularExpression(@"^(?i)(PRIVADA|INSTITUCIONAL|AGREGADA_PUBLICA)$",
            ErrorMessage = "Tipo de visibilidade inválido. Valores aceitos: PRIVADA, INSTITUCIONAL, AGREGADA_PUBLICA.")]
        [SwaggerSchema(Description = "Visibilidade dos dados — valores aceitos: PRIVADA, INSTITUCIONAL, AGREGADA_PUBLICA")]
        public string TpVisib { get; set; }
    }
}