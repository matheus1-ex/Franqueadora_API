namespace Franqueada.API.Services
{
    public interface ITokenService
    {
        string GerarToken(string usuarioId, string email, string? perfilNome);
    }
}