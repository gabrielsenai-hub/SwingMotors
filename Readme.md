# Swing Motors 

  

## Descrição

  

Na **Swing Motors**, não vendemos apenas carros, entregamos uma experiência de propriedade incomparável. Nosso compromisso é com a qualidade irrepreensível, a transparência total e o atendimento personalizado que você, como cliente de alta classe, merece.

  

Prezamos pela excelência em cada detalhe, desde a curadoria dos veículos mais exclusivos do mercado até o suporte pós‑venda que garante sua tranquilidade. Sua satisfação e confiança são o nosso maior ativo.

  

---

  

## Demo / Acesso

A aplicação está disponível no endereço mascarado (utilize o QR abaixo para acessar):

  

**Link:**  
> [SwingMotors.com](https://x60m3cnz-5139.brs.devtunnels.ms)

  

**QR code:** 

  

```markdown

![QR Code](./wwwroot/IMG/qrSwingMotors.png)

```

  

---

  

## Tecnologias usadas

  

-  **Backend:** ASP.NET Core (Razor Pages / MVC) com C#

-  **ORM:** Entity Framework Core

-  **Banco de dados:** MySQL (criação via migration ou dump MySQL 8.x)

-  **Front-end:** Razor Views + Bootstrap 5

-  **Hospedagem / tunelamento:** Dev tunnels (link fornecido)

-  **Padrões/Arquitetura:** Repository Pattern (repositórios para _Carro_ e _CarroImagem_)

  

> **Versões recomendadas (ajuste conforme seu ambiente):**

> - .NET SDK: **8.0**

> - MySQL Server: **8.0**

> - EF Core: compatível com a versão do .NET utilizada

> - Bootstrap: **5.3.x**


  

---

  

## Estrutura importante do projeto

  

-  `Controllers/` — controllers (ex: `CarroController`, `AdministracaoController`)

-  `Models/` — entidades e ViewModels (`Carro`, `CarroImagem`, `RegistroCarroViewModel`, `CarroVm`, `CarroDetalheVm` etc.)

-  `Views/` — Razor views (Coleção, Detalhes, Criar, Editar, Administração)

-  `Data/` — `DbContext` e configurações do EF

-  `Repository/` — repositórios que encapsulam acesso ao _DbContext_

-  `wwwroot/` — arquivos estáticos (css, js, assets, QR code)

  

---

  

## Funcionalidades principais

  

- CRUD completo de veículos (criar, listar, editar, excluir)

- Upload de imagens (as imagens são armazenadas no banco como `byte[]`)

- Exibição de galeria/carrossel por veículo

- Endpoint para servir imagem: `/Carro/Foto/{id}`

- Página de coleção com paginação/filtragem no front-end

- Área administrativa com permissões (checar `User.IsInRole("Admin")`)

  

---

  

## Requisitos pré‑execução

  

- .NET SDK (8.0 recomendado) instalado

- MySQL Server (8.0) instalado

- Rider IDEA / VS Code (com C# extension)

- Ferramenta para restaurar o dump SQL, caso não queira usar migration (MySQL Workbench)

  

---

  

## Restaurando o banco de dados (dump SQL ou migration)

  

Você pode restaurar o banco usando o arquivo SQL fornecido (dump). Link para download do dump:

  

> [Baixe aqui!](https://drive.google.com/file/d/1QZDP0OdrB7T9vnHRZW78cT09-Fu2OAt7/view?usp=drive_link)

  
### Via Migration
Apenas rode: 
```bash
dotnet watch
```
O banco será criado automaticamente.


  

### Via MySQL Workbench

1. Crie o banco de dados desejado (por exemplo `swingmotors`).

2. Abra *Server > Data Import* e selecione o arquivo SQL.

3. Importe para o schema criado.

  

> Se o dump contiver `CREATE DATABASE`, você pode apenas executar o script em um client (se tiver permissões).

  

---

  

## Configurar _connection string_

  

Edite o `appsettings.json` (ou `appsettings.Development.json`) para apontar para seu banco:

  

```json

"ConnectionStrings": {

"DefaultConnection": "Server=localhost;Database=swingmotors;User=root;Password=SuaSenha;"

}
```
---

## Como executar localmente

  

1. Restaurar/Importar o dump SQL no MySQL.

2. Atualizar `appsettings.json` com sua connection string.

3. Abrir o projeto em VS Code/Visual Studio.

4. Restaurar pacotes e build:

  

```bash

dotnet  restore

dotnet  build

```

  

5. Executar a aplicação:

  

```bash
dotnet  watch 
```
---

  

## Observações e dicas úteis

  

-  **Enctype no forms:** Sempre use `enctype="multipart/form-data"` para formulários que enviam arquivos.

-  **ModelState:** quando ModelState acusar erro em campos do upload, verifique `name` do input e se o ViewModel possui `List<IFormFile> ArquivosImagens`.

-  **Imagens no banco:** como as imagens ficam em `byte[]`, o endpoint `/Carro/Foto/{id}` retorna `File(bytes, contentType)`.

-  **Delete:** ao deletar um carro, remova também todas as imagens relacionadas (`RemoveRange`).


  

---

  

## Rotas úteis

  

-  `GET /Carro/Colecao` — Galeria

-  `GET /Carro/DetalhesVeiculo/{id}` — Detalhes do veículo

-  `GET /Carro/Foto/{id}` — Retorna imagem de um veículo

-  `POST /Carro/Criar` — cria novo veículo (form com arquivos)

-  `POST /Administracao/DeletarCarro` — Deleta um veículo (recebe `id`)

  

---

  

## Equipe

  

-  **Gabriel Oliveira Silva** — Backend & Banco de Dados

-  **Leandro Rivas** — Front-end & Idealização

-  **Arthur Michelangelo** — Backend & Alimentação do banco