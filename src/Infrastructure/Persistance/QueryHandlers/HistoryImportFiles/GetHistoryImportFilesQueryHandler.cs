using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MongoDB.Driver;
using TesteBackend.Application.HistoryImportFiles.Query.GetAllHistoryImportFiles;
using TesteBackend.Application.HistoryImportFiles.Query.GetHistoryImportFileById;
using TesteBackend.Common.Utilities;
using TesteBackend.Domain.Entities.HistoryImportFiles;

namespace TesteBackend.Persistence.QueryHandlers.HistoryImportFiles;

public class GetHistoryImportFilesQueryHandler: IRequestHandler<GetHistoryImportFilesQuery, PagedResult<HistoryImportFile>>
{
    private readonly MongoDbContext _dbContext;

    public GetHistoryImportFilesQueryHandler(MongoDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<PagedResult<HistoryImportFile>> Handle(GetHistoryImportFilesQuery request, CancellationToken cancellationToken)
    {
        var filter = Builders<HistoryImportFile>.Filter.Empty;

        var historyImportFiles = await _dbContext.GetCollection<HistoryImportFile>("HistoryImportFiles")
            .GetPaged(request.Page, request.PageSize, filter);

        return historyImportFiles;
    }
}