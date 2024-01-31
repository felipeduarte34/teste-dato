using TesteBackend.Application.Products.Query.GetProducts;
using TesteBackend.Common.Utilities;
using TesteBackend.Domain.Entities.Products;
using TesteBackend.Persistence.Db;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TesteBackend.Persistence.QueryHandlers.Products
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, PagedResult<Product>>
    {
        private readonly CleanArchReadOnlyDbContext _dbContext;

        public GetProductsQueryHandler(CleanArchReadOnlyDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<PagedResult<Product>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _dbContext.Set<Product>().GetPaged(request.Page, request.PageSize);

            return products;
        }
    }
}
