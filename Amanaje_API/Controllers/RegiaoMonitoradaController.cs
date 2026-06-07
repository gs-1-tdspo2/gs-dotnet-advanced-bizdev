using Amanaje_API.Data;
using Amanaje_API.DTOs;
using Amanaje_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Amanaje_API.Controllers
{
    [Route("api/regiao")]
    [ApiController]
    public class RegiaoMonitoradaController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public RegiaoMonitoradaController(ApplicationContext context)
        {
            _context = context;
        }

        private static RegiaoMonitoradaResponseDto MapToResponse(RegiaoMonitorada x) =>
            new RegiaoMonitoradaResponseDto
            {
                IdRegiao = x.IdRegiao,
                IdCliente = x.IdCliente,
                NmRegiao = x.NmRegiao,
                NmCidade = x.NmCidade,
                SgEstado = x.SgEstado,
                NrLatitude = x.NrLatitude,
                NrLongitude = x.NrLongitude,
                TpArea = x.TpArea,
                NrNivelVuln = x.NrNivelVuln,
                TpVisib = x.TpVisib,
                StAtivo = x.StAtivo,
                DtCriadoEm = x.DtCriadoEm,
                DtAtualizadoEm = x.DtAtualizadoEm
            };

        [HttpGet]
        [SwaggerOperation(
            Summary = "Lista todas as regiões monitoradas",
            Description = "Retorna todas as regiões cadastradas no sistema."
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem retornada com sucesso", type: typeof(IEnumerable<RegiaoMonitoradaResponseDto>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhuma região encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetAllRegioes()
        {
            try
            {
                var resultado = await _context.RegiaoMonitorada
                    .Include(x => x.Cliente)
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

        [HttpGet("ativas")]
        [SwaggerOperation(
            Summary = "Lista regiões monitoradas ativas",
            Description = "Retorna apenas as regiões com status ativo."
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem retornada com sucesso", type: typeof(IEnumerable<RegiaoMonitoradaResponseDto>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhuma região ativa encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetRegioesAtivas()
        {
            try
            {
                var resultado = await _context.RegiaoMonitorada
                    .Include(x => x.Cliente)
                    .Where(x => x.StAtivo == "S")
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

        [HttpGet("inativas")]
        [SwaggerOperation(
            Summary = "Lista regiões monitoradas inativas",
            Description = "Retorna apenas as regiões excluídas logicamente."
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem retornada com sucesso", type: typeof(IEnumerable<RegiaoMonitoradaResponseDto>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhuma região inativa encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetRegioesInativas()
        {
            try
            {
                var resultado = await _context.RegiaoMonitorada
                    .Include(x => x.Cliente)
                    .Where(x => x.StAtivo == "N")
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
            Summary = "Busca região monitorada por ID",
            Description = "Retorna uma região monitorada específica pelo seu identificador."
        )]
        [SwaggerResponse(statusCode: 200, description: "Região retornada com sucesso", type: typeof(RegiaoMonitoradaResponseDto))]
        [SwaggerResponse(statusCode: 404, description: "Região não encontrada")]
        [SwaggerResponse(statusCode: 400, description: "Erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetRegiaoById(int id)
        {
            try
            {
                var regiao = await _context.RegiaoMonitorada
                    .Include(x => x.Cliente)
                    .FirstOrDefaultAsync(x => x.IdRegiao == id);

                if (regiao is null)
                    return NotFound();

                return Ok(MapToResponse(regiao));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("cliente/{idCliente}")]
        [SwaggerOperation(
            Summary = "Lista regiões por cliente",
            Description = "Retorna todas as regiões ativas vinculadas a um cliente específico."
        )]
        [SwaggerResponse(statusCode: 200, description: "Listagem retornada com sucesso", type: typeof(IEnumerable<RegiaoMonitoradaResponseDto>))]
        [SwaggerResponse(statusCode: 204, description: "Nenhuma região encontrada para este cliente")]
        [SwaggerResponse(statusCode: 400, description: "Erro ao retornar os dados", type: typeof(string))]
        public async Task<IActionResult> GetRegioesByCliente(int idCliente)
        {
            try
            {
                var resultado = await _context.RegiaoMonitorada
                    .Include(x => x.Cliente)
                    .Where(x => x.StAtivo == "S" && x.IdCliente == idCliente)
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
            Summary = "Cadastrar região monitorada",
            Description = "Cria uma nova região monitorada vinculada a um cliente existente."
        )]
        [SwaggerResponse(statusCode: 201, description: "Região criada com sucesso", type: typeof(RegiaoMonitoradaResponseDto))]
        [SwaggerResponse(statusCode: 404, description: "Cliente informado não encontrado")]
        [SwaggerResponse(statusCode: 400, description: "Erro ao criar a região", type: typeof(string))]
        public async Task<IActionResult> CreateRegiao(RegiaoMonitoradaCreateDto model)
        {
            try
            {
                var cliente = await _context.ClienteExternal
                    .FirstOrDefaultAsync(x => x.Id == model.IdCliente);

                if (cliente is null)
                    return NotFound($"Cliente com ID {model.IdCliente} não encontrado.");

                var nomeNormalizado = model.NmRegiao.Trim();
                var regiaoExistente = await _context.RegiaoMonitorada
                    .FirstOrDefaultAsync(x => x.IdCliente == model.IdCliente && x.NmRegiao == nomeNormalizado);

                if (regiaoExistente is not null)
                    return Conflict($"Já existe uma região chamada '{nomeNormalizado}' para o cliente com ID {model.IdCliente}.");

                var regiao = new RegiaoMonitorada
                {
                    IdCliente = model.IdCliente,
                    NmRegiao = nomeNormalizado,
                    NmCidade = model.NmCidade.Trim(),
                    SgEstado = model.SgEstado.Trim().ToUpper(),
                    NrLatitude = model.NrLatitude,
                    NrLongitude = model.NrLongitude,
                    TpArea = model.TpArea.ToUpper(),
                    NrNivelVuln = model.NrNivelVuln,
                    TpVisib = model.TpVisib.ToUpper(),
                    StAtivo = "S",
                    DtCriadoEm = DateTime.UtcNow
                };

                _context.RegiaoMonitorada.Add(regiao);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetRegiaoById), new { id = regiao.IdRegiao }, MapToResponse(regiao));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Atualizar região monitorada",
            Description = "Atualiza os dados de uma região monitorada ativa existente."
        )]
        [SwaggerResponse(statusCode: 200, description: "Região atualizada com sucesso", type: typeof(RegiaoMonitoradaResponseDto))]
        [SwaggerResponse(statusCode: 404, description: "Região não encontrada ou inativa")]
        [SwaggerResponse(statusCode: 400, description: "Erro ao atualizar os dados", type: typeof(string))]
        public async Task<IActionResult> UpdateRegiao(int id, RegiaoMonitoradaUpdateDto model)
        {
            try
            {
                var regiao = await _context.RegiaoMonitorada
                    .Where(x => x.StAtivo == "S")
                    .FirstOrDefaultAsync(x => x.IdRegiao == id);

                if (regiao is null)
                    return NotFound();

                var nomeAtualizado = model.NmRegiao.Trim();
                var regiaoComMesmoNome = await _context.RegiaoMonitorada
                    .FirstOrDefaultAsync(x => x.IdCliente == regiao.IdCliente && x.NmRegiao == nomeAtualizado && x.IdRegiao != id);

                if (regiaoComMesmoNome is not null)
                    return Conflict($"Já existe outra região chamada '{nomeAtualizado}' para este cliente.");

                regiao.NmRegiao = nomeAtualizado;
                regiao.NmCidade = model.NmCidade.Trim();
                regiao.SgEstado = model.SgEstado.Trim().ToUpper();
                regiao.NrLatitude = model.NrLatitude;
                regiao.NrLongitude = model.NrLongitude;
                regiao.TpArea = model.TpArea.ToUpper();
                regiao.NrNivelVuln = model.NrNivelVuln;
                regiao.TpVisib = model.TpVisib.ToUpper();
                regiao.DtAtualizadoEm = DateTime.UtcNow;

                _context.RegiaoMonitorada.Update(regiao);
                await _context.SaveChangesAsync();

                return Ok(MapToResponse(regiao));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("reativar/{id}")]
        [SwaggerOperation(
            Summary = "Reativar região monitorada",
            Description = "Restaura uma região monitorada previamente inativada."
        )]
        [SwaggerResponse(statusCode: 200, description: "Região reativada com sucesso", type: typeof(RegiaoMonitoradaResponseDto))]
        [SwaggerResponse(statusCode: 404, description: "Região não encontrada ou já está ativa")]
        [SwaggerResponse(statusCode: 400, description: "Erro ao reativar a região", type: typeof(string))]
        public async Task<IActionResult> ReativarRegiao(int id)
        {
            try
            {
                var regiao = await _context.RegiaoMonitorada
                    .Where(x => x.StAtivo == "N")
                    .FirstOrDefaultAsync(x => x.IdRegiao == id);

                if (regiao is null)
                    return NotFound();

                regiao.StAtivo = "S";
                regiao.DtDelEm = null;
                regiao.IdDelPor = null;
                regiao.DsMotivoExclusao = null;
                regiao.DtAtualizadoEm = DateTime.UtcNow;

                _context.RegiaoMonitorada.Update(regiao);
                await _context.SaveChangesAsync();

                return Ok(MapToResponse(regiao));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Inativar região monitorada (soft delete)",
            Description = "Realiza a exclusão lógica da região, preservando o histórico no banco."
        )]
        [SwaggerResponse(statusCode: 200, description: "Região inativada com sucesso", type: typeof(RegiaoMonitoradaResponseDto))]
        [SwaggerResponse(statusCode: 404, description: "Região não encontrada ou já está inativa")]
        [SwaggerResponse(statusCode: 400, description: "Erro ao inativar a região", type: typeof(string))]
        public async Task<IActionResult> DeleteRegiao(int id)
        {
            try
            {
                var regiao = await _context.RegiaoMonitorada
                    .Where(x => x.StAtivo == "S")
                    .FirstOrDefaultAsync(x => x.IdRegiao == id);

                if (regiao is null)
                    return NotFound();

                regiao.StAtivo = "N";
                regiao.DtDelEm = DateTime.UtcNow;

                _context.RegiaoMonitorada.Update(regiao);
                await _context.SaveChangesAsync();

                return Ok(MapToResponse(regiao));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}