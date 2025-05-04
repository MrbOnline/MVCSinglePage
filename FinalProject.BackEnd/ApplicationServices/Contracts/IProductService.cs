
using FinalProject.BackEnd.ApplicationServices.Dtos.ProductDtos;
using SimpleWebApiFullCrud.Dtos;

namespace FinalProject.BackEnd.ApplicationServices.Contracts
{
    public interface IProductService:IService<PostProductServiceDto,GetProductServiceDto,GetAllProductServiceDto,PutProductServiceDto,DeleteProductServiceDto>
    {
    }
}
