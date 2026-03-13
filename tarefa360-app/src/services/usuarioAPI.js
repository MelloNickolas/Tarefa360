import { HTTPClient } from "./client";

const UsuarioAPI = {
  async obterAsync(usuarioId){
    try {
      const response = await HTTPClient.get(`/Usuario/Obter/${usuarioId}`)
      return response.data;
    }
    catch (error){
      console.error("Erro ao OBTER USUÁRIO: ", error);
      throw error;
    }
  },


  async listarAsync(ativos) {
    try {
      const response = await HTTPClient.get(`/Usuario/Listar?ativos=${ativos}`)
      return response.data;
    }
    catch (error){
      console.error("Erro ao LISTAR USUÁRIOS: ", error);
      throw error;
    }
  },

  async criarAsync(nome, email, senha, tipoUsuarioId) {
    try {
      const usuarioCriar = {
        TipoUsuarioId: tipoUsuarioId,
        Nome: nome,
        Email: email,
        Senha: senha
      }
      const response = await HTTPClient.post(`/Usuario/Criar`, usuarioCriar)
      return response.data;
    }
    catch (error){
      console.error("Erro ao CRIAR USUÁRIO: ", error);
      throw error;
    }
  },

  async atualizarAsync(id, nome, email, tipoUsuarioId) {
    try {
      const usuarioAtualizar = {
        TipoUsuarioId: tipoUsuarioId,
        Id: id,
        Nome: nome,
        Email: email
      }
      const response = await HTTPClient.put(`/Usuario/Atualizar`, usuarioAtualizar)
      return response.data;
    }
    catch (error){
      console.error("Erro ao ATUALIZAR USUÁRIO: ", error);
      throw error;
    }
  },

  async deletarAsync(usuarioId) {
    try {
      const response = await HTTPClient.delete(`/Usuario/Deletar/${usuarioId}`)
      return response.data;
    }
    catch (error){
      console.error("Erro ao DELETAR USUÁRIO: ", error);
      throw error;
    }
  },

  async listarTiposUsuarioAsync() {
    try {
      const response = await HTTPClient.get(`/api/Enum/ListarTiposUsuario`)
      return response.data;
    }
    catch (error){
      console.error("Erro ao LISTAR TIPOS DE USUÁRIO: ", error);
      throw error;
    }
  },

  async alterarSenhaAsync(id, senha, senhaAntiga) {
    try {
      const usuarioAlterarSenha = {
        Id: id,
        Senha: senha,
        SenhaAntiga: senhaAntiga
      }
      const response = await HTTPClient.put(`/Usuario/AtualizarSenha`, usuarioAlterarSenha)
      return response.data;
    }
    catch (error){
      console.error("Erro ao TROCA: ", error);
      throw error;
    }
  },

  async restaurarAsync(usuarioId) {
    try {
      const response = await HTTPClient.put(`/Usuario/Restaurar/${usuarioId}`)
      return response.data;
    }
    catch (error){
      console.error("Erro ao RESTAURAR USUÁRIO: ", error);
      throw error;
    }
  },
}

export default UsuarioAPI;