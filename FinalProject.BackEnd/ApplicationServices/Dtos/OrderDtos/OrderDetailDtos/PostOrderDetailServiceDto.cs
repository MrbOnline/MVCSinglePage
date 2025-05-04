using FinalProject.BackEnd.Models.DomainModels.PersonAggregates;

namespace FinalProject.BackEnd.ApplicationServices.Dtos.OrderDtos.OrderDetailDtos
{
    public class PostOrderDetailServiceDto
    {
        public Guid? ProductId { get; set; }
        //public decimal UnitPrice { get; set; }
        public decimal Amount { get; set; }
       
    }
}
