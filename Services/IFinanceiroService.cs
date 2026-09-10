using Microsoft.EntityFrameworkCore;
using Franqueada.API.Models;
using Franqueada.API.DTOs;

namespace Franqueada.API.Services;
public interface IFinanceiroService
{
    Task DefinirPercentualRoyaltyAsync(SalvarConfiguracaoRoyaltyDto dto, CancellationToken cancellationToken = default);
    Task<RoyaltyResponseDto> CalcularEGerarRoyaltyAsync(GerarRoyaltyDto dto, CancellationToken cancellationToken = default);
    Task<bool> RegistrarPagamentoAsync(int lancamentoId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<RoyaltyResponseDto>> ConsultarValoresPorUnidadeAsync(int? unidadeId, StatusPagamento? status, CancellationToken cancellationToken = default);
}