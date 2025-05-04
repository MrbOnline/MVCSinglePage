
using Microsoft.EntityFrameworkCore;
using System.Net;
using FinalProject.BackEnd.Models.Services.Contracts;
using FinalProject.BackEnd.Models.DomainModels.ProductAggregates;
using FinalProject.BackEnd.Frameworks.ResponseFrameworks.Contracts;
using FinalProject.BackEnd.Frameworks;
using FinalProject.BackEnd.Frameworks.ResponseFrameworks;

namespace FinalProject.BackEnd.Models.Services.Repositories
{
    public class ProductRepository: IProductRepository
    {
        private readonly ProjectDbContext _projectDbContext;

        #region [- Ctor -]
        public ProductRepository(ProjectDbContext projectDbContext)
        {
            _projectDbContext = projectDbContext;
        } 
        #endregion

        #region [- SelectAll() -]
        public async Task<IResponse<IEnumerable<Product>>> SelectAll()
        {
            try
            {
                var products = await _projectDbContext.Product.AsNoTracking().ToListAsync();
                return products is null ?
                    new Response<IEnumerable<Product>>(false, HttpStatusCode.UnprocessableContent, ResponseMessages.NullInput, null) :
                    new Response<IEnumerable<Product>>(true, HttpStatusCode.OK, ResponseMessages.SuccessfullOperation, products);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region [- Select() -]
        public async Task<IResponse<Product>> Select(Product product)
        {
            try
            {
                var responseValue = new Product();
                
                   
             
                
                
                    responseValue = await _projectDbContext.Product.FindAsync(product.Id);
                
                return responseValue is null ?
                     new Response<Product>(false, HttpStatusCode.UnprocessableContent, ResponseMessages.NullInput, null) :
                     new Response<Product>(true, HttpStatusCode.OK, ResponseMessages.SuccessfullOperation, responseValue);
            }
            catch (Exception ex ) 
            {
                return new Response<Product>(false, HttpStatusCode.InternalServerError, ex.Message, null);
            }
        }
        #endregion

        #region [- Insert() -]
        public async Task<IResponse<Product>> Insert(Product model)
        {
            try
            {
                if (model is null)
                {
                    return new Response<Product>(false, HttpStatusCode.UnprocessableContent, ResponseMessages.NullInput, null);
                }
                await _projectDbContext.AddAsync(model);
                await _projectDbContext.SaveChangesAsync();
                var response = new Response<Product>(true, HttpStatusCode.OK, ResponseMessages.SuccessfullOperation, model);
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region [- Update() -]
        public async Task<IResponse<Product>> Update(Product model)
        {
            try
            {
                if (model is null)
                {
                    return new Response<Product>(false, HttpStatusCode.UnprocessableContent, ResponseMessages.NullInput, null);
                }
                //_projectDbContext.Update(model);
                _projectDbContext.Entry(model).State = EntityState.Modified;
                await _projectDbContext.SaveChangesAsync();
                var response = new Response<Product>(true, HttpStatusCode.OK, ResponseMessages.SuccessfullOperation, model);
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region [- Delete() -]
        public async Task<IResponse<Product>> Delete(Product model)
        {
            try
            {
                if (model is null)
                {
                    return new Response<Product>(false, HttpStatusCode.UnprocessableContent, ResponseMessages.NullInput, null);
                }

                _projectDbContext.Product.Remove(model);
                await _projectDbContext.SaveChangesAsync();
                var response = new Response<Product>(true, HttpStatusCode.OK, ResponseMessages.SuccessfullOperation, model);
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}


