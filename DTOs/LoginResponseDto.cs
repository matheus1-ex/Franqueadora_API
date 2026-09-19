using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using Franqueada.API.Models;
namespace Franqueada.API.DTOs;
public sealed class LoginResponseDto
{
    ///
    /// 
    /// Aqui mostra os resultados do usuário
    /// 
    /// 
    /// 
    

    /// <summary>
    /// Mensagem serve para dar um retorno claro sobre o resultado da tentativa de autenticação
    /// </summary>/
    public string Mensagem { get; set; } = string.Empty;

    /// <summary>
    /// Nome do usuário
    /// </summary>
    public string? Nome {get; set;}

    /// <summary>
    /// Email do usuário
    /// </summary>
    public string? EmailUsuario { get; set; }

    /// <summary>
    /// Senha
    /// </summary>
    public string Senha {get; set;} = string.Empty;

    /// <summary>
    /// Tipo de usuario (Admin, Gestão, usr)
    /// </summary>
    public string TipoUsuario {get; set;} = string.Empty;

    /// <summary>
    /// Token gerado
    /// </summary>
    public string? Token {get; set;}

    /// <summary>
    /// Data de Expiração
    /// </summary>
    public DateTime? DatadeExpiracao {get; set;} 

    /// <summary>
    /// Status da Conta
    /// </summary>
    public StatusAtivo StatusConta {get; set;} = StatusAtivo.Ativado;

}