using Amanaje_API.Models;
using Amanaje_API.Models.Externals;
using Microsoft.EntityFrameworkCore;

namespace Amanaje_API.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }
        
        public DbSet<ObservacaoClimatica> ObservacaoClimatica { get; set; }
        public DbSet<Processamento> Processamento { get; set; }
        public DbSet<RegiaoMonitorada> RegiaoMonitorada { get; set; }

        // Classes externas trabalhadas na API Java, usadas para consulta/validação
        public DbSet<ClienteExternal> ClienteExternal { get; set; }
        public DbSet<UsuarioExternal> UsuarioExternal { get; set; }
    }
}
