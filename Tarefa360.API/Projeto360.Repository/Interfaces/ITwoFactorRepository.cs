using Projeto360.Domain.Entities;

namespace Projeto360.Repository.Interfaces;
public interface ITwoFactorRepository
{
    // Salva o novo código gerado no banco
    Task CriarAsync(TwoFactorToken token);
    // Busca um código válido (não expirado, não utilizado) para o usuário
    Task<TwoFactorToken> ObterTokenValidoAsync(int usuarioId, string codigo);
    // Marca o código como já usado para evitar reuso
    Task MarcarComoUtilizadoAsync(TwoFactorToken token);
    // Invalida todos os códigos pendentes do usuário (ao gerar um novo)
    Task InvalidarTokensAnterioresAsync(int usuarioId);
}