using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using TesteBackend.Common.Exceptions;
using TesteBackend.Domain.Entities.HistoryImportFiles;
using TesteBackend.Domain.Entities.Movies;
using TesteBackend.Domain.Entities.Pokemons;
using TesteBackend.Domain.Entities.UsersPokemons;
using TesteBackend.Persistence.Db;

public class ProcessExcelFileBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private string _filePath;
    private string _fileType;
    private int _userId;

    public ProcessExcelFileBackgroundService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void SetFilePathAndType(string filePath, string fileType)
    {
        _filePath = filePath;
        _fileType = fileType;
    }
    
    public void SetUserId(int userId)
    {
        _userId = userId;
    }
    
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await LongRunningOperation(cancellationToken);
    }
    
    private async Task LongRunningOperation(CancellationToken cancellationToken)
    {
        // Simula uma operação de longa duração
        // Processa o arquivo Excel e atualiza o banco de dados
         using (var scope = _serviceProvider.CreateScope())
        {
            var mongoDbContext = scope.ServiceProvider.GetService<MongoDbContext>();
            var dbContext = scope.ServiceProvider.GetService<AppDbContext>();
            
            using (var fs = new FileStream(_filePath, FileMode.Open, FileAccess.Read))
            {
                // Define um filtro para o buscar Historico
                var historySort = Builders<HistoryImportFile>.Sort.Descending(m => m.CreatedAt);
                var historyFilter = Builders<HistoryImportFile>.Filter.Eq(m => m.FilePath, _filePath);
                var histories = await mongoDbContext.GetCollection<HistoryImportFile>("HistoryImportFiles")
                    .Find(historyFilter)
                    .Sort(historySort)
                    .ToListAsync();
           
                if (!histories.Any())
                {
                    throw new ExistingRecordException("No History found for this file");
                }
                
                var error = false;
                var errorDetail = "";
                
                // Obtém o primeiro histórico da lista ordenada
                var firstHistory = histories.First();

                // Define um filtro para localizar o primeiro histórico
                var historyFilterUpdate = Builders<HistoryImportFile>.Filter.Eq(m => m.Id, firstHistory.Id);

                // Define uma atualização para alterar o campo que você deseja atualizar
                var historyUpdate = Builders<HistoryImportFile>.Update
                    .Set(m => m.StartProessingAt, DateTime.UtcNow)
                    .Set(m => m.Status, "PROCESSING")
                    .Set(m => m.FileExtension, Path.GetExtension(_filePath))
                    .Set(m => m.FileSize, fs.Length.ToString())
                    .Set(m => m.FileMimeType, fs.GetType().ToString());

                // Atualiza o histórico no MongoDB
                await mongoDbContext.GetCollection<HistoryImportFile>("HistoryImportFiles")
                    .UpdateOneAsync(historyFilterUpdate, historyUpdate);
                
                IWorkbook workbook = new XSSFWorkbook(fs);
                ISheet sheet = workbook.GetSheetAt(0);

                switch (_fileType)
                {
                    case "Movies":
                        //var resultMovies = await ProcessFileMovies(sheet, mongoDbContext);
                        var resultMovies = await ProcessFileMoviesV2(workbook.GetSheetAt(1), mongoDbContext);
                        
                        error = resultMovies.Item1;
                        errorDetail = resultMovies.Item2;
                        break;
                    case "Pokemons":
                        var resultPokemons = await ProcessFilePokemons(sheet, dbContext, _userId);
                        error = resultPokemons.Item1;
                        errorDetail = resultPokemons.Item2;
                        break;
                    case "CatalogPokemons":
                        var resultCatalogoPokemons = await ProcessFileCatalogoPokemons(sheet, dbContext);
                        error = resultCatalogoPokemons.Item1;
                        errorDetail = resultCatalogoPokemons.Item2;
                        break;
                }
                
                var historyUpdateFinish = Builders<HistoryImportFile>.Update
                    .Set(m => m.ImportedAt, DateTime.UtcNow)
                    .Set(m => m.Status, "FINISHED")
                    .Set(m => m.Error, error)
                    .Set(m => m.ErrorDetail, errorDetail);
                
                // Atualiza o primeiro histórico no MongoDB
                await mongoDbContext.GetCollection<HistoryImportFile>("HistoryImportFiles")
                    .UpdateOneAsync(historyFilterUpdate, historyUpdateFinish);
            }
        }
    }
    
    private async Task<Tuple<bool, string>> ProcessFileMovies(ISheet sheet, MongoDbContext mongoDbContext)
    {
        try
        {
            for (int row = 1; row <= sheet.LastRowNum; row++)
            {
                // Lê os valores das células na linha
                var title = sheet.GetRow(row).GetCell(0)?.ToString() ?? "";
                var director = sheet.GetRow(row).GetCell(1)?.ToString() ?? "";
                var runtime = sheet.GetRow(row).GetCell(2)?.ToString() ?? "";
                var year = sheet.GetRow(row).GetCell(3)?.ToString() ?? "";
                var mainActor = sheet.GetRow(row).GetCell(4)?.ToString() ?? "";
                var actors = sheet.GetRow(row).GetCell(5) + ";" + sheet.GetRow(row).GetCell(6) ?? "";
                var genre = sheet.GetRow(row).GetCell(7)?.ToString() ?? "";
                var rated = sheet.GetRow(row).GetCell(8)?.ToString() ?? "";
                var productionBudget = sheet.GetRow(row).GetCell(9)?.ToString() ?? "";
                var worldwideGross = sheet.GetRow(row).GetCell(10)?.ToString() ?? "";
                var synopsis = sheet.GetRow(row).GetCell(11)?.ToString() ?? "";
                var awards = sheet.GetRow(row).GetCell(12)?.ToString() ?? "";
                //var soundtrack = sheet.GetRow(row).GetCell(13)?.ToString() ?? "";
                var productionStudio = sheet.GetRow(row).GetCell(14)?.ToString() ?? "";
                var writers = sheet.GetRow(row).GetCell(13)?.ToString() ?? "";
                // var marvelChronology = sheet.GetRow(row).GetCell(14)?.ToString() ?? "";
                var specialParticipations = sheet.GetRow(row).GetCell(15)?.ToString() ?? "";
                var reception = sheet.GetRow(row).GetCell(16)?.ToString() ?? "";
                var curiosities = sheet.GetRow(row).GetCell(17)?.ToString() ?? "";

                decimal valueWorldWideGross;
                Decimal.TryParse(worldwideGross, NumberStyles.Any, CultureInfo.InvariantCulture,
                    out valueWorldWideGross);

                // Cria um novo Movie
                var movie = new Movie
                {
                    Title = title,
                    Director = director,
                    Runtime = Convert.ToInt32(runtime),
                    Year = Convert.ToInt32(year),
                    MainActor = mainActor,
                    Actors = actors,
                    Genre = genre,
                    Rated = rated,
                    ProductionBudget = !productionBudget.IsNullOrEmpty() ? Convert.ToInt32(productionBudget) : 0,
                    WorldwideGross = valueWorldWideGross,
                    Synopsis = synopsis,
                    Awards = awards,
                    Soundtrack = "",
                    ProductionStudio = productionStudio,
                    Writers = writers,
                    MarvelChronology = "",
                    SpecialParticipations = specialParticipations,
                    Reception = reception,
                    Curiosities = curiosities
                };

                // Define um filtro para o upsert
                var filter = Builders<Movie>.Filter.Eq(m => m.Title, title) &
                             Builders<Movie>.Filter.Eq(m => m.Director, director) &
                             Builders<Movie>.Filter.Eq(m => m.Year, Convert.ToInt32(year));

                var movies = await mongoDbContext.GetCollection<Movie>("Movies")
                    .FindAsync(filter);
                var existingMovie = await movies.FirstOrDefaultAsync();

                movie.Id = existingMovie?.Id ?? Guid.NewGuid();

                // Realiza o upsert no MongoDB
                await mongoDbContext.GetCollection<Movie>("Movies")
                    .ReplaceOneAsync(filter, movie, new ReplaceOptions { IsUpsert = true });
            }
            return new Tuple<bool, string>(false, "");
        }
        catch (Exception ex)
        {
            return new Tuple<bool, string>(true, ex.Message);
        }
    }
    private async Task<Tuple<bool, string>> ProcessFileMoviesV2(ISheet sheet, MongoDbContext mongoDbContext)
    {
        try
        {
            // Obtenha a primeira linha para usar como identificadores de coluna
            var headerRow = sheet.GetRow(0);
            var columnIdentifiers = new Dictionary<string, int>();

            for (int column = 0; column < headerRow.LastCellNum; column++)
            {
                var columnName = headerRow.GetCell(column)?.ToString();
                if (!string.IsNullOrEmpty(columnName))
                {
                    columnIdentifiers[columnName] = column;
                }
            }

            for (int row = 1; row <= sheet.LastRowNum; row++)
            {
                // Cria um novo Movie
                var movie = new Movie();

                // Lê os valores das células na linha
                var dataRow = sheet.GetRow(row);
                foreach (var columnIdentifier in columnIdentifiers)
                {
                    var cellValue = dataRow.GetCell(columnIdentifier.Value)?.ToString() ?? "";

                    switch (columnIdentifier.Key)
                    {
                        case "Title":
                            movie.Title = cellValue;
                            break;
                        case "Director":
                            movie.Director = cellValue;
                            break;
                        case "Runtime":
                            movie.Runtime = Convert.ToInt32(cellValue);
                            break;
                        case "Year":
                            movie.Year = Convert.ToInt32(cellValue);
                            break;
                        case "MainActor":
                            movie.MainActor = cellValue;
                            break;
                        case "Actors":
                            movie.Actors = cellValue;
                            break;
                        case "Genre":
                            movie.Genre = cellValue;
                            break;
                        case "Rated":
                            movie.Rated = cellValue;
                            break;
                        case "ProductionBudget":
                            movie.ProductionBudget = !cellValue.IsNullOrEmpty() ? Convert.ToInt32(cellValue) : 0;
                            break;
                        case "WorldwideGross":
                            decimal valueWorldWideGross;
                            Decimal.TryParse(cellValue, NumberStyles.Any, CultureInfo.InvariantCulture, out valueWorldWideGross);
                            movie.WorldwideGross = valueWorldWideGross;
                            break;
                        case "Synopsis":
                            movie.Synopsis = cellValue;
                            break;
                        case "Awards":
                            movie.Awards = cellValue;
                            break;
                        case "Soundtrack":
                            movie.Soundtrack = cellValue;
                            break;
                        case "ProductionStudio":
                            movie.ProductionStudio = cellValue;
                            break;
                        case "Writers":
                            movie.Writers = cellValue;
                            break;
                        case "MarvelChronology":
                            movie.MarvelChronology = cellValue;
                            break;
                        case "SpecialParticipations":
                            movie.SpecialParticipations = cellValue;
                            break;
                        case "Reception":
                            movie.Reception = cellValue;
                            break;
                        case "Curiosities":
                            movie.Curiosities = cellValue;
                            break;
                    }
                }

                // Realiza o upsert no MongoDB
                var filter = Builders<Movie>.Filter.Eq(m => m.Title, movie.Title) &
                             Builders<Movie>.Filter.Eq(m => m.Director, movie.Director) &
                             Builders<Movie>.Filter.Eq(m => m.Year, movie.Year);

                var movies = await mongoDbContext.GetCollection<Movie>("Movies")
                    .FindAsync(filter);
                var existingMovie = await movies.FirstOrDefaultAsync();

                movie.Id = existingMovie?.Id ?? Guid.NewGuid();

                await mongoDbContext.GetCollection<Movie>("Movies")
                    .ReplaceOneAsync(filter, movie, new ReplaceOptions { IsUpsert = true });
            }
            return new Tuple<bool, string>(false, "");
        }
        catch (Exception ex)
        {
            return new Tuple<bool, string>(true, ex.Message);
        }
    }
    private async Task<Tuple<bool, string>> ProcessFilePokemons(ISheet sheet, AppDbContext dbContext, int userId)
    {
        try
        {
            for (int row = 1; row <= sheet.LastRowNum; row++)
            {
                // Lê os valores das células na linha
                var id = sheet.GetRow(row).GetCell(0)?.ToString() ?? "";
                
                // Busca a relação na tabela UserPokemons
                var existingUserPokemon = await dbContext.Set<UserPokemon>()
                    .FirstOrDefaultAsync(up => up.UserId == userId && up.PokemonId == Convert.ToInt32(id));

                if (existingUserPokemon != null)
                {
                    continue;
                }
                
                dbContext.Set<UserPokemon>().Add(new UserPokemon
                {
                    UserId = userId,
                    PokemonId = Convert.ToInt32(id)
                });
                
                // Salva as alterações no banco de dados
                await dbContext.SaveChangesAsync();

            }
            return new Tuple<bool, string>(false, "");
        }
        catch (Exception ex)
        {
            return new Tuple<bool, string>(true, ex.Message);
        }
    }
    private async Task<Tuple<bool, string>> ProcessFileCatalogoPokemons(ISheet sheet, AppDbContext dbContext)
    {
        try
        {
            for (int row = 1; row <= sheet.LastRowNum; row++)
            {
                // Lê os valores das células na linha
                var name = sheet.GetRow(row).GetCell(1)?.ToString() ?? "";
                var hp = sheet.GetRow(row).GetCell(2)?.ToString() ?? "";
                var attack = sheet.GetRow(row).GetCell(3)?.ToString() ?? "";
                var defense = sheet.GetRow(row).GetCell(4)?.ToString() ?? "";
                var specialAttack = sheet.GetRow(row).GetCell(5)?.ToString() ?? "";
                var specialDefense = sheet.GetRow(row).GetCell(6)?.ToString() ?? "";
                var speed = sheet.GetRow(row).GetCell(7)?.ToString() ?? "";
                
                // Cria um novo Pokemon
                var pokemon = new Pokemon
                {
                   Name = name,
                   Hp = Convert.ToInt32(hp),
                   Attack = Convert.ToInt32(attack),
                   Defense = Convert.ToInt32(defense),
                   SpecialAttack = Convert.ToInt32(specialAttack),
                   SpecialDefense = Convert.ToInt32(specialDefense),
                   Speed = Convert.ToInt32(speed)
                };
                
                // Busca o pokemon no banco de dados
                var existingPokemon = await dbContext.Set<Pokemon>()
                    .FirstOrDefaultAsync(p => p.Id == pokemon.Id);

                if (existingPokemon != null)
                {
                    // Se o pokemon existir, atualiza os dados
                    existingPokemon.Name = pokemon.Name;
                    existingPokemon.Hp = pokemon.Hp;
                    existingPokemon.Attack = pokemon.Attack;
                    existingPokemon.Defense = pokemon.Defense;
                    existingPokemon.SpecialAttack = pokemon.SpecialAttack;
                    existingPokemon.SpecialDefense = pokemon.SpecialDefense;
                    existingPokemon.Speed = pokemon.Speed;
                    dbContext.Set<Pokemon>().Update(existingPokemon);
                }
                else
                {
                    // Se o pokemon não existir, adiciona um novo
                    dbContext.Set<Pokemon>().Add(pokemon);
                }
                
                // Salva as alterações no banco de dados
                await dbContext.SaveChangesAsync();
            }
            
            // Turn off IDENTITY_INSERT for the Pokemon table
            await dbContext.SaveChangesAsync();
            
            return new Tuple<bool, string>(false, "");
        }
        catch (Exception ex)
        {
            return new Tuple<bool, string>(true, ex.Message);
        }
    }
}