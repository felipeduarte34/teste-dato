using MediatR;

namespace TesteBackend.Application.Products.Query.GetProductById
{
    public class GetProductByIdQuery : IRequest<ProductQueryModel>
    {
        public int ProductId { get; set; }
    }
}