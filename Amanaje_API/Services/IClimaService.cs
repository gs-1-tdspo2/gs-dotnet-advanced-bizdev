using Amanaje_API.Models;

namespace Amanaje_API.Services
{
    public interface IClimaService
    {
        Task<ObservacaoClimatica> SincronizarAsync(int idRegiao);
        IEnumerable<string> ListarFontes();
    }
}