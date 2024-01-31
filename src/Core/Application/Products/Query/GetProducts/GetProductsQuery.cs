using TesteBackend.Common.Utilities;
using TesteBackend.Domain.Entities.Products;
using MediatR;

namespace TesteBackend.Application.Products.Query.GetProducts
{
    public class GetProductsQuery : IRequest<PagedResult<Product>>
    {
        public int Page { get; set; }

        public int PageSize { get; set; }
    }
}
