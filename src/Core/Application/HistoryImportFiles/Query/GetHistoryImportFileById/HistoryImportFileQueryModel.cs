using System;

namespace TesteBackend.Application.HistoryImportFiles.Query.GetHistoryImportFileById;

public class HistoryImportFileQueryModel
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public string FileExtension { get; set; }
    public string FileSize { get; set; }
    public string FileMimeType { get; set; }
    public string Status { get; set; }
    public DateTime StartProessingAt { get; set; }
    public DateTime ImportedAt { get; set; }
    public bool Error { get; set; }
    public string ErrorDetail { get; set; }
}