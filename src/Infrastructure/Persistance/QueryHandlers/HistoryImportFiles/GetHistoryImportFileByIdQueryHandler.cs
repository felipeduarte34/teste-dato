using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MongoDB.Driver;
using TesteBackend.Application.HistoryImportFiles.Query.GetHistoryImportFileById;
using TesteBackend.Application.Movies.Query.GetMovieById;
using TesteBackend.Domain.Entities.HistoryImportFiles;
using TesteBackend.Domain.Entities.Movies;

namespace TesteBackend.Persistence.QueryHandlers.HistoryImportFiles;

public class GetHistoryImportFileByIdQueryHandler : IRequestHandler<GetHistoryImportFileByIdQuery, HistoryImportFileQueryModel>
{
    private readonly MongoDbContext _dbContext;

    public GetHistoryImportFileByIdQueryHandler(MongoDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<HistoryImportFileQueryModel> Handle(GetHistoryImportFileByIdQuery request, CancellationToken cancellationToken)
    {
        var builder = Builders<HistoryImportFile>.Filter;
        var filter = builder.Eq(m => m.Id, request.HistoryImportFileId);

        var historyImportFiles = await _dbContext.GetCollection<HistoryImportFile>("HistoryImportFiles")
            .FindAsync(filter, cancellationToken: cancellationToken);
            
        var historyImportFile = await historyImportFiles.FirstOrDefaultAsync(cancellationToken: cancellationToken);

        return new HistoryImportFileQueryModel
        {
            Id = historyImportFile.Id,
            FilePath = historyImportFile.FilePath,
            FileName = historyImportFile.FileName,
            Status = historyImportFile.Status,
            CreatedAt = historyImportFile.CreatedAt,
            Error = historyImportFile.Error,
            ErrorDetail = historyImportFile.ErrorDetail,
            FileExtension = historyImportFile.FileExtension,
            FileMimeType = historyImportFile.FileMimeType,
            FileSize = historyImportFile.FileSize,
            ImportedAt = historyImportFile.ImportedAt,
            StartProessingAt = historyImportFile.StartProessingAt
        };
    }
}