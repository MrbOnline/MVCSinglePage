using FinalProject.BackEnd.Frameworks;
using FinalProject.BackEnd.Frameworks.ResponseFrameworks;
using FinalProject.BackEnd.Frameworks.ResponseFrameworks.Contracts;
using FinalProject.BackEnd.Models.DomainModels.OrderAggregates;
using FinalProject.BackEnd.Models.DomainModels.OrderAggregates.OrderHeaderAggregates;
using FinalProject.BackEnd.Models.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace FinalProject.BackEnd.Models.Services.Repositories
{
    public class OrderHeaderRepository : IOrderHeaderRepository
    {
        private readonly ProjectDbContext _projectDbContext;

        public OrderHeaderRepository(ProjectDbContext projectDbContext)
        {
            _projectDbContext = projectDbContext;
        }

        public async Task<IResponse<IEnumerable<OrderHeader>>> SelectAll()
        {
            try
            {
                var orderHeaders = await _projectDbContext.OrderHeaders.AsNoTracking().ToListAsync();
                return orderHeaders is null ?
                    new Response<IEnumerable<OrderHeader>>(false, HttpStatusCode.UnprocessableContent, ResponseMessages.NullInput, null) :
                    new Response<IEnumerable<OrderHeader>>(true, HttpStatusCode.OK, ResponseMessages.SuccessfullOperation, orderHeaders);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IResponse<OrderHeader>> Select(OrderHeader orderHeader)
        {
            try
            {
                var responseValue = await _projectDbContext.OrderHeaders.FindAsync(orderHeader.Id);
                return responseValue is null ?
                    new Response<OrderHeader>(false, HttpStatusCode.UnprocessableContent, ResponseMessages.NullInput, null) :
                    new Response<OrderHeader>(true, HttpStatusCode.OK, ResponseMessages.SuccessfullOperation, responseValue);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IResponse<OrderHeader>> Insert(OrderHeader model)
        {
            try
            {
                if (model is null)
                {
                    return new Response<OrderHeader>(false, HttpStatusCode.UnprocessableContent, ResponseMessages.NullInput, null);
                }
                await _projectDbContext.AddAsync(model);
                await _projectDbContext.SaveChangesAsync();
                return new Response<OrderHeader>(true, HttpStatusCode.OK, ResponseMessages.SuccessfullOperation, model);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IResponse<OrderHeader>> Update(OrderHeader model)
        {
            try
            {
                if (model is null)
                {
                    return new Response<OrderHeader>(false, HttpStatusCode.UnprocessableContent, ResponseMessages.NullInput, null);
                }
                _projectDbContext.Entry(model).State = EntityState.Modified;
                await _projectDbContext.SaveChangesAsync();
                return new Response<OrderHeader>(true, HttpStatusCode.OK, ResponseMessages.SuccessfullOperation, model);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IResponse<OrderHeader>> Delete(OrderHeader model)
        {
            try
            {
                if (model is null)
                {
                    return new Response<OrderHeader>(false, HttpStatusCode.UnprocessableContent, ResponseMessages.NullInput, null);
                }
                _projectDbContext.OrderHeaders.Remove(model);
                await _projectDbContext.SaveChangesAsync();
                return new Response<OrderHeader>(true, HttpStatusCode.OK, ResponseMessages.SuccessfullOperation, model);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
