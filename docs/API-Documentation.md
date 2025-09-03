# Documentação da API FINEP

## Visão Geral

Esta API foi desenvolvida para gerenciar dispositivos de medição de água, fornecendo funcionalidades de autenticação, dashboard e consulta de dados dos equipamentos.

## Tecnologias Utilizadas

- **Framework**: ASP.NET Core 9.0
- **Banco de Dados**: PostgreSQL com Entity Framework Core
- **Autenticação**: JWT (JSON Web Tokens)
- **Documentação**: Swagger/OpenAPI

## Autenticação

Todos os endpoints (exceto login e registro) requerem autenticação via JWT Token.

### Como usar:
1. Faça login ou registre-se para obter um token
2. Inclua o token no cabeçalho das requisições: `Authorization: Bearer {seu-token}`

---

## Endpoints de Autenticação

### 1. Login
**POST** `/api/auth/login`

**Descrição**: Autentica um usuário e retorna um token JWT.

**Corpo da Requisição**:
```json
{
  "username": "string",
  "password": "string"
}
```

**Resposta de Sucesso (200)**:
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "id": 1,
    "username": "usuario",
    "email": "usuario@email.com",
    "fullName": "Nome Completo",
    "role": "User"
  }
}
```

### 2. Registro
**POST** `/api/auth/register`

**Descrição**: Registra um novo usuário no sistema.

**Corpo da Requisição**:
```json
{
  "username": "string",
  "email": "string",
  "fullName": "string",
  "password": "string"
}
```

**Resposta de Sucesso (201)**:
```json
{
  "message": "Usuário registrado com sucesso",
  "userId": 1
}
```

### 3. Perfil do Usuário
**GET** `/api/auth/profile`

**Descrição**: Retorna informações do usuário autenticado.

**Cabeçalhos Obrigatórios**:
- `Authorization: Bearer {token}`

**Resposta de Sucesso (200)**:
```json
{
  "id": 1,
  "username": "usuario",
  "email": "usuario@email.com",
  "fullName": "Nome Completo",
  "role": "User"
}
```

### 4. Validar Token
**GET** `/api/auth/validate-token`

**Descrição**: Valida se o token JWT ainda é válido.

**Cabeçalhos Obrigatórios**:
- `Authorization: Bearer {token}`

**Resposta de Sucesso (200)**:
```json
{
  "valid": true,
  "message": "Token válido"
}
```

---

## Endpoints do Dashboard

### 1. Resumo do Dashboard
**GET** `/api/dashboard/summary`

**Descrição**: Retorna estatísticas gerais do sistema.

**Parâmetros de Consulta (Opcionais)**:
- `description` (string): Filtrar por descrição do dispositivo
- `lastReadingInicio` (datetime): Data inicial para filtro
- `lastReadingFim` (datetime): Data final para filtro

**Exemplo de Requisição**:
```
GET /api/dashboard/summary?description=sensor&lastReadingInicio=2024-01-01&lastReadingFim=2024-01-31
```

**Resposta de Sucesso (200)**:
```json
{
  "totalRegistros": 1500,
  "totalDispositivos": 25,
  "ultimoUpdate": "2024-01-15T10:30:00Z"
}
```

### 2. Estatísticas dos Dispositivos
**GET** `/api/dashboard/device-statistics`

**Descrição**: Retorna estatísticas detalhadas dos dispositivos.

**Parâmetros de Consulta (Opcionais)**:
- `description` (string): Filtrar por descrição
- `lastReadingInicio` (datetime): Data inicial
- `lastReadingFim` (datetime): Data final

**Resposta de Sucesso (200)**:
```json
[
  {
    "deviceId": 1,
    "serial": "DEV001",
    "description": "Sensor de Fluxo Principal",
    "timestamp": "2024-01-15T10:30:00Z",
    "flow": 125.50,
    "volume": 1000.75
  }
]
```

### 3. Leituras das Últimas 24 Horas
**GET** `/api/dashboard/readings-24h`

**Descrição**: Retorna leituras dos dispositivos nas últimas 24 horas.

**Parâmetros de Consulta (Opcionais)**:
- `description` (string): Filtrar por descrição
- `lastReadingInicio` (datetime): Data inicial
- `lastReadingFim` (datetime): Data final

**Resposta de Sucesso (200)**:
```json
[
  {
    "deviceId": 1,
    "serial": "DEV001",
    "description": "Sensor Principal",
    "timestamp": "2024-01-15T10:30:00Z",
    "flow": 125.50,
    "volume": 1000.75
  }
]
```

---

## Endpoints de Dispositivos

### 1. Obter Dispositivo por ID
**GET** `/api/devices/{id}`

**Descrição**: Retorna informações completas de um dispositivo específico.

**Parâmetros de Rota**:
- `id` (int): ID do dispositivo (obrigatório, deve ser > 0)

**Exemplo de Requisição**:
```
GET /api/devices/1
```

**Resposta de Sucesso (200)**:
```json
{
  "id": 1,
  "serial": "DEV001",
  "description": "Sensor de Fluxo Principal",
  "status": "Ativo",
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-15T10:30:00Z",
  "minFlow": 0.0,
  "maxFlow": 500.0,
  "logradouro": "Rua das Flores, 123",
  "cidade": "São Paulo",
  "estado": "SP",
  "latitude": -23.5505,
  "longitude": -46.6333,
  "minVolume": 0.0,
  "maxVolume": 10000.0,
  "cep": "01234-567",
  "bairro": "Centro",
  "numero": "123"
}
```

**Resposta de Erro (404)**:
```json
{
  "message": "Dispositivo com ID 999 não encontrado"
}
```

### 2. Obter Peaks dos Dispositivos
**GET** `/api/devices/peaks`

**Descrição**: Retorna os picos de medição dos dispositivos.

**Parâmetros de Consulta (Todos Obrigatórios)**:
- `serial` (string): Serial do dispositivo
- `factory` (int): Factory (deve ser 0 ou 1)
- `type` (string): Tipo (deve ser "DNS" ou "UPS")

**Exemplo de Requisição**:
```
GET /api/devices/peaks?serial=DEV001&factory=1&type=DNS
```

**Resposta de Sucesso (200)**:
```json
[
  {
    "sequence": 1,
    "value": 125.50
  },
  {
    "sequence": 2,
    "value": 130.25
  }
]
```

**Resposta de Erro (400)**:
```json
{
  "message": "Serial parameter is required"
}
```

---

## Códigos de Status HTTP

| Código | Descrição |
|--------|----------|
| 200 | Sucesso - Requisição processada com sucesso |
| 201 | Criado - Recurso criado com sucesso |
| 400 | Requisição Inválida - Parâmetros incorretos ou ausentes |
| 401 | Não Autorizado - Token inválido ou ausente |
| 404 | Não Encontrado - Recurso não existe |
| 500 | Erro Interno - Erro no servidor |

---

## Modelos de Dados

### Usuário (User)
```json
{
  "id": "int",
  "username": "string (máx. 50 caracteres)",
  "email": "string (máx. 100 caracteres)",
  "fullName": "string (máx. 100 caracteres)",
  "passwordHash": "string",
  "createdAt": "datetime",
  "isActive": "boolean",
  "role": "string (máx. 20 caracteres)"
}
```

### Dispositivo (Device)
```json
{
  "id": "int",
  "serial": "string",
  "description": "string (opcional)",
  "latitude": "decimal (opcional)",
  "longitude": "decimal (opcional)",
  "minFlow": "decimal (opcional)",
  "maxFlow": "decimal (opcional)",
  "minVolume": "decimal (opcional)",
  "maxVolume": "decimal (opcional)",
  "cep": "string (opcional)",
  "logradouro": "string (opcional)",
  "bairro": "string (opcional)",
  "cidade": "string (opcional)",
  "estado": "string (opcional)",
  "numero": "string (opcional)",
  "status": "string",
  "createdAt": "datetime",
  "updatedAt": "datetime (opcional)"
}
```

### Peak
```json
{
  "id": "int",
  "peaksDevicesId": "int",
  "sequence": "int",
  "type": "string",
  "value": "decimal",
  "address": "int",
  "factory": "int"
}
```

---

## Exemplos de Uso

### 1. Fluxo Completo de Autenticação
```bash
# 1. Registrar usuário
curl -X POST "http://localhost:5000/api/auth/register" \
  -H "Content-Type: application/json" \
  -d '{
    "username": "joao",
    "email": "joao@email.com",
    "fullName": "João Silva",
    "password": "senha123"
  }'

