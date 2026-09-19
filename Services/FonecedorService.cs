using Microsoft.EntityFrameworkCore;
using Franqueada.API.Models;
using Franqueada.API.Data;

namespace Franqueada.API.Services;

public class FornecedorService : IFornecedorService
    {
        private readonly AppDbContext _context;

        public FornecedorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<FornecedorResponseDto>> ObterTodosAsync(CancellationToken cancellationToken)
        {
            return await _context.Fornecedores.Select
            (fornecedor => new FornecedorResponseDto
            {
                Id = fornecedor.Id,
                NomeRazaoSocial = fornecedor.NomeRazaoSocial,
                Cnpj = fornecedor.Cnpj,
                Telefone = fornecedor.Telefone,
                Email = fornecedor.Email,
                Status = fornecedor.Status,
                TotalProdutos = fornecedor.Produtos.Count //total de produtos que o fornecedor tem
            }).ToListAsync(cancellationToken);
        }

        public async Task<FornecedorResponseDto?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Fornecedores
            .AsNoTracking()
            .Where(f => f.Id == id)
            .Select(f => new FornecedorResponseDto
            {
                Id = f.Id,
                NomeRazaoSocial = f.NomeRazaoSocial,
                Email = f.Email,
                Telefone = f.Telefone,
                Cnpj = f.Cnpj,
                Status = f.Status,
                TotalProdutos = f.Produtos.Count
            })
            .FirstOrDefaultAsync(cancellationToken);
            }

        public async Task<FornecedorResponseDto> CriarAsync(FornecedorRequestDto dto, CancellationToken cancellationToken = default)
        {
            var fornecedor = new Fornecedor
            {
                NomeRazaoSocial = dto.NomeRazaoSocial,
                Cnpj = dto.Cnpj,
                Telefone = dto.Telefone,
                Email = dto.Email,
                Status = StatusAtivo.Ativado
            };

            _context.Fornecedores.Add(fornecedor);
            await _context.SaveChangesAsync(cancellationToken);

            return MapToDto(fornecedor);
        }

        public async Task<bool> AtualizarAsync(int id, FornecedorRequestDto dto, CancellationToken cancellationToken = default)
        {
            var fornecedor = await _context.Fornecedores.FindAsync(new object[] { id }, cancellationToken);
            if (fornecedor == null) return false;

            fornecedor.NomeRazaoSocial = dto.NomeRazaoSocial;
            fornecedor.Cnpj = dto.Cnpj;
            fornecedor.Telefone = dto.Telefone;
            fornecedor.Email = dto.Email;

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> AlternarStatusAsync(int id, CancellationToken cancellationToken = default)
        {
            var fornecedor = await _context.Fornecedores.FindAsync(new object[] { id }, cancellationToken);
            if (fornecedor == null) return false;

            fornecedor.Status = fornecedor.Status == StatusAtivo.Ativado ? StatusAtivo.Desativado : StatusAtivo.Ativado;
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> AssociarAoProdutoAsync(int produtoId, int fornecedorId, CancellationToken cancellationToken = default)
        {
            var produto = await _context.Produtos.FindAsync(new object[] { produtoId }, cancellationToken);
            var fornecedor = await _context.Fornecedores.FindAsync(new object[] { fornecedorId }, cancellationToken);

            if (produto == null || fornecedor == null) return false;

            produto.FornecedorId = fornecedorId;
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> RemoverAsync(int id, CancellationToken cancellationToken)
        {
            // 1. Busca o registro no banco pelo ID
            var entidade = await _context.Fornecedores
                .FirstOrDefaultAsync(f => f.Id == id, cancellationToken);

            if (entidade == null)
            {
                return false;
            }

            // Verficar antes de remover o fornecedor, se tem produto
            var TemProduto = await _context.Produtos.AnyAsync(produto => produto.FornecedorId == id, cancellationToken);
            if (TemProduto)
            {
                throw new InvalidOperationException("Existe produto cadastrado nesse fornecedor, não foi possível remover o fornecedor.");
            }

            // 3. Remove a entidade do DbContext e persiste no banco
            _context.Fornecedores.Remove(entidade);
            await _context.SaveChangesAsync(cancellationToken);

            // 4. Retorna true para confirmar que a exclusão foi concluída
            return true;
        }

        private static FornecedorResponseDto MapToDto(Fornecedor f) => new()
        {
            Id = f.Id,
            NomeRazaoSocial = f.NomeRazaoSocial,
            Cnpj = f.Cnpj,
            Telefone = f.Telefone,
            Email = f.Email,
            Status = f.Status,
            TotalProdutos = f.Produtos.Count
        };
    }