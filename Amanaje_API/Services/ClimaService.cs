using System.Net.Http.Json;
using Amanaje_API.Data;
using Amanaje_API.DTOs;
using Amanaje_API.Enums;
using Amanaje_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Amanaje_API.Services
{
    public class ClimaService : IClimaService
    {
        private readonly ApplicationContext _context;
        private readonly HttpClient _httpClient;
        private const string NomeFonte = "OpenMeteo";

        public ClimaService(ApplicationContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        public async Task<ObservacaoClimatica> SincronizarAsync(int idRegiao)
        {
            // 1. Busca a região no banco
            var regiao = await _context.RegiaoMonitorada
                .FirstOrDefaultAsync(r => r.IdRegiao == idRegiao && r.StAtivo == "S")
                ?? throw new KeyNotFoundException($"Região com ID {idRegiao} não encontrada ou inativa.");

            // 2. Registra o início do processamento
            var processamento = new Processamento
            {
                IdRegiao = idRegiao,
                TpProcess = TipoProcessamento.SINCRONIZACAO_CLIM.ToString(),
                StProcess = StatusProcessamento.EM_EXECUCAO.ToString(),
                DsOrigem = "Amanaje_API - ClimaService",
                DsParam = $"lat={regiao.NrLatitude}&lon={regiao.NrLongitude}",
                DtInicio = DateTime.UtcNow
            };

            _context.Processamento.Add(processamento);
            await _context.SaveChangesAsync();

            try
            {
                // 3. Monta a URL e chama a OpenMeteo
                var url = $"https://api.open-meteo.com/v1/forecast" +
                          $"?latitude={regiao.NrLatitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}" +
                          $"&longitude={regiao.NrLongitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}" +
                          $"&current=temperature_2m,relative_humidity_2m,precipitation," +
                          $"wind_speed_10m,surface_pressure,shortwave_radiation,uv_index";

                var resposta = await _httpClient.GetFromJsonAsync<OpenMeteoResponseDto>(url)
                    ?? throw new Exception("Resposta inválida da OpenMeteo.");

                var current = resposta.Current
                    ?? throw new Exception("Bloco 'current' ausente na resposta da OpenMeteo.");

                // 4. Normaliza e persiste a observação climática
                var observacao = new ObservacaoClimatica
                {
                    IdRegiao = idRegiao,
                    NmFonte = NomeFonte,
                    NrTemperaturaC = current.Temperature_2m.HasValue ? (decimal)current.Temperature_2m.Value : null,
                    NrUmidadePct = current.Relative_Humidity_2m.HasValue ? (decimal)current.Relative_Humidity_2m.Value : null,
                    NrPrecipMm = current.Precipitation.HasValue ? (decimal)current.Precipitation.Value : null,
                    NrVentoKmh = current.Wind_Speed_10m.HasValue ? (decimal)current.Wind_Speed_10m.Value : null,
                    NrPressaoHpa = current.Surface_Pressure.HasValue ? (decimal)current.Surface_Pressure.Value : null,
                    NrRadiacaoSolar = current.Shortwave_Radiation.HasValue ? (decimal)current.Shortwave_Radiation.Value : null,
                    NrIndiceUv = current.Uv_Index.HasValue ? (decimal)current.Uv_Index.Value : null,
                    DtObs = DateTime.TryParse(current.Time, out var dtObs) ? dtObs : DateTime.UtcNow,
                    DtCriadoEm = DateTime.UtcNow
                };

                _context.ObservacaoClimatica.Add(observacao);

                // 5. Atualiza o processamento como concluído
                processamento.StProcess = StatusProcessamento.CONCLUIDO.ToString();
                processamento.DsResult = $"Observação registrada com sucesso. Fonte: {NomeFonte}";
                processamento.DtFim = DateTime.UtcNow;

                _context.Processamento.Update(processamento);
                await _context.SaveChangesAsync();

                return observacao;
            }
            catch (Exception ex)
            {
                // 6. Atualiza o processamento como falho
                processamento.StProcess = StatusProcessamento.FALHOU.ToString();
                processamento.DsResult = $"Erro: {ex.Message}";
                processamento.DtFim = DateTime.UtcNow;

                _context.Processamento.Update(processamento);
                await _context.SaveChangesAsync();

                throw;
            }
        }

        public IEnumerable<string> ListarFontes()
        {
            return new List<string> { NomeFonte };
        }
    }
}