# 2. Fazer login
curl -X POST "http://localhost:5000/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "username": "joao",
    "password": "senha123"
  }'

# 3. Usar o token retornado nas próximas requisições
curl -X GET "http://localhost:5000/api/dashboard/summary" \
  -H "Authorization: Bearer SEU_TOKEN_AQUI"
```

### 2. Consultar Dispositivo Específico
```bash
curl -X GET "http://localhost:5000/api/devices/1" \
  -H "Authorization: Bearer SEU_TOKEN_AQUI"
```

### 3. Obter Peaks com Filtros
```bash
curl -X GET "http://localhost:5000/api/devices/peaks?serial=DEV001&factory=1&type=DNS" \
  -H "Authorization: Bearer SEU_TOKEN_AQUI"
```

---

## Configuração e Execução

### Pré-requisitos
- .NET 9.0 SDK
- PostgreSQL
- Visual Studio ou VS Code (opcional)

### Como Executar
1. Clone o repositório
2. Configure a string de conexão no `appsettings.json`
3. Execute as migrações: `dotnet ef database update`
4. Inicie a aplicação: `dotnet run`
5. Acesse a documentação Swagger em: `http://localhost:5000/swagger`

---

## Observações Importantes

- Todos os endpoints (exceto `/auth/login` e `/auth/register`) requerem autenticação
- Os tokens JWT têm tempo de expiração configurado
- Parâmetros de data devem estar no formato ISO 8601
- A API utiliza CORS configurado para desenvolvimento
- Logs são gerados automaticamente para todas as requisições

---

## Suporte

Para dúvidas ou problemas, consulte os logs da aplicação ou entre em contato com a equipe de desenvolvimento.