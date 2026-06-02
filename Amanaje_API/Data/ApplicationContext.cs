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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // -------------------------
            // RegiaoMonitorada
            // -------------------------
            modelBuilder.Entity<RegiaoMonitorada>(entity =>
            {
                // Precisão dos decimais
                entity.Property(e => e.NrLatitude).HasPrecision(9, 6);
                entity.Property(e => e.NrLongitude).HasPrecision(9, 6);

                // Unique constraint: um cliente não pode ter duas regiões com o mesmo nome
                entity.HasIndex(e => new { e.IdCliente, e.NmRegiao })
                      .IsUnique()
                      .HasDatabaseName("UQ_AMANAJE_REGIAO_CLI_NOME");

                // CHECK constraints
                entity.ToTable(t =>
                {
                    t.HasCheckConstraint("CK_AMANAJE_REGIAO_LAT",
                        "NR_LATITUDE BETWEEN -90 AND 90");

                    t.HasCheckConstraint("CK_AMANAJE_REGIAO_LONG",
                        "NR_LONGITUDE BETWEEN -180 AND 180");

                    t.HasCheckConstraint("CK_AMANAJE_REGIAO_AREA",
                        "TP_AREA IN ('PONTE','ENCOSTA','AREA_RURAL','COMUNIDADE','PROPRIEDADE_PRIVADA','REGIAO_RIBEIRINHA','AREA_URBANA','OUTRA')");

                    t.HasCheckConstraint("CK_AMANAJE_REGIAO_VULN",
                        "NR_NIVEL_VULN BETWEEN 0 AND 100");

                    t.HasCheckConstraint("CK_AMANAJE_REGIAO_VISIB",
                        "TP_VISIB IN ('PRIVADA','INSTITUCIONAL','AGREGADA_PUBLICA')");

                    t.HasCheckConstraint("CK_AMANAJE_REGIAO_ATIVO",
                        "ST_ATIVO IN ('S','N')");

                    t.HasCheckConstraint("CK_AMANAJE_REGIAO_ESTADO",
                        "REGEXP_LIKE(SG_ESTADO, '^[A-Z]{2}$')");

                    t.HasCheckConstraint("CK_AMANAJE_REGIAO_DEL",
                        "(ST_ATIVO = 'S' AND DT_DEL_EM IS NULL) OR (ST_ATIVO = 'N')");
                });

                // Relacionamentos
                entity.HasOne(e => e.Cliente)
                      .WithMany()
                      .HasForeignKey(e => e.IdCliente)
                      .HasConstraintName("FK_REGIAO_CLI")
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.UsuarioDeletor)
                      .WithMany()
                      .HasForeignKey(e => e.IdDelPor)
                      .HasConstraintName("FK_REGIAO_DEL_POR")
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // -------------------------
            // ObservacaoClimatica
            // -------------------------
            modelBuilder.Entity<ObservacaoClimatica>(entity =>
            {
                // Precisão dos decimais
                entity.Property(e => e.NrTemperaturaC).HasPrecision(6, 2);
                entity.Property(e => e.NrUmidadePct).HasPrecision(5, 2);
                entity.Property(e => e.NrPrecipMm).HasPrecision(8, 2);
                entity.Property(e => e.NrVentoKmh).HasPrecision(8, 2);
                entity.Property(e => e.NrPressaoHpa).HasPrecision(8, 2);
                entity.Property(e => e.NrRadiacaoSolar).HasPrecision(10, 2);
                entity.Property(e => e.NrIndiceUv).HasPrecision(5, 2);

                // Unique constraint
                entity.HasIndex(e => new { e.IdObservacao, e.IdRegiao })
                      .IsUnique()
                      .HasDatabaseName("UQ_AMANAJE_OBS_ID_REGIAO");

                // CHECK constraints
                entity.ToTable(t =>
                {
                    t.HasCheckConstraint("CK_AMANAJE_OBS_UMIDADE",
                        "NR_UMIDADE_PCT IS NULL OR NR_UMIDADE_PCT BETWEEN 0 AND 100");

                    t.HasCheckConstraint("CK_AMANAJE_OBS_PREC",
                        "NR_PRECIP_MM IS NULL OR NR_PRECIP_MM >= 0");

                    t.HasCheckConstraint("CK_AMANAJE_OBS_VENTO",
                        "NR_VENTO_KMH IS NULL OR NR_VENTO_KMH >= 0");

                    t.HasCheckConstraint("CK_AMANAJE_OBS_PRESSAO",
                        "NR_PRESSAO_HPA IS NULL OR NR_PRESSAO_HPA BETWEEN 800 AND 1200");

                    t.HasCheckConstraint("CK_AMANAJE_OBS_UV",
                        "NR_INDICE_UV IS NULL OR NR_INDICE_UV BETWEEN 0 AND 20");
                });

                // Relacionamento
                entity.HasOne(e => e.Regiao)
                      .WithMany(r => r.Observacoes)
                      .HasForeignKey(e => e.IdRegiao)
                      .HasConstraintName("FK_OBS_REGIAO")
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // -------------------------
            // Processamento
            // -------------------------
            modelBuilder.Entity<Processamento>(entity =>
            {
                // CHECK constraints
                entity.ToTable(t =>
                {
                    t.HasCheckConstraint("CK_AMANAJE_PROCESS_TIPO",
                        "TP_PROCESS IN ('SINCRONIZACAO_CLIM','CALCULO_RISCO','GERACAO_IND','GERACAO_ALERTA','CARGA_DADOS','ROTINA_PL_SQL','OUTRO')");

                    t.HasCheckConstraint("CK_AMANAJE_PROCESS_STATUS",
                        "ST_PROCESS IN ('INICIADO','EM_EXECUCAO','CONCLUIDO','FALHOU','CANCELADO')");

                    t.HasCheckConstraint("CK_AMANAJE_PROCESS_FIM",
                        "DT_FIM IS NULL OR DT_FIM >= DT_INICIO");
                });

                // Relacionamentos
                entity.HasOne(e => e.Regiao)
                      .WithMany(r => r.Processamentos)
                      .HasForeignKey(e => e.IdRegiao)
                      .HasConstraintName("FK_PROCESS_REGIAO")
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Usuario)
                      .WithMany()
                      .HasForeignKey(e => e.IdUsuario)
                      .HasConstraintName("FK_PROCESS_USU")
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // -------------------------
            // Externos — somente leitura, sem migrations
            // -------------------------
            modelBuilder.Entity<ClienteExternal>().ToTable("TB_AMANAJE_CLI");
            modelBuilder.Entity<UsuarioExternal>().ToTable("TB_AMANAJE_USU");
        }
    }
}