using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Amanaje_API.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_AMANAJE_CLI",
                columns: table => new
                {
                    ID_CLIENTE = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_AMANAJE_CLI", x => x.ID_CLIENTE);
                });

            migrationBuilder.CreateTable(
                name: "TB_AMANAJE_USU",
                columns: table => new
                {
                    ID_USUARIO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_AMANAJE_USU", x => x.ID_USUARIO);
                });

            migrationBuilder.CreateTable(
                name: "TB_AMANAJE_REGIAO_MONIT",
                columns: table => new
                {
                    ID_REGIAO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ID_CLIENTE = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    NM_REGIAO = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: false),
                    NM_CIDADE = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    SG_ESTADO = table.Column<string>(type: "NVARCHAR2(2)", maxLength: 2, nullable: false),
                    NR_LATITUDE = table.Column<decimal>(type: "DECIMAL(9,6)", precision: 9, scale: 6, nullable: false),
                    NR_LONGITUDE = table.Column<decimal>(type: "DECIMAL(9,6)", precision: 9, scale: 6, nullable: false),
                    TP_AREA = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    NR_NIVEL_VULN = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    TP_VISIB = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    ST_ATIVO = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    DT_CRIADO_EM = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    DT_ATUALIZADO_EM = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DT_DEL_EM = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    ID_DEL_POR = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    DS_MOTIVO_EXCLUSAO = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_AMANAJE_REGIAO_MONIT", x => x.ID_REGIAO);
                    table.CheckConstraint("CK_AMANAJE_REGIAO_AREA", "TP_AREA IN ('PONTE','ENCOSTA','AREA_RURAL','COMUNIDADE','PROPRIEDADE_PRIVADA','REGIAO_RIBEIRINHA','AREA_URBANA','OUTRA')");
                    table.CheckConstraint("CK_AMANAJE_REGIAO_ATIVO", "ST_ATIVO IN ('S','N')");
                    table.CheckConstraint("CK_AMANAJE_REGIAO_DEL", "(ST_ATIVO = 'S' AND DT_DEL_EM IS NULL) OR (ST_ATIVO = 'N')");
                    table.CheckConstraint("CK_AMANAJE_REGIAO_ESTADO", "REGEXP_LIKE(SG_ESTADO, '^[A-Z]{2}$')");
                    table.CheckConstraint("CK_AMANAJE_REGIAO_LAT", "NR_LATITUDE BETWEEN -90 AND 90");
                    table.CheckConstraint("CK_AMANAJE_REGIAO_LONG", "NR_LONGITUDE BETWEEN -180 AND 180");
                    table.CheckConstraint("CK_AMANAJE_REGIAO_VISIB", "TP_VISIB IN ('PRIVADA','INSTITUCIONAL','AGREGADA_PUBLICA')");
                    table.CheckConstraint("CK_AMANAJE_REGIAO_VULN", "NR_NIVEL_VULN BETWEEN 0 AND 100");
                    table.ForeignKey(
                        name: "FK_REGIAO_CLI",
                        column: x => x.ID_CLIENTE,
                        principalTable: "TB_AMANAJE_CLI",
                        principalColumn: "ID_CLIENTE",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_REGIAO_DEL_POR",
                        column: x => x.ID_DEL_POR,
                        principalTable: "TB_AMANAJE_USU",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TB_AMANAJE_OBS_CLIM",
                columns: table => new
                {
                    ID_OBSERVACAO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ID_REGIAO = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    NM_FONTE = table.Column<string>(type: "NVARCHAR2(80)", maxLength: 80, nullable: false),
                    NR_TEMPERATURA_C = table.Column<decimal>(type: "DECIMAL(6,2)", precision: 6, scale: 2, nullable: true),
                    NR_UMIDADE_PCT = table.Column<decimal>(type: "DECIMAL(5,2)", precision: 5, scale: 2, nullable: true),
                    NR_PRECIP_MM = table.Column<decimal>(type: "DECIMAL(8,2)", precision: 8, scale: 2, nullable: true),
                    NR_VENTO_KMH = table.Column<decimal>(type: "DECIMAL(8,2)", precision: 8, scale: 2, nullable: true),
                    NR_PRESSAO_HPA = table.Column<decimal>(type: "DECIMAL(8,2)", precision: 8, scale: 2, nullable: true),
                    NR_RADIACAO_SOLAR = table.Column<decimal>(type: "DECIMAL(10,2)", precision: 10, scale: 2, nullable: true),
                    NR_INDICE_UV = table.Column<decimal>(type: "DECIMAL(5,2)", precision: 5, scale: 2, nullable: true),
                    DT_OBS = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    DT_CRIADO_EM = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_AMANAJE_OBS_CLIM", x => x.ID_OBSERVACAO);
                    table.CheckConstraint("CK_AMANAJE_OBS_PREC", "NR_PRECIP_MM IS NULL OR NR_PRECIP_MM >= 0");
                    table.CheckConstraint("CK_AMANAJE_OBS_PRESSAO", "NR_PRESSAO_HPA IS NULL OR NR_PRESSAO_HPA BETWEEN 800 AND 1200");
                    table.CheckConstraint("CK_AMANAJE_OBS_UMIDADE", "NR_UMIDADE_PCT IS NULL OR NR_UMIDADE_PCT BETWEEN 0 AND 100");
                    table.CheckConstraint("CK_AMANAJE_OBS_UV", "NR_INDICE_UV IS NULL OR NR_INDICE_UV BETWEEN 0 AND 20");
                    table.CheckConstraint("CK_AMANAJE_OBS_VENTO", "NR_VENTO_KMH IS NULL OR NR_VENTO_KMH >= 0");
                    table.ForeignKey(
                        name: "FK_OBS_REGIAO",
                        column: x => x.ID_REGIAO,
                        principalTable: "TB_AMANAJE_REGIAO_MONIT",
                        principalColumn: "ID_REGIAO",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TB_AMANAJE_PROCESS",
                columns: table => new
                {
                    ID_PROCESSAMENTO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ID_REGIAO = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    ID_USUARIO = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    TP_PROCESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    ST_PROCESS = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    DS_ORIGEM = table.Column<string>(type: "NVARCHAR2(120)", maxLength: 120, nullable: false),
                    DS_PARAM = table.Column<string>(type: "NVARCHAR2(1000)", maxLength: 1000, nullable: true),
                    DS_RESULT = table.Column<string>(type: "NVARCHAR2(1000)", maxLength: 1000, nullable: true),
                    DT_INICIO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    DT_FIM = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_AMANAJE_PROCESS", x => x.ID_PROCESSAMENTO);
                    table.CheckConstraint("CK_AMANAJE_PROCESS_FIM", "DT_FIM IS NULL OR DT_FIM >= DT_INICIO");
                    table.CheckConstraint("CK_AMANAJE_PROCESS_STATUS", "ST_PROCESS IN ('INICIADO','EM_EXECUCAO','CONCLUIDO','FALHOU','CANCELADO')");
                    table.CheckConstraint("CK_AMANAJE_PROCESS_TIPO", "TP_PROCESS IN ('SINCRONIZACAO_CLIM','CALCULO_RISCO','GERACAO_IND','GERACAO_ALERTA','CARGA_DADOS','ROTINA_PL_SQL','OUTRO')");
                    table.ForeignKey(
                        name: "FK_PROCESS_REGIAO",
                        column: x => x.ID_REGIAO,
                        principalTable: "TB_AMANAJE_REGIAO_MONIT",
                        principalColumn: "ID_REGIAO",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PROCESS_USU",
                        column: x => x.ID_USUARIO,
                        principalTable: "TB_AMANAJE_USU",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_AMANAJE_OBS_CLIM_ID_REGIAO",
                table: "TB_AMANAJE_OBS_CLIM",
                column: "ID_REGIAO");

            migrationBuilder.CreateIndex(
                name: "UQ_AMANAJE_OBS_ID_REGIAO",
                table: "TB_AMANAJE_OBS_CLIM",
                columns: new[] { "ID_OBSERVACAO", "ID_REGIAO" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TB_AMANAJE_PROCESS_ID_REGIAO",
                table: "TB_AMANAJE_PROCESS",
                column: "ID_REGIAO");

            migrationBuilder.CreateIndex(
                name: "IX_TB_AMANAJE_PROCESS_ID_USUARIO",
                table: "TB_AMANAJE_PROCESS",
                column: "ID_USUARIO");

            migrationBuilder.CreateIndex(
                name: "IX_TB_AMANAJE_REGIAO_MONIT_ID_DEL_POR",
                table: "TB_AMANAJE_REGIAO_MONIT",
                column: "ID_DEL_POR");

            migrationBuilder.CreateIndex(
                name: "UQ_AMANAJE_REGIAO_CLI_NOME",
                table: "TB_AMANAJE_REGIAO_MONIT",
                columns: new[] { "ID_CLIENTE", "NM_REGIAO" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_AMANAJE_OBS_CLIM");

            migrationBuilder.DropTable(
                name: "TB_AMANAJE_PROCESS");

            migrationBuilder.DropTable(
                name: "TB_AMANAJE_REGIAO_MONIT");

            migrationBuilder.DropTable(
                name: "TB_AMANAJE_CLI");

            migrationBuilder.DropTable(
                name: "TB_AMANAJE_USU");
        }
    }
}
