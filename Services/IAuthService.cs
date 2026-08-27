using Franqueada.API.DTOs;
using Franqueada.API.Models;

namespace Franqueada.API.IAuthService;

public interface IAuthService
{
    Task<IReadOnlyCollection<Usuario>> ObterTodosAsync(
        Perfil? Nome,
        int id,
        CancellationToken cancellationToken
    );
    Task<Usuario?> ObterTodosIdAsync(int id, CancellationToken cancellationToken);

    Task<Usuario> CriarAsync
    (
        UsuarioCadastroDto dados,
        CancellationToken cancellationToken
    );




}