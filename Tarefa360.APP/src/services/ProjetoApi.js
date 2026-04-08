import { HTTPClient } from './client';

const ProjetoApi = {
    async ObterProjetoPorIdAsync(projetoId) {
        try {
            const response = await HTTPClient.get(`/Projeto/ObterProjetoPorId/${projetoId}`);
            return response.data;
        } catch (error) {
            console.error('Erro ao obter projeto:', error);
            throw error;
        }
    },

    async ObterProjetoPorNomeAsync(nome) {
        try {
            const response = await HTTPClient.get(`/Projeto/ObterProjetoPorNome/${nome}`);
            return response.data;
        } catch (error) {
            console.error('Erro ao obter projeto:', error);
            throw error;
        }
    },

    async ListarProjetoAsync(ativo) {
        try {
            const response = await HTTPClient.get(`/Projeto/ListarProjeto?ativo=${ativo}`);
            return response.data;
        } catch (error) {
            console.error('Erro ao listar projetos:', error);
            throw error;
        }
    },

    async CriarProjetoAsync(nome, descricao) {
        try {
            const projetoRequest = {
                Nome: nome,
                Descricao: descricao
            };

            const response = await HTTPClient.post('/Projeto/CriarProjeto', projetoRequest);
            return response.data;
        } catch (error) {
            console.error('Erro ao criar projeto:', error);
            throw error;
        }
    },

    async AtualizarProjetoAsync(projetoId, nome, descricao) {
        try {
            const projetoRequest = {
                Id: projetoId,
                Nome: nome,
                Descricao: descricao
            };

            const response = await HTTPClient.put(`/Projeto/AtualizarProjeto/${projetoId}`, projetoRequest);
            return response.data;
        } catch (error) {
            console.error('Erro ao atualizar projeto:', error);
            throw error;
        }
    },

    async DeletarProjetoAsync(projetoId) {
        try {
            const response = await HTTPClient.delete(`/Projeto/DeletarProjeto/${projetoId}`);
            return response.data;
        } catch (error) {
            console.error('Erro ao excluir projeto:', error);
            throw error;
        }
    },

    async ListarProjetoDropdown(ativo) {
        try {
            const response = await HTTPClient.get(`/Projeto/ListarProjetoDropdown?ativo=${ativo}`);
            return response.data;
        } catch (error) {
            console.error('Erro ao listar projetos dropdown:', error);
            throw error;
        }
    }
}

export default ProjetoApi;