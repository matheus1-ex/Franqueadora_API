using Franqueada.API.DTOs;
namespace Franqueada.API.Services;
public interface IVendaService
{
    Task<VendaResponseDto> RegistrarVendaAsync(CriarVendaRequestDto dto, CancellationToken cancellationToken = default);
    Task<VendaResponseDto?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);
}
