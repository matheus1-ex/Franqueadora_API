using System.ComponentModel.DataAnnotations;
using Franqueada.API.Models;
namespace Franqueada.API.DTOs;
public sealed class UnidadeResponseDto
{
    // Mostra o id da unidade
    public int Id_Und {get; set;}

    // Mostra o nome da unidade
    public string? NomeUnidade {get; set;}

    public string? Cidade {get; set;}

    public string? Estado {get; set;}

    public string? Telefone {get; set;}

    // Mostra o código do identificador
    public string? CodIdentificador {get; set;}

    // Mostra o endereço por onde vai a unidade (num sei se esse contexto está certo).
    public string? Endereco {get; set;}

    public int FranquiaId {get; set;}
    
    // Mostra o status da unidade se ela está ativada ou não
    [EnumDataType(typeof(StatusAtivo), ErrorMessage = "Status da Unidade inválida.")]
    public StatusAtivo status {get; set;}

}