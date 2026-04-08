import {HTTPClient} from './client';

const SprintApi = {
    async ObterSprintPorIdAsync(sprintId) {
        try {
            const response = await HTTPClient.get(`/Sprint/ObterSprintPorId/${sprintId}`);
            return response.data;
        } catch (error) {
            console.error('Erro ao obter sprint:', error);
            throw error;
        }
    },

    async ObterSprintPorNomeAsync(nome) {
        try {
            const response = await HTTPClient.get(`/Sprint/ObterSprintPorTitulo/${nome}`);
            return response.data;
        } catch (error) {
            console.error('Erro ao obter sprint:', error);
            throw error;
        }
    },

    async ListarSprintAsync() {
        try {
            const response = await HTTPClient.get(`/Sprint/ListarSprints`);
            return response.data;
        } catch (error) {
            console.error('Erro ao listar sprints:', error);
            throw error;
        }
    },

    async CriarSprintAsync(titulo, descricao, projetoId) {
        try {
            const sprintRequest = {
                Titulo: titulo,
                Descricao: descricao,
                ProjetoId: projetoId
            };

            const response = await HTTPClient.post('/Sprint/CriarSprint', sprintRequest);
            return response.data;
        } catch (error) {
            console.error('Erro ao criar sprint:', error);
            throw error;
        }
    },

    async AtualizarSprintAsync(sprintId, titulo, descricao, projetoId) {
        try {
            const sprintRequest = {
                Titulo: titulo,
                Descricao: descricao,
                ProjetoId: projetoId
            };
    
            const response = await HTTPClient.put(
                `/Sprint/AtualizarSprint/${sprintId}`, 
                sprintRequest
            );
    
            return response.data;
    
        } catch (error) {
            console.error('Erro ao atualizar sprint:', error);
            throw error;
        }
    },

    async DeletarSprintAsync(sprintId) {
        try {
            const response = await HTTPClient.delete(`/Sprint/DeletarSprint/${sprintId}`);
            return response.data;
        } catch (error) {
            console.error('Erro ao excluir sprint:', error);
            throw error;
        }
    },
}

export default SprintApi;