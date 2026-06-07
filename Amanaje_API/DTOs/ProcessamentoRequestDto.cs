using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Amanaje_API.DTOs
{
    public class ProcessamentoRequestDto
    {
        [SwaggerSchema(Description = "ID da região monitorada relacionada (opcional)")]
        public int? IdRegiao { get; set; } = 1;

        [SwaggerSchema(Description = "ID do usuário responsável (opcional)")]
        public int? IdUsuario { get; set; } = null;

        [Required(ErrorMessage = "O tipo de processamento é obrigatório.")]
        [RegularExpression(@"^(SINCRONIZACAO_CLIM|CALCULO_RISCO|GERACAO_IND|GERACAO_ALERTA|CARGA_DADOS|ROTINA_PL_SQL|OUTRO)$",
            ErrorMessage = "Tipo de processamento inválido. Valores aceitos: SINCRONIZACAO_CLIM, CALCULO_RISCO, GERACAO_IND, GERACAO_ALERTA, CARGA_DADOS, ROTINA_PL_SQL, OUTRO.")]
        [SwaggerSchema(Description = "Tipo do processamento — valores aceitos: SINCRONIZACAO_CLIM, CALCULO_RISCO, GERACAO_IND, GERACAO_ALERTA, CARGA_DADOS, ROTINA_PL_SQL, OUTRO")]
        public string TpProcess { get; set; } = "SINCRONIZACAO_CLIM";

        [Required(ErrorMessage = "A origem do processamento é obrigatória.")]
        [StringLength(120, MinimumLength = 1, ErrorMessage = "A origem deve ter entre 1 e 120 caracteres.")]
        [RegularExpression(@".*\S.*", ErrorMessage = "A origem não pode conter apenas espaços.")]
        [SwaggerSchema(Description = "Origem do processamento — ex: API Java, Amanaje_API, Script PL/SQL")]
        public string DsOrigem { get; set; } = "Amanaje_API";

        [StringLength(1000, ErrorMessage = "Os parâmetros devem ter no máximo 1000 caracteres.")]
        [SwaggerSchema(Description = "Parâmetros de entrada usados na execução (opcional)")]
        public string? DsParam { get; set; } = null;
    }
}