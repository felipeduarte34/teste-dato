using MediatR;
using TesteBackend.Common.Utilities;
using TesteBackend.Domain.Entities.HistoryImportFiles;

namespace TesteBackend.Application.HistoryImportFiles.Query.GetAllHistoryImportFiles;

public class GetHistoryImportFilesQuery : IRequest<PagedResult<HistoryImportFile>>
{
    public int Page { get; set; }
    public int PageSize { get; set; }
}