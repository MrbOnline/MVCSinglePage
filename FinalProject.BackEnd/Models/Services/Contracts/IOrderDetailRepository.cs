using FinalProject.BackEnd.Models.DomainModels.OrderAggregates.OrderDetailsAggregates;
using FinalProject.BackEnd.Models.DomainModels.PersonAggregates;

namespace FinalProject.BackEnd.Models.Services.Contracts
{
    public interface IOrderDetailRepository : IRepository<OrderDetail, IEnumerable<OrderDetail>>
    {
    }
}
