import { HTTPClient } from './client';

const EquipeApi = {
    async ObterEquipePorIdAsync(equipeId) {
        try {
            const response = await HTTPClient.get(`/Equipe/ObterEquipePorId/${equipeId}`);
            return response.data;
        } catch (error) {
            console.error('Erro ao obter equipe:', error);
            throw error;
        }
    },

    async ListarEquipesComMembrosAsync() {
        try {
            const response = await HTTPClient.get('/Equipe/ListarMembrosDaEquipe');
            return response.data;
        } catch (error) {
            console.error('Erro ao listar equipes com membros:', error);
            throw error;
        }
    },

    async CriarEquipeAsync(nome) {
        try {
            const equipeRequest = { Nome: nome };
            const response = await HTTPClient.post('/Equipe/CriarEquipe', equipeRequest);
            return response.data;
        } catch (error) {
            console.error('Erro ao criar equipe:', error);
            throw error;
        }
    },

    async AtualizarEquipeAsync(equipeId, nome) {
        try {
            const equipeRequest = { Nome: nome };
            const response = await HTTPClient.put(`/Equipe/AtualizarEquipe/${equipeId}`, equipeRequest);
            return response.data;
        } catch (error) {
            console.error('Erro ao atualizar equipe:', error);
            throw error;
        }
    },

    async DeletarEquipeAsync(equipeId) {
        try {
            const response = await HTTPClient.delete(`/Equipe/DeletarEquipe/${equipeId}`);
            return response.data;
        } catch (error) {
            console.error('Erro ao deletar equipe:', error);
            throw error;
        }
    },

    async ListarUsuarioEquipePorEquipeAsync(equipeId) {
        try {
            const response = await HTTPClient.get(`/UsuarioEquipe/ListarUsuarioEquipePorEquipe?equipeID=${equipeId}`);
            return response.data;
        } catch (error) {
            console.error('Erro ao listar membros da equipe:', error);
            throw error;
        }
    },

    async CriarUsuarioEquipeAsync(papeisEquipe, usuarioID, equipeID) {
        try {
            const request = { PapeisEquipe: papeisEquipe, UsuarioID: usuarioID, EquipeID: equipeID };
            const response = await HTTPClient.post('/UsuarioEquipe/CriarUsuarioEquipe', request);
            return response.data;
        } catch (error) {
            console.error('Erro ao adicionar membro:', error);
            throw error;
        }
    },

    async AtualizarPapelAsync(usuarioEquipeId, papeisEquipe) {
        try {
            const request = { PapeisEquipe: papeisEquipe };
            const response = await HTTPClient.put(`/UsuarioEquipe/AtualizarPapelUsuarioEquipePorId/${usuarioEquipeId}`, request);
            return response.data;
        } catch (error) {
            console.error('Erro ao atualizar papel:', error);
            throw error;
        }
    },

    async DeletarUsuarioEquipeAsync(usuarioEquipeId) {
        try {
            const response = await HTTPClient.delete(`/UsuarioEquipe/DeletarUsuarioEquipe/${usuarioEquipeId}`);
            return response.data;
        } catch (error) {
            console.error('Erro ao remover membro:', error);
            throw error;
        }
    },

    async ListarPapeisEquipeAsync() {
        try {
            const response = await HTTPClient.get('/UsuarioEquipe/ListarPapeisEquipe');
            return response.data;
        } catch (error) {
            console.error('Erro ao listar papeis:', error);
            throw error;
        }
    },
}

export default EquipeApi;