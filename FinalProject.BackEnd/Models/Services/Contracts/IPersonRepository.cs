using FinalProject.BackEnd.Models.DomainModels.PersonAggregates;

namespace FinalProject.BackEnd.Models.Services.Contracts
{
    public interface IPersonRepository : IRepository<Person, IEnumerable<Person>>
    {
    }
}
