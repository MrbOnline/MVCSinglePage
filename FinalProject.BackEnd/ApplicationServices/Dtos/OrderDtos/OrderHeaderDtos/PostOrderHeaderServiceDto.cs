using FinalProject.BackEnd.ApplicationServices.Dtos.OrderDtos.OrderDetailDtos;
using FinalProject.BackEnd.Models.DomainModels.PersonAggregates;

namespace FinalProject.BackEnd.ApplicationServices.Dtos.OrderDtos.OrderHeaderDtos
{
    public class PostOrderHeaderServiceDto
    {
      
        
            public Guid? SellerId { get; set; }
            public Guid? BuyerId { get; set; }
            public List<PostOrderDetailServiceDto>? OrderDetails { get; set; }
        

        //public Person Seller { get; set; }
        //public Person Buyer { get; set; }
    }
}
