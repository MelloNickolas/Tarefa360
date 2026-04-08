# 🚀 Tarefa360

Sistema de gerenciamento de tarefas baseado na metodologia **Scrum**, desenvolvido em equipe com foco em organização, produtividade e boas práticas de desenvolvimento full-stack.

---

## 💡 Sobre o projeto

O **Tarefa360** foi desenvolvido como parte de um desafio proposto pela **ITERA 360**, dentro de um programa de formação em desenvolvimento full-stack.

O projeto teve como objetivo simular um ambiente real de desenvolvimento ágil, aplicando na prática conceitos de **Scrum**, colaboração em equipe e construção de uma aplicação completa (front-end + back-end).

---

## 👥 Desenvolvimento em equipe

O sistema foi desenvolvido por uma equipe de 5 integrantes, utilizando o **Azure DevOps** como ferramenta de versionamento e gestão.

Durante o desenvolvimento:

- O projeto foi dividido em **3 Sprints**
- Houve rotação de papéis entre os integrantes:
  - Product Owner (PO)
  - Scrum Master (SM)
  - Desenvolvedor (DEV)
- Aplicação prática de metodologias ágeis no dia a dia do projeto

---

## 🖥️ Funcionalidades

✔️ Organização de tarefas em formato Kanban  
✔️ Gestão de Sprints  
✔️ Dashboard com visão geral do projeto  
✔️ Rotas protegidas (Protected Routes)  
✔️ Autenticação com JWT  
✔️ Login com verificação em duas etapas (2FA)  
✔️ Proteção com CAPTCHA  
✔️ Componentização com React  

---

## 🛠️ Tecnologias utilizadas

### Front-end
- React  
- React Router  
- Componentização de UI  

### Back-end
- .NET  
- Dapper  
- API REST  
- Entity Framework (migrations)

### DevOps & Metodologia
- Azure DevOps  
- Scrum (Sprints, backlog, divisão de papéis)  

### Segurança
- JWT (JSON Web Token)  
- Autenticação em dois fatores (2FA)  
- CAPTCHA  

---

## 📊 Dashboard

O sistema conta com um dashboard que permite:

- Visualizar o progresso das tarefas  
- Acompanhar sprints  
- Ter uma visão geral do fluxo de trabalho  

---

## 🔐 Segurança

Pensando em boas práticas, o projeto implementa:

- Autenticação baseada em token (JWT)  
- Verificação em duas etapas (2FA)  
- Proteção contra acessos automatizados com CAPTCHA  

---

## 🚀 Como rodar o projeto

### Pré-requisitos
- Node.js  
- .NET SDK  
- Banco de dados configurado corretamente  

---

### Front-end

```bash
cd frontend
npm install
npm start
```

### Back-End
```bash
# Acesse a pasta de repositório (migrations)
cd repository

# Atualize o banco de dados
dotnet ef database update

# Volte para a raiz do projeto
cd ..

# Compile a aplicação
dotnet build

# Execute a API
cd API
dotnet run
```
