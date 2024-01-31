using TesteBackend.Application.Products.Query.GetProductById;
using TesteBackend.Domain.Entities.Products;
using TesteBackend.Persistence.Db;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TesteBackend.Persistence.QueryHandlers.Products
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductQueryModel>
    {
        private readonly CleanArchReadOnlyDbContext _dbContext;

        public GetProductByIdQueryHandler(CleanArchReadOnlyDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<ProductQueryModel> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var existingProduct = await _dbContext.Set<Product>().Where(a => a.Id == request.ProductId).Select(a =>
               new ProductQueryModel
               {
                   Name = a.Name,
                   Price = a.Price
               }).FirstOrDefaultAsync(cancellationToken: cancellationToken);

            return existingProduct;
        }
    }
}