# Task Manager

Sistema de gerenciamento de tarefas com backend em ASP.NET Core Web API e frontend em React, usando SQL Server, Entity Framework Core e uma organização inspirada em Clean Architecture.

## 📌 Overview

O projeto permite:

- listar tarefas
- criar tarefas
- editar tarefas
- excluir tarefas
- filtrar tarefas por status
- alterar o status diretamente pela listagem
- visualizar detalhes de uma tarefa em um dialog

## 🛠️ Tech Stack

### ⚙️ Backend

- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- xUnit
- Moq
- FluentAssertions

### 🎨 Frontend

- React
- TypeScript
- Vite
- PrimeReact
- Axios

### 🐳 Infraestrutura

- Docker Compose
- SQL Server 2022

## 🧱 Arquitetura

O backend está organizado em camadas separadas para responsabilidades distintas:

- `TaskManager.Api`: expõe os endpoints HTTP, configuração da aplicação, middleware e mapeamentos de request/response
- `TaskManager.Application`: contém serviços, DTOs e contratos de aplicação
- `TaskManager.Domain`: contém entidades, enums e regras de negócio
- `TaskManager.Infrastructure`: persistência com Entity Framework Core, contexto e repositórios
- `TaskManager.Application.Tests`: testes unitários da camada de aplicação
- `TaskManager.Domain.Tests`: testes unitários da camada de domínio

O frontend consome a API e concentra a interface de gestão de tarefas em uma única página, com componentes separados para tabela, formulário, filtros e confirmação de exclusão.

## 🗂️ Estrutura do Repositório

```text
task-manager/
	backend/
		TaskManager.Api/
		TaskManager.Application/
		TaskManager.Domain/
		TaskManager.Infrastructure/
		TaskManager.Application.Tests/
		TaskManager.Domain.Tests/
		TaskManager.sln
	frontend/
		src/
			api/
			components/
			types/
			utils/
			App.tsx
			main.tsx
		public/
		package.json
		vite.config.ts
	docker-compose.yml
```

### 🎯 Estrutura do Frontend

- `src/api`: comunicação HTTP com a API
- `src/components`: componentes da interface, como tabela, dialogs e filtros
- `src/types`: tipagens e enums usados no frontend
- `src/utils`: utilitários compartilhados, como tratamento de datas
- `src/App.tsx`: composição principal da tela
- `src/main.tsx`: bootstrap da aplicação React
- `public`: arquivos estáticos públicos

## ✅ Requisitos

Antes de rodar o projeto, tenha instalado:

- .NET SDK 9
- Node.js 20+ com npm
- Docker Desktop

## 🐘 Configuração do Banco com Docker

O banco SQL Server é provisionado pelo arquivo [docker-compose.yml](c:\Users\julio\Documents\Development\data System\task-manager\docker-compose.yml).

### 🚀 Subir o container do banco

Na raiz do projeto, execute:

```powershell
docker compose up -d
```

Isso iniciará um container SQL Server com:

- host: `localhost`
- porta: `1433`
- usuário: `sa`
- senha: `TaskManager@2026`
- banco usado pela aplicação: `TaskManagerDb`

## 🔧 Como Rodar o Backend

### 1. 📦 Restaurar dependências

```powershell
cd backend
dotnet restore TaskManager.sln
```

### 2. 🗃️ Aplicar as migrations no banco

```powershell
dotnet ef database update --project TaskManager.Infrastructure --startup-project TaskManager.Api
```

Esse comando cria o banco `TaskManagerDb` e aplica o schema inicial.

### 3. ▶️ Iniciar a API

```powershell
cd TaskManager.Api
dotnet run
```

Por padrão, a API roda em:

- `http://localhost:5030`
- `https://localhost:7297`

Swagger disponível em desenvolvimento:

- `http://localhost:5030/swagger`

## 💻 Como Rodar o Frontend

Em outro terminal:

```powershell
cd frontend
npm install
npm run dev
```

O frontend roda por padrão em:

- `http://localhost:5173`

## 🧭 Ordem Recomendada para Subir Tudo

### 📍 Passo a passo completo

1. Suba o SQL Server com Docker

```powershell
docker compose up -d
```

2. Aplique as migrations do backend

```powershell
cd backend
dotnet ef database update --project TaskManager.Infrastructure --startup-project TaskManager.Api
```

3. Inicie a API

```powershell
cd TaskManager.Api
dotnet run
```

4. Em outro terminal, inicie o frontend

```powershell
cd frontend
npm install
npm run dev
```

5. Acesse no navegador

- Frontend: `http://localhost:5173`
- Swagger: `http://localhost:5030/swagger`

## 🧪 Testes

### 🔹 Testes da camada de aplicação

```powershell
cd backend
dotnet test TaskManager.Application.Tests\TaskManager.Application.Tests.csproj
```

### 🔹 Testes da camada de domínio

```powershell
cd backend
dotnet test TaskManager.Domain.Tests\TaskManager.Domain.Tests.csproj
```

## 📋 Regras de Negócio Importantes

- toda tarefa nova é criada com status `Pending`
- o status pode ser alterado depois via edição ou diretamente pela listagem
- o prazo final não pode ser anterior ao momento atual
- o título não pode ultrapassar 100 caracteres

## 👀 Observações

- a API está configurada com CORS liberado em ambiente de desenvolvimento
- a connection string do backend já está configurada para o SQL Server do Docker com a senha `TaskManager@2026`
- as datas da API são tratadas para sair em UTC explícito no backend
- para simplificar a execução local deste projeto de teste, a configuração do banco foi mantida de forma explícita; em um ambiente profissional, credenciais e segredos devem ser externalizados

## 🧰 Comandos Úteis

### ⏹️ Parar o banco Docker

```powershell
docker compose down
```

### 🧹 Parar o banco e remover o volume de dados

```powershell
docker compose down -v
```

### 🏗️ Gerar build do frontend

```powershell
cd frontend
npm run build
```

### 🏗️ Compilar a solução backend

```powershell
cd backend
dotnet build TaskManager.sln
```
