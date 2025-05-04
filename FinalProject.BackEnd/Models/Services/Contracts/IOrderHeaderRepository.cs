using FinalProject.BackEnd.Models.DomainModels.OrderAggregates.OrderHeaderAggregates;

namespace FinalProject.BackEnd.Models.Services.Contracts
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader, IEnumerable<OrderHeader>>
    {
    }
}
