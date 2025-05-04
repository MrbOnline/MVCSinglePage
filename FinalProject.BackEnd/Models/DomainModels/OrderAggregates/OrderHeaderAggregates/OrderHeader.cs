using FinalProject.BackEnd.Models.DomainModels.PersonAggregates;
using FinalProject.BackEnd.Models.DomainModels.OrderAggregates.OrderDetailsAggregates;

namespace FinalProject.BackEnd.Models.DomainModels.OrderAggregates.OrderHeaderAggregates
{
    public class OrderHeader
    {
        public Guid? Id { get; set; }
        public Guid? SellerId { get; set; }
        public Guid? BuyerId { get; set; }
        public Person? Seller { get; set; }
        public Person? Buyer { get; set; }

        public ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
