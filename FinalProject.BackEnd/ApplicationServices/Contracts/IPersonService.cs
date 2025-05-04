using FinalProject.BackEnd.ApplicationServices.Dtos.PersonDtos;

namespace FinalProject.BackEnd.ApplicationServices.Contracts
{
    public interface IPersonService:IService<PostPersonServiceDto,GetPersonServiceDto,GetAllPersonServiceDto,PutPersonServiceDto,DeletePersonServiceDto>
    {
    }
}
