using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
namespace Franqueada.Api.Middleware;

public sealed class RequestLogginMiddleWare
{
    private readonly RequestDelegate _proximo;
    private readonly Ilogger<RequestLogginMiddleWare> _logger;

    public RequestLogginMiddleWare(
        RequestLogginMiddleWare proximo,
        ILogger<RequestLogginMiddleWare> logger
    )
    {
        _proximo = proximo;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext contexto)
    {
        //Momento incial
        var inicio = Stopwatch.GetTimestamp();

    _logger.LogInformation(
        "Iniciando {Método} {Caminho}",
        contexto.Request.Method,
        contexto.Request.Path
    );

    try
    {
        await _proximo(contexto);
    }
    finally
        {
            var duracao = Stopwatch.GetElapsedTime(inicio);
            _logger.LogInformation(
                "Finalizando {Metodo} {Caminho}" +
                "com status {Status} em {Milissegundos:F2} ms",
                contexto.Request.Method,
                contexto.Request.Path,
                contexto.Response.StatusCode,
                duracao.TotalMilliseconds
            );
        }
    }
    
}