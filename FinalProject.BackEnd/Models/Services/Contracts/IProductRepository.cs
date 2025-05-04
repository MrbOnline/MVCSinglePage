
using FinalProject.BackEnd.Models.DomainModels.ProductAggregates;

namespace FinalProject.BackEnd.Models.Services.Contracts
{
    public interface IProductRepository : IRepository<Product, IEnumerable<Product>>
    {
    }
}
