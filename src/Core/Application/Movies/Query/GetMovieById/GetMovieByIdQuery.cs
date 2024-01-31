using System;
using MediatR;

namespace TesteBackend.Application.Movies.Query.GetMovieById;

public class GetMovieByIdQuery : IRequest<MovieQueryModel>
{
    public Guid MovieId { get; set; }
}
