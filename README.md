# Tarefando

# Sobre o projeto
Todas as etapas foram cumpridas e testadas. O projeto está publicado no link abaixo:
Link projeto render: 

📌 Perguntas para o PO — Refinamento Fase 2
📊 Relatórios e Métricas
Quais outros tipos de relatórios além do número médio de tarefas concluídas por usuário nos últimos 30 dias seriam relevantes para os gestores?
Os relatórios precisam ser exportáveis (ex.: CSV, PDF) ou apenas consumíveis via API?
Deve haver filtros personalizáveis (por período, por projeto, por prioridade)?


📌 Funcionalidades de Projeto e Tarefa
Deseja permitir a ordenação ou categorização dos projetos (por data de criação, prioridade geral, etc.)?
Vai existir um conceito de arquivamento ou apenas exclusão de projetos e tarefas?
As tarefas podem ser delegadas de um usuário para outro?
Qual deve ser o comportamento se uma tarefa for criada sem data de vencimento?
Será possível reabrir uma tarefa já concluída?


📌 Comentários e Histórico
Os comentários das tarefas poderão ser editados ou removidos?
Deve existir um histórico de exclusão de comentários ou apenas de alterações na tarefa?
Os comentários podem conter anexos (imagens, arquivos)?


📌 Limitações e Restrições
O limite de 20 tarefas por projeto pode variar por usuário ou tipo de projeto no futuro?
Deve haver notificação (e-mail, push ou API callback) para tarefas próximas do vencimento ou com status alterado?


📌 Integrações e Autenticação
No futuro, pretende-se integrar com sistemas externos de autenticação ou calendário (Google Calendar, Outlook, etc.)?
A API deve suportar roles adicionais além de “usuário” e “gerente”?


📌 Experiência e Qualidade
Existe alguma exigência de performance ou SLA (tempo de resposta máximo) para os endpoints?
Deve haver paginação e ordenação na listagem de projetos e tarefas?


📌 Segurança e Controle
A API deve registrar logs de acesso e operações sensíveis para auditoria?
Qual a política para retenção e limpeza de histórico de alterações e tarefas antigas?

📌 Etapa 3 — Possíveis Melhorias e Evoluções
🔧 Melhorias Técnicas e de Código
Separação mais rígida de camadas e responsabilidades, apesar de ter sido feito um pouco, mas devido ao tempo não foi 100% mas deve seguir organizando em Application, Domain, Infrastructure e API seguindo Clean Architecture.
Aplicação de validações com FluentValidation ou Notification Pattern, evitando validações diretamente nas entidades.
Substituir primitivas por Value Objects para dados como nome de projeto, descrição de tarefa e prioridade.
Ampliação da cobertura de testes para services e controllers.

☁️ Visão de Arquitetura e Cloud
Configuração de ambiente com Docker Compose, permitindo subir API e banco de dados simultaneamente.
Deploy automatizado via CI/CD pipelines (GitHub Actions, GitLab CI ou Azure DevOps).
Implementação de logging estruturado (Serilog) e observabilidade (Prometheus, Grafana ou CloudWatch).
Controle de autenticação e autorização via API Gateway validando JWT tokens.



# 🐳 Executando o Projeto com Docker

Este guia mostra como executar o projeto Tarefando usando Docker.

## 📋 Pré-requisitos

- **Docker Desktop ou Podman** instalado e rodando
  - [Download Docker Desktop](https://www.docker.com/products/docker-desktop/)
- **Git** (para clonar o repositório)

## 🚀 Iniciando o Projeto

### 1. Clone o Repositório
```bash
git clone <url-do-repositorio>
cd Tarefando
```

### 2. Verifique se o Docker está Rodando
```bash
# Verificar se o Docker está funcionando
docker --version
docker ps
```

**💡 Dica:** Se aparecer erro de conexão, certifique-se que o Docker Desktop está aberto e rodando.

### 3. Construir a Imagem Docker
```bash
# Construir a imagem do projeto
docker build -t tarefando-image .
```

### 4. Executar o Container
```bash
# Executar o container em background
docker run -d -p 8080:8080 -p 8081:8081 --name tarefando-app tarefando-image
```

### 5. Acessar a Aplicação
- **HTTP:** http://localhost:8080/swagger/index.html




## 🐛 Solução de Problemas

### Docker Desktop não está rodando
```bash
# Se aparecer este erro:
# "open //./pipe/dockerDesktopLinuxEngine: The system cannot find the file specified"

# Solução:
# 1. Abrir Docker Desktop
# 2. Aguardar inicialização completa
# 3. Verificar ícone na bandeja do sistema
```

### Porta já está em uso
```bash
# Alterar portas no comando docker run:
docker run -d -p 8082:8080 -p 8083:8081 --name tarefando-app tarefando-image
```

### Container com mesmo nome já existe
```bash
# Remover container existente
docker rm tarefando-app

# Ou usar nome diferente
docker run -d -p 8080:8080 -p 8081:8081 --name tarefando-app-v2 tarefando-image
```

### Verificar se o Build Funcionou
```bash
# Listar imagens disponíveis
docker images

# Testar se a imagem foi criada corretamente
docker run --rm tarefando-image dotnet --version
```

### Limpar Imagens Antigas
```bash
# Remover imagens não utilizadas
docker image prune

# Remover imagem específica
docker rmi tarefando-image

# Limpeza completa (CUIDADO!)
docker system prune -a
```

## 📂 Estrutura do Projeto

```
Tarefando/
├── Application/         # Camada de aplicação
├── Domain/             # Camada de domínio
├── Infrastructure/     # Camada de infraestrutura
├── Tarefando/          # Projeto principal (API)
├── Dockerfile          # Configuração Docker
└── Tests               # Arquivo de testes
└── README.md           # Este arquivo
```

## 🔗 Links Úteis

- [Documentação Docker](https://docs.docker.com/)
- [Docker Run Reference](https://docs.docker.com/engine/reference/run/)
- [.NET in Docker](https://docs.microsoft.com/en-us/dotnet/core/docker/)

---

**✅ Pronto!** Sua aplicação está rodando em um container Docker.

**Comandos essenciais:**
- **Construir:** `docker build -t tarefando-image .`
- **Executar:** `docker run -d -p 8080:8080 -p 8081:8081 --name tarefando-app tarefando-image`
- **Ver logs:** `docker logs -f tarefando-app`
- **Parar:** `docker stop tarefando-app`
