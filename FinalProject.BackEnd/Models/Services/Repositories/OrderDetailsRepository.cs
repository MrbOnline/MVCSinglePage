using FinalProject.BackEnd.Frameworks;
using FinalProject.BackEnd.Frameworks.ResponseFrameworks;
using FinalProject.BackEnd.Frameworks.ResponseFrameworks.Contracts;
using FinalProject.BackEnd.Models.DomainModels.OrderAggregates;
using FinalProject.BackEnd.Models.DomainModels.OrderAggregates.OrderDetailsAggregates;
using FinalProject.BackEnd.Models.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace FinalProject.BackEnd.Models.Services.Repositories
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly ProjectDbContext _projectDbContext;

        public OrderDetailRepository(ProjectDbContext projectDbContext)
        {
            _projectDbContext = projectDbContext;
        }

        public async Task<IResponse<IEnumerable<OrderDetail>>> SelectAll()
        {
            try
            {
                var orderDetails = await _projectDbContext.OrderDetails.AsNoTracking().ToListAsync();
                return orderDetails is null ?
                    new Response<IEnumerable<OrderDetail>>(false, HttpStatusCode.UnprocessableContent, ResponseMessages.NullInput, null) :
                    new Response<IEnumerable<OrderDetail>>(true, HttpStatusCode.OK, ResponseMessages.SuccessfullOperation, orderDetails);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IResponse<OrderDetail>> Select(OrderDetail orderDetail)
        {
            try
            {
                var responseValue = await _projectDbContext.OrderDetails.FindAsync(orderDetail.OrderHeaderId, orderDetail.ProductId);
                return responseValue is null ?
                    new Response<OrderDetail>(false, HttpStatusCode.UnprocessableContent, ResponseMessages.NullInput, null) :
                    new Response<OrderDetail>(true, HttpStatusCode.OK, ResponseMessages.SuccessfullOperation, responseValue);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IResponse<OrderDetail>> Insert(OrderDetail model)
        {
            try
            {
                if (model is null)
                {
                    return new Response<OrderDetail>(false, HttpStatusCode.UnprocessableContent, ResponseMessages.NullInput, null);
                }
                await _projectDbContext.AddAsync(model);
                await _projectDbContext.SaveChangesAsync();
                return new Response<OrderDetail>(true, HttpStatusCode.OK, ResponseMessages.SuccessfullOperation, model);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IResponse<OrderDetail>> Update(OrderDetail model)
        {
            try
            {
                if (model is null)
                {
                    return new Response<OrderDetail>(false, HttpStatusCode.UnprocessableContent, ResponseMessages.NullInput, null);
                }
                _projectDbContext.Entry(model).State = EntityState.Modified;
                await _projectDbContext.SaveChangesAsync();
                return new Response<OrderDetail>(true, HttpStatusCode.OK, ResponseMessages.SuccessfullOperation, model);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IResponse<OrderDetail>> Delete(OrderDetail model)
        {
            try
            {
                if (model is null)
                {
                    return new Response<OrderDetail>(false, HttpStatusCode.UnprocessableContent, ResponseMessages.NullInput, null);
                }
                _projectDbContext.OrderDetails.Remove(model);
                await _projectDbContext.SaveChangesAsync();
                return new Response<OrderDetail>(true, HttpStatusCode.OK, ResponseMessages.SuccessfullOperation, model);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
