using System;
using MediatR;

namespace TesteBackend.Application.Movies.Command.ProcessExcelMovieFile;

public class ProcessExcelMovieFileCommand : IRequest<Guid>
{
    public string FilePath { get; }
    public string FileName { get;  }

    public ProcessExcelMovieFileCommand(string filePath, string fileName)
    {
        FilePath = filePath;
        FileName = fileName;
    }
}