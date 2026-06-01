using Amanaje_API.Data;
using Amanaje_API.DTOs;
using Amanaje_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Amanaje_API.Controllers
{
    [Route("api/observacao")]
    [ApiController]
    public class ObservacaoClimaticaController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public ObservacaoClimaticaController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Lista todas as observações climáticas",
            Description = "Retorna todas as observações climáticas registradas no sistema."
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem retornada com sucesso", type: typeof(IEnumerable<ObservacaoClimaticaResponseDto>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhuma observação encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetAllObservacoes()
        {
            try
            {
                var resultado = await _context.ObservacaoClimatica
                    .Include(x => x.Regiao)
                    .ToListAsync();

                if (!resultado.Any())
                    return NoContent();

                var response = resultado.Select(x => new ObservacaoClimaticaResponseDto
                {
                    IdObservacao = x.IdObservacao,
                    IdRegiao = x.IdRegiao,
                    NmFonte = x.NmFonte,
                    NrTemperaturaC = x.NrTemperaturaC,
                    NrUmidadePct = x.NrUmidadePct,
                    NrPrecipMm = x.NrPrecipMm,
                    NrVentoKmh = x.NrVentoKmh,
                    NrPressaoHpa = x.NrPressaoHpa,
                    NrRadiacaoSolar = x.NrRadiacaoSolar,
                    NrIndiceUv = x.NrIndiceUv,
                    DtObs = x.DtObs,
                    DtCriadoEm = x.DtCriadoEm
                });

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Busca observação climática por ID",
            Description = "Retorna uma observação climática específica pelo seu identificador."
        )]
        [SwaggerResponse(statusCode: 200, description: "Observação retornada com sucesso", type: typeof(ObservacaoClimaticaResponseDto))]
        [SwaggerResponse(statusCode: 404, description: "Observação não encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetObservacaoById(int id)
        {
            try
            {
                var observacao = await _context.ObservacaoClimatica
                    .Include(x => x.Regiao)
                    .FirstOrDefaultAsync(x => x.IdObservacao == id);

                if (observacao is null)
                    return NotFound();

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

        [HttpGet("regiao/{idRegiao}")]
        [SwaggerOperation(
            Summary = "Lista observações por região",
            Description = "Retorna todas as observações climáticas vinculadas a uma região monitorada específica."
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem retornada com sucesso", type: typeof(IEnumerable<ObservacaoClimaticaResponseDto>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhuma observação encontrada para esta região")]
        [SwaggerResponse(statusCode: 400, description: "Erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetObservacoesByRegiao(int idRegiao)
        {
            try
            {
                var resultado = await _context.ObservacaoClimatica
                    .Include(x => x.Regiao)
                    .Where(x => x.IdRegiao == idRegiao)
                    .ToListAsync();

                if (!resultado.Any())
                    return NoContent();

                var response = resultado.Select(x => new ObservacaoClimaticaResponseDto
                {
                    IdObservacao = x.IdObservacao,
                    IdRegiao = x.IdRegiao,
                    NmFonte = x.NmFonte,
                    NrTemperaturaC = x.NrTemperaturaC,
                    NrUmidadePct = x.NrUmidadePct,
                    NrPrecipMm = x.NrPrecipMm,
                    NrVentoKmh = x.NrVentoKmh,
                    NrPressaoHpa = x.NrPressaoHpa,
                    NrRadiacaoSolar = x.NrRadiacaoSolar,
                    NrIndiceUv = x.NrIndiceUv,
                    DtObs = x.DtObs,
                    DtCriadoEm = x.DtCriadoEm
                });

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("regiao/{idRegiao}/ultima")]
        [SwaggerOperation(
            Summary = "Busca última observação de uma região",
            Description = "Retorna a observação climática mais recente vinculada a uma região monitorada."
        )]
        [SwaggerResponse(statusCode: 200, description: "Observação retornada com sucesso", type: typeof(ObservacaoClimaticaResponseDto))]
        [SwaggerResponse(statusCode: 404, description: "Nenhuma observação encontrada para esta região")]
        [SwaggerResponse(statusCode: 400, description: "Erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetUltimaObservacaoByRegiao(int idRegiao)
        {
            try
            {
                var observacao = await _context.ObservacaoClimatica
                    .Include(x => x.Regiao)
                    .Where(x => x.IdRegiao == idRegiao)
                    .OrderByDescending(x => x.DtObs)
                    .FirstOrDefaultAsync();

                if (observacao is null)
                    return NotFound();

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

        [HttpPost]
        [SwaggerOperation(
            Summary = "Registrar observação climática",
            Description = "Registra manualmente uma nova observação climática vinculada a uma região monitorada ativa."
        )]
        [SwaggerResponse(statusCode: 201, description: "Observação registrada com sucesso", type: typeof(ObservacaoClimaticaResponseDto))]
        [SwaggerResponse(statusCode: 404, description: "Região informada não encontrada ou inativa")]
        [SwaggerResponse(statusCode: 400, description: "Erro ao registrar a observação", type: typeof(string))]
        public async Task<IActionResult> CreateObservacao(ObservacaoClimaticaRequestDto model)
        {
            try
            {
                var regiao = await _context.RegiaoMonitorada
                    .FirstOrDefaultAsync(x => x.IdRegiao == model.IdRegiao && x.StAtivo == "S");

                if (regiao is null)
                    return NotFound($"Região com ID {model.IdRegiao} não encontrada ou inativa.");

                var observacao = new ObservacaoClimatica
                {
                    IdRegiao = model.IdRegiao,
                    NmFonte = model.NmFonte,
                    NrTemperaturaC = model.NrTemperaturaC,
                    NrUmidadePct = model.NrUmidadePct,
                    NrPrecipMm = model.NrPrecipMm,
                    NrVentoKmh = model.NrVentoKmh,
                    NrPressaoHpa = model.NrPressaoHpa,
                    NrRadiacaoSolar = model.NrRadiacaoSolar,
                    NrIndiceUv = model.NrIndiceUv,
                    DtObs = model.DtObs,
                    DtCriadoEm = DateTime.UtcNow
                };

                _context.ObservacaoClimatica.Add(observacao);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetObservacaoById), new { id = observacao.IdObservacao }, observacao);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Remover observação climática",
            Description = "Remove permanentemente uma observação climática. Observações são dados imutáveis de telemetria, portanto não possuem soft delete."
        )]
        [SwaggerResponse(statusCode: 200, description: "Observação removida com sucesso", type: typeof(ObservacaoClimaticaResponseDto))]
        [SwaggerResponse(statusCode: 404, description: "Observação não encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Erro ao remover a observação", type: typeof(string))]
        public async Task<IActionResult> DeleteObservacao(int id)
        {
            try
            {
                var observacao = await _context.ObservacaoClimatica
                    .FirstOrDefaultAsync(x => x.IdObservacao == id);

                if (observacao is null)
                    return NotFound();

                _context.ObservacaoClimatica.Remove(observacao);
                await _context.SaveChangesAsync();

                return Ok(observacao);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}