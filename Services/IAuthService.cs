using Franqueada_API.DTOs;
using Franqueada_API.Models;

namespace Franqueada_API.Services;

public interface IAuthService
{
    /// <summary>
    /// Vai pegar todas as informações da Classe Usuario
    /// </summary>
    /// <param name="Nome"></param>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IReadOnlyCollection<Usuario>> ObterTodosAsync(
        Perfil? Nome,
        int id,
        CancellationToken cancellationToken
    );


    /// <summary>
    /// Vai Pegar todos os IDs do Usuário
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>

    Task<Usuario?> ObterTodosIdAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Criar Usuário
    /// </summary>
    /// <param name="dados"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>

    Task<Usuario> CriarAsync
    (
        UsuarioCadastroDto dados,
        CancellationToken cancellationToken
    );




}