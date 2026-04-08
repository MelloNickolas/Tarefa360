import {HTTPClient} from './client';

const UsuarioApi = {
    async ObterPorIdAsync(usuarioId) {
        try {
            const response = await HTTPClient.get(`/Usuario/ObterPorId/${usuarioId}`);
            return response.data;
        } catch (error) {
            console.error('Erro ao obter usuário:', error);
            throw error;
        }
    },

    async ObterPorEmailAsync(email) {
        try {
            const response = await HTTPClient.get(`/Usuario/ObterPorEmail/${email}`);
            return response.data;
        } catch (error) {
            console.error('Erro ao obter usuário:', error);
            throw error;
        }
    },

    async ListarAsync(ativo) {
        try {
            const response = await HTTPClient.get(`/Usuario/Listar?ativo=${ativo}`);
            return response.data;
        } catch (error) {
            console.error('Erro ao listar usuários:', error);
            throw error;
        }
    },

    async ListarTiposUsuarioAsync() {
        try {
            const response = await HTTPClient.get('/Usuario/ListarTiposUsuario');
            return response.data;
        } catch (error) {
            console.error('Erro ao listar tipos de usuário:', error);
            throw error;
        }    
    },

    async CriarAsync(nome, email, senha, tipoUsuarioId) {
        try {
            const UsuarioRequest = {
                Nome: nome,
                Email: email,
                Senha: senha,
                TipoUsuarioId: tipoUsuarioId
            };

            const response = await HTTPClient.post('/Usuario/Criar', UsuarioRequest);
            return response.data;
        } catch (error) {
            console.error('Erro ao criar usuário:', error);
            throw error;
        }
    },

    async AtualizarAsync(usuarioId, nome, email, tipoUsuarioId) {
        try {
            const UsuarioRequest = {
                Id: usuarioId,
                Nome: nome,
                Email: email,
                TipoUsuarioId: tipoUsuarioId
            };

            const response = await HTTPClient.put(`/Usuario/Atualizar/${usuarioId}`, UsuarioRequest);
            return response.data;
        } catch (error) {
            console.error('Erro ao atualizar usuário:', error);
            throw error;
        }
    },

    async AlterarSenhaAsync(usuarioId, novaSenha) {
        try {
            const SenhaRequest = {
                Id: usuarioId,
                NovaSenha: novaSenha
            };

            const response = await HTTPClient.put(`/Usuario/AlterarSenha/${usuarioId}`, SenhaRequest);
            return response.data;
        } catch (error) {
            console.error('Erro ao alterar senha do usuário:', error);
            throw error;
        }
    },

    async DeletarAsync(usuarioId) {
        try {
            const response = await HTTPClient.delete(`/Usuario/Deletar/${usuarioId}`);
            return response.data;
        } catch (error) {
            console.error('Erro ao excluir usuário:', error);
            throw error;
        }
    },
}

export default UsuarioApi;