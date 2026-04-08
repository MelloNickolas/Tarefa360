import {HTTPClient} from './client';

const DashboardProjetosApi = {
    async QtdTotalHistorias() {
        try {
            const response = await HTTPClient.get(`/DashboardProjetos/QtdTotalHistorias`);
            return response.data;
        } catch (error) {
            console.error('Erro ao obter quantidade total de histórias:', error);
            throw error;
        }
    },

    async QtdHistoriasPorProjeto(projetoID) {
        try {
            const response = await HTTPClient.get(`/DashboardProjetos/QtdHistoriasPorProjeto/${projetoID}`);
            return response.data;
        } catch (error) {
            console.error('Erro ao obter quantidade de histórias por projeto:', error);
            throw error;
        }
    },

    async QtdHistoriasPorProjetoAgrupado()
    {
        try {
            const response = await HTTPClient.get(`/DashboardProjetos/QtdHistoriasPorProjetoAgrupado`);
            return response.data;
        } catch (error) {
            console.error('Erro ao obter quantidade de histórias por projeto agrupado:', error);
            throw error;
        }
    },

    async QtdHistoriasPorConclusao(concluido) {
        try {
            const response = await HTTPClient.get(`/DashboardProjetos/QtdHistoriasPorConclusao/?concluido=${concluido}`);
            return response.data;
        } catch (error) {
            console.error('Erro ao obter quantidade de histórias por conclusão:', error);
            throw error;
        }
    },

    async QtdTotalTarefasAgrupadoPorTipo() {
        try {
            const response = await HTTPClient.get(`/DashboardProjetos/QtdTotalTarefasAgrupadoPorTipo`);
            return response.data;
        } catch (error) {
            console.error('Erro ao obter quantidade total de tarefas agrupado por tipo:', error);
            throw error;
        }
    },

    async QtdTarefasAgrupadoPorTipoPorConclusao(concluido) {
        try {
            const response = await HTTPClient.get(`/DashboardProjetos/QtdTarefasAgrupadoPorTipoPorConclusao/?concluido=${concluido}`);
            return response.data;
        } catch (error) {
            console.error('Erro ao obter quantidade total de tarefas agrupado por tipo e conclusão:', error);
            throw error;
        }
    },

    // Tipos: 1 - bug, 2 - funcionalidade, 3 - melhoria, 4 - tarefa 
    async QtdTotalTarefasPorTipoPorConclusao(tipoTarefa, concluido) {
        try {
            const response = await HTTPClient.get(`/DashboardProjetos/QtdTotalTarefasPorTipoPorConclusao/?concluido=${concluido}&tipoTarefa=${tipoTarefa}`);
            return response.data;
        } catch (error) {
            console.error('Erro ao obter quantidade total de tarefas por tipo e conclusão:', error);
            throw error;
        }
    },

    async QtdTotalTarefasPorConclusao(concluido) {
        try {
            const response = await HTTPClient.get(`/DashboardProjetos/QtdTotalTarefasPorConclusao/?concluido=${concluido}`);
            return response.data;
        } catch (error) {
            console.error('Erro ao obter quantidade total de tarefas por conclusão:', error);
            throw error;
        }
    },

    async QtdHorasPorConclusao(concluido) {
        try {
            const response = await HTTPClient.get(`/DashboardProjetos/QtdHorasPorConclusao/?concluido=${concluido}`);
            return response.data;
        } catch (error) {
            console.error('Erro ao obter quantidade total de horas agrupado por conclusão:', error);
            throw error;
        }
    },

    async ListarTarefasConcluidasDataAtual() {
        try {
            const response = await HTTPClient.get(`/DashboardProjetos/ListarTarefasConcluidasDataAtual`);
            return response.data;
        } catch (error) {
            console.error('Erro ao listar tarefas concluídas na data atual:', error);
            throw error;
        }
    },
}

export default DashboardProjetosApi;