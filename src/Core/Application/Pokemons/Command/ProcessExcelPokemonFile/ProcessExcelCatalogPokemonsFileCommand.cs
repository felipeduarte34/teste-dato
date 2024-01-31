using System;
using MediatR;

namespace TesteBackend.Application.Pokemons.Command.ProcessExcelPokemonFile;

public class ProcessExcelCatalogPokemonsFileCommand : IRequest<Guid>
{
    public string FilePath { get; }
    public string FileName { get; }

    public ProcessExcelCatalogPokemonsFileCommand(string filePath, string fileName)
    {
        FilePath = filePath;
        FileName = fileName;
    }
}