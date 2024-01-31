using System;
using MediatR;

namespace TesteBackend.Application.HistoryImportFiles.Query.GetHistoryImportFileById;

public class GetHistoryImportFileByIdQuery: IRequest<HistoryImportFileQueryModel>
{
    public Guid HistoryImportFileId { get; set; }
}
