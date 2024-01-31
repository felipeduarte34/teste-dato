using System;
using MediatR;

namespace TesteBackend.Application.Movies.Query.ReadMovieFromRedis;

public class ReadMovieFromRedisQuery : IRequest<ReadMovieFromRedisResponse>
{
    public ReadMovieFromRedisQuery(Guid movieId)
    {
        MovieId = movieId;
    }

    public Guid MovieId { get; private set; }
}