using System;
using MediatR;

namespace TesteBackend.Application.Pokemons.Command.ProcessExcelPokemonFile;

public class ProcessExcelMyPokemonsFileCommand: IRequest<Guid>
{
    public string FilePath { get; }
    public string FileName { get; }
    
    public int UserId { get; }

    public ProcessExcelMyPokemonsFileCommand(string filePath, string fileName, int userId)
    {
        FilePath = filePath;
        FileName = fileName;
        UserId = userId;
    }
}