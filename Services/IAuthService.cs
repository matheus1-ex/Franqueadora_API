using System.Linq.Expressions;
using Franqueada.API.DTOs;
using Franqueada.API.Models;

namespace Franqueada.API.Services;

public interface IAuthService
{
    /// <summary>
        /// Autentica o usuário e gera o Token JWT caso as credenciais sejam válidas.
        /// </summary>
        Task<LoginResponseDto> LoginAsync(LoginRequestDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Cadastra um novo usuário no sistema.
        /// </summary>
        Task<LoginResponseDto> RegistrarAsync(LoginRequestDto dto, CancellationToken cancellationToken = default);

        /// <summary>
        /// Valida se o token atual ainda é válido.
        /// </summary>
        Task<bool> ValidarTokenAsync(string token, CancellationToken cancellationToken = default);

}