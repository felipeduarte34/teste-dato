using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TesteBackend.Api.Controllers.v1.Pokemons.Requests;
using TesteBackend.ApiFramework.Tools;
using TesteBackend.Application.Pokemons.Command.AddPokemon;
using TesteBackend.Application.Pokemons.Command.ProcessExcelPokemonFile;
using TesteBackend.Application.Pokemons.Query.GetMyPokemons;
using TesteBackend.Application.Pokemons.Query.GetPokemonById;
using TesteBackend.Application.Pokemons.Query.GetPokemons;
using TesteBackend.Common.Utilities;
using TesteBackend.Domain.Entities.Pokemons;

namespace TesteBackend.Api.Controllers.v1.Pokemons;

[ApiVersion("1")]
public class PokemonController : BaseControllerV1
{
    [HttpPost]
    [SwaggerOperation("add a pokemon")]
    [AllowAnonymous]
    public async Task<IActionResult> AddAsync([FromBody] AddPokemonRequest request)
    {
        var command = request.Adapt<AddPokemonCommand>();

        var result = await Mediator.Send(command);

        return new ApiResult<int>(result);
    }
    
    [HttpGet]
    [SwaggerOperation("get a pokemon by id")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByIdAsync([FromQuery] int pokemonId)
    {
        var result = await Mediator.Send(new GetPokemonByIdQuery { PokemonId = pokemonId });
        return new ApiResult<PokemonQueryModel>(result);
    }

    [HttpGet("all")]
    [SwaggerOperation("get all pokemons")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllAsync(GetPokemonsRequest request)
    {
        var query = request.Adapt<GetPokemonsQuery>();

        var result = await Mediator.Send(query);
        return new ApiResult<PagedResult<Pokemon>>(result);
    }
    
    [HttpGet("my")]
    [SwaggerOperation("get all my pokemons")]
    public async Task<IActionResult> GetMyPokemonsAsync(GetPokemonsRequest request)
    {
        // Obter o usuário atual através do HttpContext
        var currentUser = HttpContext.User;

        // Você pode agora usar o currentUser para obter informações do usuário
        // Por exemplo, para obter o ID do usuário:
        var userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // Agora você pode usar o userId para filtrar os Pokemons pertencentes a este usuário

        var query = request.Adapt<GetMyPokemonsQuery>();
        query.UserId = Convert.ToInt32(userId); // Adicione o UserId ao seu query

        var result = await Mediator.Send(query);
        return new ApiResult<PagedResult<Pokemon>>(result);
    }
    
    [HttpPost("upload-catalog")]
    [SwaggerOperation("upload an excel file for catalog pokemons")]
    public async Task<IActionResult> UploadExcelCatalogPokemonsFileAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("File is not selected");

        var filePath = Path.Combine(Path.GetTempPath(), file.FileName.Replace(" ", "_").ToLower());

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // Inicie o processamento do arquivo Excel
        var command = new ProcessExcelCatalogPokemonsFileCommand(filePath, file.FileName.Replace(" ", "_").ToLower());
        var result = await Mediator.Send(command);

        return new ApiResult<Guid>(result);
    }
    
    [HttpPost("upload-my")]
    [SwaggerOperation("upload an excel file for my pokemons")]
    public async Task<IActionResult> UploadExcelMyPokemonsFileAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("File is not selected");

        var filePath = Path.Combine(Path.GetTempPath(), file.FileName.Replace(" ", "_").ToLower());

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
        
        // Obter o usuário atual através do HttpContext
        var currentUser = HttpContext.User;

        // Você pode agora usar o currentUser para obter informações do usuário
        // Por exemplo, para obter o ID do usuário:
        var userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        // Inicie o processamento do arquivo Excel
        var command = new ProcessExcelMyPokemonsFileCommand(
            filePath, 
            file.FileName.Replace(" ", "_").ToLower(), 
            Convert.ToInt32(userId));
        var result = await Mediator.Send(command);

        return new ApiResult<Guid>(result);
    }
}