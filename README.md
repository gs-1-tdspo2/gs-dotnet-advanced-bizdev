# Amanajé — API Climática .NET

API RESTful desenvolvida em **ASP.NET Core** para o serviço climático da plataforma **Amanajé**, solução proposta para o **Global Solution 2026 FIAP**.

A API é responsável pelo **cadastro de regiões monitoradas**, **sincronização de dados climáticos públicos** via integração com a [OpenMeteo](https://open-meteo.com/) e **rastreamento de processamentos**, servindo como serviço climático do ecossistema Amanajé — composto também por uma API Java responsável pelo core de clientes, alertas e avaliações de risco.

---

## Integrantes

| Nome | RM |
|------|----|
| Victor Sabelli | RM566224 |
| Gustavo Crevelari | RM561408 |
| Lucca Gomes | RM561996 |
| Rafaela Ferreira | RM561671 |

---

## Repositório

[![GitHub](https://img.shields.io/badge/GitHub-Acessar%20Repositório-181717?style=for-the-badge&logo=github&logoColor=white)](https://github.com/seu-usuario/amanaje-dotnet)

---

## Vídeos

| Tipo | Link |
|------|------|
| Demonstração da Solução | [Assistir no YouTube](https://www.youtube.com/watch?v=SEU_LINK_AQUI) |
| Pitch | [Assistir no YouTube](https://www.youtube.com/watch?v=SEU_LINK_PITCH_AQUI) |

---

## Sobre o Amanajé

O **Amanajé** é uma plataforma de monitoramento climático e gestão de risco voltada para comunidades, agricultores e gestores públicos em regiões vulneráveis do Brasil. O nome é inspirado em uma palavra indígena que significa *"mensageiro"*.

O serviço .NET é responsável pelo módulo climático:

- Cadastro e gerenciamento de **regiões monitoradas** (pontes, encostas, comunidades, áreas rurais etc.)
- **Sincronização automática** de dados climáticos públicos via OpenMeteo (temperatura, umidade, precipitação, vento, pressão, radiação solar e índice UV)
- **Rastreamento de processamentos** com ciclo de vida completo (INICIADO → EM_EXECUCAO → CONCLUIDO / FALHOU)

---

## Estrutura do Projeto

```
Amanaje_API/
├── Controllers/         # Endpoints REST
│   ├── RegiaoMonitoradaController.cs
│   ├── ObservacaoClimaticaController.cs
│   ├── ProcessamentoController.cs
│   └── ClimaController.cs
├── Data/                # ApplicationContext (EF Core)
├── DTOs/                # Objetos de transferência de dados (Request / Response)
├── Enums/               # TipoArea, TipoVisibilidade, TipoProcessamento, StatusProcessamento
├── Migrations/          # Migrations do banco Oracle
├── Models/              # Entities mapeadas
│   └── Externals/       # Entities somente leitura (API Java)
├── Services/            # IClimaService + ClimaService (integração OpenMeteo)
└── prints/              # Evidências dos testes por endpoint
```

---

## Modelagem do Banco

> Este repositório gerencia as tabelas: `TB_AMANAJE_REGIAO_MONIT`, `TB_AMANAJE_OBS_CLIM` e `TB_AMANAJE_PROCESS`. As demais tabelas do ecossistema (clientes, usuários, alertas, avaliações de risco, estações IoT) são de responsabilidade da **API Java** da equipe.

As tabelas externas `TB_AMANAJE_CLI` e `TB_AMANAJE_USU` são mapeadas como entidades somente leitura para validação de chaves estrangeiras, sem geração de migrations.

---

## Tecnologias

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core + Oracle Provider
- Swashbuckle (Swagger / OpenAPI)
- Oracle Database (compartilhado com API Java)
- [OpenMeteo API](https://open-meteo.com/)

---

## Instalação e Execução

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Acesso ao banco Oracle configurado pela equipe

### 1. Clone o repositório

```bash
git clone https://github.com/seu-usuario/amanaje-dotnet.git
cd amanaje-dotnet
```

### 2. Configure a string de conexão

As credenciais do banco Oracle estão configuradas em `Amanaje_API/appsettings.json`. Basta clonar e executar — nenhuma alteração é necessária para o ambiente de avaliação.

```json
{
  "ConnectionStrings": {
    "Oracle": "User Id=...;Password=...;Data Source=..."
  }
}
```

### 3. Migrations

As migrations foram geradas com o comando abaixo e estão disponíveis na pasta `Migrations/`:

```bash
dotnet ef migrations add InitialMigration
dotnet ef database update
```

> O banco Oracle foi modelado e criado previamente pela equipe. As migrations existem para fins de versionamento e rastreabilidade do schema.

### 4. Execute a API

```bash
dotnet run
```

A API estará disponível em `https://localhost:7001` (ou a porta configurada em `launchSettings.json`).

### 5. Acesse o Swagger

```
https://localhost:7001/swagger
```

---

## Rotas

### Regiões Monitoradas — `/api/regiao`

| Método | Rota | Descrição | Retorno |
|--------|------|-----------|---------|
| GET | `/api/regiao` | Lista todas as regiões | 200 / 204 |
| GET | `/api/regiao/ativas` | Lista regiões ativas | 200 / 204 |
| GET | `/api/regiao/inativas` | Lista regiões inativas | 200 / 204 |
| GET | `/api/regiao/{id}` | Busca região por ID | 200 / 404 |
| GET | `/api/regiao/cliente/{idCliente}` | Lista regiões ativas por cliente | 200 / 204 |
| POST | `/api/regiao` | Cadastra nova região | 201 / 404 / 409 / 400 |
| PUT | `/api/regiao/{id}` | Atualiza região ativa | 200 / 404 / 409 / 400 |
| PUT | `/api/regiao/reativar/{id}` | Reativa região inativa | 200 / 404 |
| DELETE | `/api/regiao/{id}` | Inativa região (soft delete) | 200 / 404 |

**POST / PUT — Body:**
```json
{
  "idCliente": 1,
  "nmRegiao": "Encosta Norte",
  "nmCidade": "São Paulo",
  "sgEstado": "SP",
  "nrLatitude": -23.55,
  "nrLongitude": -46.63,
  "tpArea": 1,
  "nrNivelVuln": 75,
  "tpVisib": 0
}
```

> `tpArea` aceita: `PONTE (0)`, `ENCOSTA (1)`, `AREA_RURAL (2)`, `COMUNIDADE (3)`, `PROPRIEDADE_PRIVADA (4)`, `REGIAO_RIBEIRINHA (5)`, `AREA_URBANA (6)`, `OUTRA (7)`

> `tpVisib` aceita: `PRIVADA (0)`, `INSTITUCIONAL (1)`, `AGREGADA_PUBLICA (2)`

---

### Observações Climáticas — `/api/observacao`

| Método | Rota | Descrição | Retorno |
|--------|------|-----------|---------|
| GET | `/api/observacao` | Lista todas as observações | 200 / 204 |
| GET | `/api/observacao/{id}` | Busca observação por ID | 200 / 404 |
| GET | `/api/observacao/regiao/{idRegiao}` | Lista observações por região | 200 / 204 |
| GET | `/api/observacao/regiao/{idRegiao}/ultima` | Retorna a última observação da região | 200 / 404 |
| POST | `/api/observacao` | Registra observação manual | 201 / 404 / 400 |
| DELETE | `/api/observacao/{id}` | Remove observação (delete físico) | 200 / 404 |

> Sem `PUT` — observações climáticas são registros imutáveis de telemetria.

**POST — Body:**
```json
{
  "idRegiao": 1,
  "nmFonte": "Manual",
  "nrTemperaturaC": 28.5,
  "nrUmidadePct": 70.0,
  "nrPrecipMm": 0.0,
  "nrVentoKmh": 12.5,
  "nrPressaoHpa": 1013.0,
  "nrRadiacaoSolar": 350.0,
  "nrIndiceUv": 6.0,
  "dtObs": "2026-06-01T12:00:00"
}
```

> Todos os campos climáticos numéricos são opcionais.

---

### Processamentos — `/api/processamento`

| Método | Rota | Descrição | Retorno |
|--------|------|-----------|---------|
| GET | `/api/processamento` | Lista todos os processamentos | 200 / 204 |
| GET | `/api/processamento/{id}` | Busca processamento por ID | 200 / 404 |
| GET | `/api/processamento/regiao/{idRegiao}` | Lista processamentos por região | 200 / 204 |
| GET | `/api/processamento/status/{status}` | Lista processamentos por status | 200 / 204 |
| POST | `/api/processamento` | Registra novo processamento | 201 / 404 / 400 |

> Sem `PUT` ou `DELETE` — o ciclo de vida do processamento é controlado internamente pelo `ClimaService`. Registros de processamento são auditoria e não devem ser alterados ou removidos.

**POST — Body:**
```json
{
  "idRegiao": 1,
  "idUsuario": null,
  "tpProcess": 0,
  "dsOrigem": "Teste manual",
  "dsParam": "lat=-23.55&lon=-46.63"
}
```

> `tpProcess` aceita: `SINCRONIZACAO_CLIM (0)`, `CALCULO_RISCO (1)`, `GERACAO_IND (2)`, `GERACAO_ALERTA (3)`, `CARGA_DADOS (4)`, `ROTINA_PL_SQL (5)`, `OUTRO (6)`

> O status inicial é sempre `INICIADO` — controlado pelo servidor.

---

### Sincronização Climática — `/api/clima`

| Método | Rota | Descrição | Retorno |
|--------|------|-----------|---------|
| POST | `/api/clima/sincronizar/{idRegiao}` | Sincroniza dados climáticos via OpenMeteo | 200 / 404 / 400 |
| GET | `/api/clima/regiao/{idRegiao}/ultima` | Retorna a última observação climática da região | 200 / 404 |
| GET | `/api/clima/fontes` | Lista as fontes climáticas configuradas | 200 |

**Fluxo interno da sincronização:**

```
POST /api/clima/sincronizar/{idRegiao}
  │
  ├── Busca região no banco (lat/lon)
  ├── Cria Processamento com status INICIADO
  ├── Atualiza para EM_EXECUCAO
  ├── Chama OpenMeteo API (lat/lon)
  ├── Normaliza resposta → ObservacaoClimatica
  ├── Persiste ObservacaoClimatica
  ├── Atualiza Processamento para CONCLUIDO
  └── Retorna ObservacaoClimaticaResponseDto
       (em caso de erro → Processamento marcado como FALHOU)
```

**Exemplo de resposta:**
```json
{
  "idObservacao": 1,
  "idRegiao": 1,
  "nmFonte": "OpenMeteo",
  "nrTemperaturaC": 24.3,
  "nrUmidadePct": 68.0,
  "nrPrecipMm": 0.0,
  "nrVentoKmh": 12.5,
  "nrPressaoHpa": 1012.4,
  "nrRadiacaoSolar": 450.2,
  "nrIndiceUv": 6.1,
  "dtObs": "2026-06-01T12:00:00Z",
  "dtCriadoEm": "2026-06-01T12:00:05Z"
}
```

---

## Evidências de Testes

Prints de todos os endpoints testados estão na pasta `prints/`, organizados por controller:

```
prints/
├── RegiaoMonitorada/      (9 endpoints)
├── ObservacaoClimatica/   (6 endpoints)
├── Processamento/         (5 endpoints)
└── Clima/                 (3 endpoints)
```

| Controller | Endpoints | Evidência |
| :--- | :---: | :--- |
| **RegiaoMonitorada** | 9 endpoints | [Visualizar Prints](prints/RegiaoMonitorada/) |
| **ObservacaoClimatica** | 6 endpoints | [Visualizar Prints](prints/ObservacaoClimatica/) |
| **Processamento** | 5 endpoints | [Visualizar Prints](prints/Processamento/) |
| **Clima** | 3 endpoints | [Visualizar Prints](prints/Clima/) |

> **Total:** 23 endpoints testados e documentados.

---

## Observações

- O banco Oracle é compartilhado com a **API Java** da equipe. As tabelas de `Cliente` e `Usuário` são gerenciadas pela API Java — a API .NET realiza apenas leitura dessas tabelas para validação de FKs.
- Exclusões em `RegiaoMonitorada` são **lógicas** via `ST_ATIVO`, preservando a integridade referencial com observações e processamentos vinculados.
- Exclusões em `ObservacaoClimatica` são **físicas**, pois a remoção de um dado de telemetria específico não compromete o histórico geral.
- `Processamento` não possui exclusão — é registro de auditoria imutável.
- A integração com a **OpenMeteo** é gratuita e não requer chave de API.