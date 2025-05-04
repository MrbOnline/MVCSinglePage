

using FinalProject.BackEnd.ApplicationServices.Dtos.OrderDtos.OrderHeaderDtos;

namespace FinalProject.BackEnd.ApplicationServices.Contracts
{
    public interface IOrderService : IService<PostOrderHeaderServiceDto,GetOrderHeaderServiceDto,GetAllOrderHeaderServiceDto,PutOrderHeaderServiceDto,DeleteOrderHeaderServiceDto>
    {
    }

}
