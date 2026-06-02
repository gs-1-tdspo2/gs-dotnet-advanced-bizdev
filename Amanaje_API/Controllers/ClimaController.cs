using Amanaje_API.Data;
using Amanaje_API.DTOs;
using Amanaje_API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Amanaje_API.Controllers
{
    [Route("api/clima")]
    [ApiController]
    public class ClimaController : ControllerBase
    {
        private readonly IClimaService _climaService;
        private readonly ApplicationContext _context;

        public ClimaController(IClimaService climaService, ApplicationContext context)
        {
            _climaService = climaService;
            _context = context;
        }

        [HttpPost("sincronizar/{idRegiao}")]
        [SwaggerOperation(
            Summary = "Sincronizar dados climáticos externos",
            Description = "Busca dados climáticos públicos da OpenMeteo para a região informada, normaliza e persiste a observação no banco."
        )]
        [SwaggerResponse(statusCode: 200, description: "Dados climáticos sincronizados com sucesso", type: typeof(ObservacaoClimaticaResponseDto))]
        [SwaggerResponse(statusCode: 404, description: "Região não encontrada ou inativa")]
        [SwaggerResponse(statusCode: 400, description: "Erro ao sincronizar dados climáticos", type: typeof(string))]
        public async Task<IActionResult> Sincronizar(int idRegiao)
        {
            try
            {
                var observacao = await _climaService.SincronizarAsync(idRegiao);

                var response = new ObservacaoClimaticaResponseDto
                {
                    IdObservacao = observacao.IdObservacao,
                    IdRegiao = observacao.IdRegiao,
                    NmFonte = observacao.NmFonte,
                    NrTemperaturaC = observacao.NrTemperaturaC,
                    NrUmidadePct = observacao.NrUmidadePct,
                    NrPrecipMm = observacao.NrPrecipMm,
                    NrVentoKmh = observacao.NrVentoKmh,
                    NrPressaoHpa = observacao.NrPressaoHpa,
                    NrRadiacaoSolar = observacao.NrRadiacaoSolar,
                    NrIndiceUv = observacao.NrIndiceUv,
                    DtObs = observacao.DtObs,
                    DtCriadoEm = observacao.DtCriadoEm
                };

                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("regiao/{idRegiao}/ultima")]
        [SwaggerOperation(
            Summary = "Consultar última observação climática externa",
            Description = "Retorna a observação climática externa mais recente registrada para a região informada."
        )]
        [SwaggerResponse(statusCode: 200, description: "Observação retornada com sucesso", type: typeof(ObservacaoClimaticaResponseDto))]
        [SwaggerResponse(statusCode: 404, description: "Nenhuma observação encontrada para esta região")]
        [SwaggerResponse(statusCode: 400, description: "Erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetUltimaObservacao(int idRegiao)
        {
            try
            {
                var observacao = await _context.ObservacaoClimatica
                    .Where(x => x.IdRegiao == idRegiao)
                    .OrderByDescending(x => x.DtObs)
                    .FirstOrDefaultAsync();

                if (observacao is null)
                    return NotFound($"Nenhuma observação climática encontrada para a região com ID {idRegiao}.");

                var response = new ObservacaoClimaticaResponseDto
                {
                    IdObservacao = observacao.IdObservacao,
                    IdRegiao = observacao.IdRegiao,
                    NmFonte = observacao.NmFonte,
                    NrTemperaturaC = observacao.NrTemperaturaC,
                    NrUmidadePct = observacao.NrUmidadePct,
                    NrPrecipMm = observacao.NrPrecipMm,
                    NrVentoKmh = observacao.NrVentoKmh,
                    NrPressaoHpa = observacao.NrPressaoHpa,
                    NrRadiacaoSolar = observacao.NrRadiacaoSolar,
                    NrIndiceUv = observacao.NrIndiceUv,
                    DtObs = observacao.DtObs,
                    DtCriadoEm = observacao.DtCriadoEm
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("fontes")]
        [SwaggerOperation(
            Summary = "Listar fontes climáticas suportadas",
            Description = "Retorna a lista de fontes públicas de dados climáticos configuradas no serviço."
        )]
        [SwaggerResponse(statusCode: 200, description: "Fontes retornadas com sucesso", type: typeof(IEnumerable<string>))]
        public IActionResult GetFontes()
        {
            var fontes = _climaService.ListarFontes();
            return Ok(fontes);
        }
    }
}