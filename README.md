# RealTimeChat

[MicrosoftSQLServer]: https://img.shields.io/badge/Microsoft%20SQL%20Server-CC2927?style=for-the-badge&logo=microsoft%20sql%20server&logoColor=white
[.Net]: https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white
[C#]: https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=csharp&logoColor=white
[React]: https://img.shields.io/badge/React-%2320232a.svg?style=for-the-badge&logo=react&logoColor=%2361DAFB
[SignalR]: https://img.shields.io/badge/SignalR-%23006B3F.svg?style=for-the-badge&logo=aspnet&logoColor=white

![.Net]
![React]
![SignalR]
![C#]
![MicrosoftSQLServer]


<p align="center">
   <a href="#principais-funcionalidades-da-api">🚀 Principais funcionalidades da API</a> 
   <a href="#tecnologias">👨‍💻 Tecnologias</a>
   <a href="#arquitetura">🏛️ Arquitetura</a>
   <a href="#como-executar">🏃 Como executar</a>
</p>

RealTimeChat é uma aplicação de chat em tempo real com suporte a autenticação de usuários, gerenciamento de salas de chat e envio de mensagens via SignalR. O projeto utiliza uma arquitetura CQRS para separar as responsabilidades e facilitar a manutenção.

<h2 id="principais-funcionalidades-da-api">Principais Funcionalidades da API</h2> 

Você pode importar uma coleção do postman que contém todos os endpoints já mapeados baixando o json [AQUI](https://github.com/EduardoKroetz/RealTimeChat/blob/main/RealTimeChat.postman_collection.json)

### 1. Autenticação de Usuário
- **Registro de Usuário**  
  `POST /auth/register`  
  Parâmetros: `username` (string), `password` (string), `email` (string)  
  Retorno: `token` (string), `id` (do usuário criado)

- **Login de Usuário**  
  `POST /auth/login`  
  Parâmetros: `email` (string), `password` (string)  
  Retorno: `token` (string)

### 2. Gerenciamento de Salas de Chat
- **Criação de Salas de Chat**  
  `POST /chatrooms?name=nomeDaSala`

- **Deleção de Salas de Chat**  
  `DELETE /chatrooms/{ID da sala}`

- **Atualização de Salas de Chat**  
  `PUT /chatrooms?name=novoNomeDaSala`

- **Listagem de Salas de Chat**  
  `GET /chatrooms?pageSize={Ex:25}&pageNumber={Ex:1}`

- **Buscar a Sala de Chat pelo Id**  
  `GET /chatrooms/{ID da sala}`

- **Entrar em Salas de Chat**  
  `POST /chatrooms/join/{ID da sala}`

- **Sair de Salas de Chat**  
  `DELETE /chatrooms/leave/{ID da sala}`

- **Buscar as Salas de Chat do usuário**  
  `GET /chatrooms/users`

- **Buscar salas de chat pelo nome**  
  `GET /chatrooms/search?name=nomeDaSala`

### 3. Mensagens em Tempo Real - Pelo Hub do SignalR

Obs: O projeto `.Client` pode ser usado para testar o hub via console.

- **Entrar em uma Sala** | `JoinGroupAsync`  
  Parâmetros: Id da sala

- **Sair de uma Sala** | `LeaveGroupAsync`  
  Parâmetros: Id da sala

- **Envio de Mensagens** | `SendMessageAsync`  
  Parâmetros: Id da sala, Id do usuário, `mensagem` (string)  
  Evento: `ReceiveMessage`: `mensagem` (objeto)

- **Deletar Mensagem** | `DeleteMessageAsync`  
  Parâmetros: Id da mensagem  
  Evento: `DeleteMessage`: Id da mensagem

- **Atualizar Mensagem** | `UpdateMessageAsync`  
  Parâmetros: Id da mensagem, `nova mensagem` (string)  
  Evento: `UpdateMessage`: `nova mensagem` (string)

### 4. Usuários
- **Buscar usuário logado**  
  `GET /users`

<h2 id="tecnologias">Tecnologias</h2> 

### Backend
- **.NET Core / ASP.NET Core**
- **Entity Framework Core** para acesso ao banco de dados
- **SignalR** para comunicação em tempo real
- **JWT (JSON Web Tokens)** para autenticação

### Frontend
- **React.js**
- **Biblioteca SignalR para React**
- **React Router Dom**

### Banco de Dados
- **SQL Server**

<h2 id="arquitetura">Arquitetura - CQRS</h2> 

- **Camada de Apresentação: .Site** (Frontend com React)
  - Componentes React para UI
  - Comunicação com a API backend via HTTP e SignalR

- **Camada de Apresentação do Backend: .API** (API Backend)
  - Controladores para gerenciar requisições HTTP
  - Hubs do SignalR para comunicação em tempo real

- **Camada de Persistência: .Infrastructure** (Banco de Dados)
  - Configuração do contexto do banco de dados
  - Implementação dos repositórios para acesso a dados

- **Camada da Aplicação: .Application** (Lógica de processamento)
  - Commands para atualização de dados
  - Queries para consultas

- **Camada .Core** (Núcleo da aplicação)
  - Entidades do Banco de Dados
  - DTOs para Transferência de Dados
  - Interfaces dos Repositórios
  - Interfaces dos Serviços
 
- **Camada .Client** (Cliente console)
  - Cliente console criado para testar a comunicação com o Hub
 
- **Camada .UnitTests** (Testes unitários)
- **Camada .IntegrationsTests** (Testes de integração)

<h2 id="como-executar">Como Executar</h2> 

### 1. Configuração do Banco de Dados
1. **Instale o SQL Server** e configure uma instância local. Certifique-se de que o banco de dados `RealTimeChat` está disponível.

2. **Configure a String de Conexão e o a chave JWT**:
   No arquivo `appsettings.json` da pasta `.API`, verifique a configuração da string de conexão:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost,1433;Database=RealTimeChat;User ID=sa;Password=sua_senha;TrustServerCertificate=true"
   },
   "JwtKey":"kfgkopwpocnAICklvgRItruYTAJnkArXaqOQbRYUeyAXJkaXaNcVghfAydq",
   "DefaultFrontendBaseUrl": "http://localhost:5173"
   ```
### Configuração do Backend

1. **Navegue até a pasta `.API`:**
    ```bash
    cd .API
    ```

2. **Restaurar pacotes e construir o projeto:**
    ```bash
    dotnet restore
    dotnet build
    ```

3. **Aplicar as Migrações do Entity Framework Core:**
    ```bash
    dotnet ef database update
    ```

4. **Executar o Backend:**
    ```bash
    dotnet run
    ```

    A API estará disponível em `https://localhost:7136` ou em `http://localhost:5000` (ou outro endereço configurado em Properties/lauchSettings.json).

### Configuração do Frontend

1. **Navegue até a pasta `.Site`:**
    ```bash
    cd .Site
    ```

2. **Instale as dependências do React:**
    ```bash
    npm install
    ```

3. **Configure o Frontend:**
    No arquivo `src/api/axiosConfig.ts`, ajuste a URL da API para apontar para o endereço do backend:
    ```javascript
    export const baseUrl = "http://localhost:5000"; //ou outra URL onde a API está executando 
    ```

4. **Executar o Frontend:**
    ```bash
    npm start
    ```

    O frontend estará disponível em `http://localhost:5173` (ou outro endereço configurado).

### Testando o Hub do SignalR pelo console

1. **Navegue até a pasta `.Client`:**
    ```bash
    cd .Client
    ```

2. **Compile e execute o cliente de teste:**
    ```bash
    dotnet run
    ```

    Utilize o cliente para se conectar ao hub do SignalR e testar as funcionalidades de chat em tempo real.

