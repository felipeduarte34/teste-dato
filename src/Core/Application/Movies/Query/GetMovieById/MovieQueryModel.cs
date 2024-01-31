using System;

namespace TesteBackend.Application.Movies.Query.GetMovieById;

public class MovieQueryModel
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Director { get; set; }
    public int Runtime { get; set; }
    public int Year { get; set; }
    
    public string MainActor { get; set; }
    public string Actors { get; set; }
    public string Genre { get; set; }
    public string Rated { get; set; }
    public int ProductionBudget { get; set; }
    public decimal WorldwideGross { get; set; }
    public string Synopsis { get; set; }
    public string Awards { get; set; }
    public string Soundtrack { get; set; }
    public string ProductionStudio { get; set; }
    public string Writers { get; set; }
    public string MarvelChronology { get; set; }
    public string SpecialParticipations { get; set; }
    public string Reception { get; set; }
    public string Curiosities { get; set; }
}