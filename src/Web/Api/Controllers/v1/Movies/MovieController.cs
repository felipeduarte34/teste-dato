using System;
using System.IO;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TesteBackend.Api.Controllers.v1.Movies.Requests;
using TesteBackend.ApiFramework.Tools;
using TesteBackend.Application.Movies.Command.AddMovie;
using TesteBackend.Application.Movies.Command.ProcessExcelMovieFile;
using TesteBackend.Application.Movies.Query.GetMovieById;
using TesteBackend.Application.Movies.Query.GetMovies;
using TesteBackend.Application.Movies.Query.ReadMovieFromRedis;
using TesteBackend.Common.Utilities;
using TesteBackend.Domain.Entities.Movies;

namespace TesteBackend.Api.Controllers.v1.Movies;

[ApiVersion("1")]
public class MovieController : BaseControllerV1
{
    [HttpPost]
    [SwaggerOperation("add a movie")]
    public async Task<IActionResult> AddAsync([FromBody] AddMovieRequest request)
    {
        var command = request.Adapt<AddMovieCommand>();

        var result = await Mediator.Send(command);

        return new ApiResult<Guid>(result);
    }
    
    [HttpGet]
    [SwaggerOperation("get a movie by id")]
    public async Task<IActionResult> GetByIdAsync([FromQuery] Guid movieId)
    {
        var result = await Mediator.Send(new GetMovieByIdQuery { MovieId = movieId });
        return new ApiResult<MovieQueryModel>(result);
    }

    [HttpGet("all")]
    [SwaggerOperation("get all movies")]
    public async Task<IActionResult> GetAllAsync(GetMoviesRequest request)
    {
        var query = request.Adapt<GetMoviesQuery>();

        var result = await Mediator.Send(query);
        return new ApiResult<PagedResult<Movie>>(result);
    }

    [HttpGet("cache-redis")]
    [SwaggerOperation("get a movie from cache. this is a example for how to use cache")]
    public async Task<IActionResult> ReadFromCacheAsync([FromQuery] Guid movieId)
    {
        var result = await Mediator.Send(new ReadMovieFromRedisQuery(movieId));
        return new ApiResult<ReadMovieFromRedisResponse>(result);
    }
    
    [HttpPost("upload")]
    [SwaggerOperation("upload an excel file")]
    public async Task<IActionResult> UploadExcelFileAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("File is not selected");

        var filePath = Path.Combine(Path.GetTempPath(), file.FileName.Replace(" ", "_").ToLower());

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // Inicie o processamento do arquivo Excel
        var command = new ProcessExcelMovieFileCommand(filePath, file.FileName.Replace(" ", "_").ToLower());
        var result = await Mediator.Send(command);

        return new ApiResult<Guid>(result);
    }
}
