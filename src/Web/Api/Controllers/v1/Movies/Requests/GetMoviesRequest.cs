namespace TesteBackend.Api.Controllers.v1.Movies.Requests;

public class GetMoviesRequest
{
    public int Page { get; set; }

    public int PageSize { get; set; }
    
    public string? GenericSearch { get; set; }
}