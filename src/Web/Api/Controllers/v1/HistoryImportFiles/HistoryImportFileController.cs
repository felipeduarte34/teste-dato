using System;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TesteBackend.Api.Controllers.v1.HistoryImportFiles.Requests;
using TesteBackend.ApiFramework.Tools;
using TesteBackend.Application.HistoryImportFiles.Query.GetAllHistoryImportFiles;
using TesteBackend.Application.HistoryImportFiles.Query.GetHistoryImportFileById;
using TesteBackend.Common.Utilities;
using TesteBackend.Domain.Entities.HistoryImportFiles;
namespace TesteBackend.Api.Controllers.v1.HistoryImportFiles;

[ApiVersion("1")]
public class HistoryImportFileController : BaseControllerV1
{
    [HttpGet]
    [SwaggerOperation("get a historyImportFile by id")]
    public async Task<IActionResult> GetByIdAsync([FromQuery] Guid historyImportFileId)
    {
        var result = await Mediator.Send(new GetHistoryImportFileByIdQuery { HistoryImportFileId = historyImportFileId });
        return new ApiResult<HistoryImportFileQueryModel>(result);
    }
    
    [HttpGet("all")]
    [SwaggerOperation("get all historyImportFile")]
    public async Task<IActionResult> GetAllAsync(GetHistoryImportFilesRequest request)
    {
        var query = request.Adapt<GetHistoryImportFilesQuery>();

        var result = await Mediator.Send(query);
        return new ApiResult<PagedResult<HistoryImportFile>>(result);
    }
}