using Microsoft.AspNetCore.Mvc;
using Franqueada.API.DTOs;
using Fraqueada.API.Models;
using Franqueada.API.Data;

namespace Franqueda.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProdutoController : ControllerBase
{
    private readonly IProdutoService _produtoService;
    
    public ProdutosController(IProdutoService produtoService)
    {
        _produtoService = produtoService;
    }

}