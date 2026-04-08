import { HTTPClient } from './client';

const TarefaApi = {
    async ObterPorIdAsync(tarefaId) {
        try {
            const response = await HTTPClient.get(`/Tarefa/ObterPorId/${tarefaId}`);
            return response.data;
        } catch (error) {
            console.error('Erro ao obter tarefa:', error);
            throw error;
        }
    },

    // Deve ser enviado um dos seguintes status: Todos, Concluida ou Pendente
    async ListarPorStatusAsync(status) {
        try {
            if (status === 'Todos') {
                const response = await HTTPClient.get(`/Tarefa/ListarTodas`);
                return response.data;
            }
            else if (status === 'Concluida') {
                const response = await HTTPClient.get(`/Tarefa/ListarPorStatus?concluida=${true}`);
                return response.data;
            }
            else if (status === 'Pendente') {
                const response = await HTTPClient.get(`/Tarefa/ListarPorStatus?concluida=${false}`);
                return response.data;
            }
            else {
                throw new Error('Status inválido');
            }
        } catch (error) {
            console.error('Erro ao listar tarefas:', error);
            throw error;
        }
    },

    async ListarTiposTarefa() {
        try {
            const response = await HTTPClient.get(`/Tarefa/ListarTiposTarefa`);
            return response.data;
        } catch (error) {
            console.error('Erro ao listar tarefas:', error);
            throw error;
        }
    },

    // Para criar a tarefa é necessário os seguintes dados:
    // Titulo, Descricao, Estimativa (opcional), 
    // TipoTarefa, ProjetoId, HistoriaId, UsuarioId e SprintId
    async CriarAsync(tarefa) {
        try {
            const TarefaRequest = {
                titulo: tarefa.titulo,
                descricao: tarefa.descricao,
                tipoTarefaID: Number(tarefa.tipoTarefaID),
                projetoID: Number(tarefa.projetoID),
                historiaID: Number(tarefa.historiaID),
                usuarioID: Number(tarefa.usuarioID),
                sprintID: Number(tarefa.sprintID),
                ...(tarefa.estimativa !== '' && {
                    estimativa: Number(tarefa.estimativa)
                })
            };

            console.log("ENVIANDO:", TarefaRequest);

            const response = await HTTPClient.post('/Tarefa/Criar', TarefaRequest);
            return response.data;

        } catch (error) {
            console.error('Erro ao criar tarefa:', error);
            throw error;
        }
    },
    async AtualizarAsync(tarefaId, tarefaAtualizada) {
        try {
            const response = await HTTPClient.put(`/Tarefa/Atualizar/${tarefaId}`, tarefaAtualizada);
            return response.data;
        } catch (error) {
            console.error('Erro ao atualizar tarefa:', error);
            throw error;
        }
    },

    async ConcluirAsync(tarefaId) {
        try {
            const response = await HTTPClient.put(`/Tarefa/Concluir/${tarefaId}`);
            return response.data;
        } catch (error) {
            console.error('Erro ao concluir tarefa:', error);
            throw error;
        }
    },

    async RetomarAsync(tarefaId) {
        try {
            const response = await HTTPClient.put(`/Tarefa/Retomar/${tarefaId}`);
            return response.data;
        } catch (error) {
            console.error('Erro ao retomar tarefa:', error);
            throw error;
        }
    },

    async DeletarAsync(tarefaId) {
        try {
            const response = await HTTPClient.delete(`/Tarefa/Deletar/${tarefaId}`);
            return response.data;
        } catch (error) {
            console.error('Erro ao excluir tarefa:', error);
            throw error;
        }
    },
}

export default TarefaApi;