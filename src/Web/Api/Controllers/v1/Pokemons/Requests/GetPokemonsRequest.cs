namespace TesteBackend.Api.Controllers.v1.Pokemons.Requests;

public class GetPokemonsRequest
{
    public int Page { get; set; }

    public int PageSize { get; set; }
    
    public string? Name { get; set; }
    
    public int? MinHP { get; set; }
    
    public int? MaxHP { get; set; }
    
    public int? Speed { get; set; }
}