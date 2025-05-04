using FinalProject.BackEnd.Frameworks;
using FinalProject.BackEnd.Frameworks.ResponseFrameworks;
using FinalProject.BackEnd.Frameworks.ResponseFrameworks.Contracts;
using FinalProject.BackEnd.Models.DomainModels.PersonAggregates;
using FinalProject.BackEnd.Models.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace FinalProject.BackEnd.Models.Services.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly ProjectDbContext _projectDbContext;

        #region [- ctor -]
        public PersonRepository(ProjectDbContext projectDbContext)
        {
            _projectDbContext = projectDbContext;
        }
        #endregion

        #region [- SelectAll() -]
        public async Task<IResponse<IEnumerable<Person>>> SelectAll()
        {
            try
            {
                var persons = await _projectDbContext.Person.AsNoTracking().ToListAsync();
                return persons is null ?
                    new Response<IEnumerable<Person>>(false, HttpStatusCode.UnprocessableContent, ResponseMessages.NullInput, null) :
                    new Response<IEnumerable<Person>>(true, HttpStatusCode.OK, ResponseMessages.SuccessfullOperation, persons);
            }
            catch (Exception )
            {
                throw;
 
            }

        }
        #endregion

        #region [- Select() -]
        public async Task<IResponse<Person>> Select(Person person)
        {
            try
            {
                var responseValue = new Person();
                if (person.Id.ToString() == "")
                {
                    //responseValue = await _projectDbContext.Person.FindAsync(person.Email);
                    responseValue = await _projectDbContext.Person.Where(c => c.Email == person.Email).SingleOrDefaultAsync();
                }
                else
                {
                    responseValue = await _projectDbContext.Person.FindAsync(person.Id);
                }
                return responseValue is null ?
                     new Response<Person>(false, HttpStatusCode.UnprocessableContent, ResponseMessages.NullInput, null) :
                     new Response<Person>(true, HttpStatusCode.OK, ResponseMessages.SuccessfullOperation, responseValue);
            }
            catch (Exception ex)
            {
              
                return new Response<Person>(false, HttpStatusCode.InternalServerError, ex.Message, null);
            }

        }
        #endregion

        #region [- Insert() -]
        public async Task<IResponse<Person>> Insert(Person model)
        {
            try
            {
                if (model is null)
                {
                    return new Response<Person>(false, HttpStatusCode.UnprocessableContent, ResponseMessages.NullInput, null);
                }
                await _projectDbContext.AddAsync(model);
                await _projectDbContext.SaveChangesAsync();
                var response = new Response<Person>(true, HttpStatusCode.OK, ResponseMessages.SuccessfullOperation, model);
                return response;
            }
            catch (Exception ex)
            {
               
                return new Response<Person>(false, HttpStatusCode.InternalServerError, ex.Message, null);
            }

        }
        #endregion

        #region [- Update() -]
        public async Task<IResponse<Person>> Update(Person model)
        {
            try
            {
                if (model is null)
                {
                    return new Response<Person>(false, HttpStatusCode.UnprocessableContent, ResponseMessages.NullInput, null);
                }
                //_projectDbContext.Update(model);
                _projectDbContext.Entry(model).State = EntityState.Modified;
                await _projectDbContext.SaveChangesAsync();
                var response = new Response<Person>(true, HttpStatusCode.OK, ResponseMessages.SuccessfullOperation, model);
                return response;
            }
            catch (Exception ex)
            {
                
                return new Response<Person>(false, HttpStatusCode.InternalServerError, ex.Message, null);
            }

        }
        #endregion

        #region [- Delete() -]
        public async Task<IResponse<Person>> Delete(Person model)
        {
            try
            {
                if (model is null)
                {
                    return new Response<Person>(false, HttpStatusCode.UnprocessableContent, ResponseMessages.NullInput, null);
                }

                _projectDbContext.Person.Remove(model);
                await _projectDbContext.SaveChangesAsync();
                var response = new Response<Person>(true, HttpStatusCode.OK, ResponseMessages.SuccessfullOperation, model);
                return response;
            }
            catch (Exception ex)
            {
               
                return new Response<Person>(false, HttpStatusCode.InternalServerError, ex.Message, null);
            }

        }
        #endregion

    }
}

