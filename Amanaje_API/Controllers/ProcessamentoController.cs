using Amanaje_API.Data;
using Amanaje_API.DTOs;
using Amanaje_API.Enums;
using Amanaje_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Amanaje_API.Controllers
{
    [Route("api/processamento")]
    [ApiController]
    public class ProcessamentoController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public ProcessamentoController(ApplicationContext context)
        {
            _context = context;
        }

        private static ProcessamentoResponseDto MapToResponse(Processamento x) =>
            new ProcessamentoResponseDto
            {
                IdProcessamento = x.IdProcessamento,
                IdRegiao = x.IdRegiao,
                IdUsuario = x.IdUsuario,
                TpProcess = x.TpProcess,
                StProcess = x.StProcess,
                DsOrigem = x.DsOrigem,
                DsParam = x.DsParam,
                DsResult = x.DsResult,
                DtInicio = x.DtInicio,
                DtFim = x.DtFim
            };

        [HttpGet]
        [SwaggerOperation(
            Summary = "Lista todos os processamentos",
            Description = "Retorna todos os processamentos registrados no sistema."
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem retornada com sucesso", type: typeof(IEnumerable<ProcessamentoResponseDto>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhum processamento encontrado")]
        [SwaggerResponse(statusCode: 400, description: "Erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetAllProcessamentos()
        {
            try
            {
                var resultado = await _context.Processamento
                    .Include(x => x.Regiao)
                    .ToListAsync();

                if (!resultado.Any())
                    return NoContent();

                return Ok(resultado.Select(MapToResponse));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Busca processamento por ID",
            Description = "Retorna um processamento específico pelo seu identificador."
        )]
        [SwaggerResponse(statusCode: 200, description: "Processamento retornado com sucesso", type: typeof(ProcessamentoResponseDto))]
        [SwaggerResponse(statusCode: 404, description: "Processamento não encontrado")]
        [SwaggerResponse(statusCode: 400, description: "Erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetProcessamentoById(int id)
        {
            try
            {
                var processamento = await _context.Processamento
                    .Include(x => x.Regiao)
                    .FirstOrDefaultAsync(x => x.IdProcessamento == id);

                if (processamento is null)
                    return NotFound();

                return Ok(MapToResponse(processamento));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("regiao/{idRegiao}")]
        [SwaggerOperation(
            Summary = "Lista processamentos por região",
            Description = "Retorna todos os processamentos vinculados a uma região monitorada específica."
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem retornada com sucesso", type: typeof(IEnumerable<ProcessamentoResponseDto>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhum processamento encontrado para esta região")]
        [SwaggerResponse(statusCode: 400, description: "Erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetProcessamentosByRegiao(int idRegiao)
        {
            try
            {
                var resultado = await _context.Processamento
                    .Include(x => x.Regiao)
                    .Where(x => x.IdRegiao == idRegiao)
                    .ToListAsync();

                if (!resultado.Any())
                    return NoContent();

                return Ok(resultado.Select(MapToResponse));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("status/{status}")]
        [SwaggerOperation(
            Summary = "Lista processamentos por status",
            Description = "Retorna todos os processamentos filtrados pelo status informado."
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem retornada com sucesso", type: typeof(IEnumerable<ProcessamentoResponseDto>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhum processamento encontrado para este status")]
        [SwaggerResponse(statusCode: 400, description: "Erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetProcessamentosByStatus(StatusProcessamento status)
        {
            try
            {
                var resultado = await _context.Processamento
                    .Include(x => x.Regiao)
                    .Where(x => x.StProcess == status.ToString())
                    .ToListAsync();

                if (!resultado.Any())
                    return NoContent();

                return Ok(resultado.Select(MapToResponse));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Registrar processamento",
            Description = "Inicia o registro de um novo processamento. O status inicial é sempre INICIADO."
        )]
        [SwaggerResponse(statusCode: 201, description: "Processamento registrado com sucesso", type: typeof(ProcessamentoResponseDto))]
        [SwaggerResponse(statusCode: 404, description: "Região informada não encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Erro ao registrar o processamento", type: typeof(string))]
        public async Task<IActionResult> CreateProcessamento(ProcessamentoRequestDto model)
        {
            try
            {
                if (model.IdRegiao is not null)
                {
                    var regiao = await _context.RegiaoMonitorada
                        .FirstOrDefaultAsync(x => x.IdRegiao == model.IdRegiao && x.StAtivo == "S");

                    if (regiao is null)
                        return NotFound($"Região com ID {model.IdRegiao} não encontrada ou inativa.");
                }

                var processamento = new Processamento
                {
                    IdRegiao = model.IdRegiao,
                    IdUsuario = model.IdUsuario,
                    TpProcess = model.TpProcess.ToString(),
                    StProcess = StatusProcessamento.INICIADO.ToString(),
                    DsOrigem = model.DsOrigem.Trim(),
                    DsParam = model.DsParam?.Trim(),
                    DtInicio = DateTime.UtcNow
                };

                _context.Processamento.Add(processamento);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetProcessamentoById), new { id = processamento.IdProcessamento }, MapToResponse(processamento));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}