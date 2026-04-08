import { HTTPClient } from './client';

const HistoriaApi =
{
  async CriarHistoria(nome, descricao, projetoId) {
    try {
      const historiaRequest = {
        Nome: nome,
        Descricao: descricao,
        ProjetoId: projetoId
      };

      const response = await HTTPClient.post('/Historia/CriarHistoria', historiaRequest);
      return response.data;
    } catch (error) {
      console.error('Erro ao criar história:', error);
      throw error;
    }
  },

  async ObterHistoriaPorId(Id) {
    try {
      const response = await HTTPClient.get(`/Historia/ObterHistoriaPorId/${Id}`);
      return response.data;
    } catch (error) {
      console.error('Erro ao obter história:', error);
      throw error;
    }
  },

  async ObterHistoriaPorNome(nome) {
    try {
      const response = await HTTPClient.get(`/Historia/ObterHistoriaPorNome?nome=${nome}`);
      return response.data;
    } catch (error) {
      console.error('Erro ao obter história por nome:', error);
      throw error;
    }
  },

  async ListarHistorias() {
    try {
      const response = await HTTPClient.get(`/Historia/ListarHistorias`);
      return response.data;
    } catch (error) {
      console.error('Erro ao listar histórias:', error);
      throw error;
    }
  },

  async ListarHistoriasPorProjeto(projetoId) {
    try {
      const response = await HTTPClient.get(`/Historia/ListarHistoriasPorProjeto/${projetoId}`);
      return response.data;
    } catch (error) {
      console.error('Erro ao listar histórias por projeto:', error);
      throw error;
    }
  },

  async AtualizarHistoria(id, nome, descricao, projetoId) {
    try {
      const historiaAtualizar = {
        Nome: nome,
        Descricao: descricao,
        ProjetoId: projetoId
      };

      const response = await HTTPClient.put(`/Historia/AtualizarHistoria/${id}`, historiaAtualizar);
      return response.data;
    } catch (error) {
      console.error('Erro ao atualizar história:', error);
      throw error;
    }
  },

  async DeletarHistoria(id) {
    try {
      const response = await HTTPClient.delete(`/Historia/DeletarHistoria/${id}`);
      return response.data;
    } catch (error) {
      console.error('Erro ao deletar história:', error);
      throw error;
    }
  }
}

export default HistoriaApi;