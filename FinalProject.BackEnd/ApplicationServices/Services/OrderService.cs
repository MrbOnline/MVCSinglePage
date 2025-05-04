using FinalProject.BackEnd.ApplicationServices.Contracts;
using FinalProject.BackEnd.ApplicationServices.Dtos.OrderDtos.OrderDetailDtos;
using FinalProject.BackEnd.ApplicationServices.Dtos.OrderDtos.OrderHeaderDtos;
using FinalProject.BackEnd.Frameworks;
using FinalProject.BackEnd.Frameworks.ResponseFrameworks;
using FinalProject.BackEnd.Frameworks.ResponseFrameworks.Contracts;
using FinalProject.BackEnd.Models.DomainModels.OrderAggregates.OrderDetailsAggregates;
using FinalProject.BackEnd.Models.DomainModels.OrderAggregates.OrderHeaderAggregates;
using FinalProject.BackEnd.Models.DomainModels.ProductAggregates;
using FinalProject.BackEnd.Models.Services.Contracts;
using System.Net;

namespace FinalProject.BackEnd.ApplicationServices.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderHeaderRepository _orderHeaderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IProductRepository _productRepository;

        public OrderService( IOrderHeaderRepository orderHeaderRepository,
            IOrderDetailRepository orderDetailRepository,
            IProductRepository productRepository)
        {
            _orderHeaderRepository = orderHeaderRepository;
            _orderDetailRepository = orderDetailRepository;
            _productRepository = productRepository;
        }

        public async Task<IResponse<GetAllOrderHeaderServiceDto>> GetAll()
        {
            var selectAllResponse = await _orderHeaderRepository.SelectAll();

            if (selectAllResponse is null || !selectAllResponse.IsSuccessful)
            {
                return new Response<GetAllOrderHeaderServiceDto>(false, HttpStatusCode.UnprocessableContent, ResponseMessages.Error, null);
            }

            var getAllOrderHeaderDto = new GetAllOrderHeaderServiceDto { GetOrderHeaderServiceDtos = new List<GetOrderHeaderServiceDto>() };

            foreach (var header in selectAllResponse.Value)
            {
                var orderDetails = await _orderDetailRepository.SelectAll();
                var orderDetailList = orderDetails.Value?.Where(d => d.OrderHeaderId == header.Id).ToList();

                var orderHeaderDto = new GetOrderHeaderServiceDto
                {
                    Id = header.Id,
                    SellerId = header.SellerId,
                    BuyerId = header.BuyerId,
                    OrderDetails = orderDetailList.Select(d => new GetOrderDetailServiceDto
                    {
                        Id = d.Id,
                        OrderHeaderId = d.OrderHeaderId,
                        ProductId = d.ProductId,
                        UnitPrice = d.UnitPrice,
                        Amount = d.Amount
                    }).ToList()
                };

                getAllOrderHeaderDto.GetOrderHeaderServiceDtos.Add(orderHeaderDto);
            }

            return new Response<GetAllOrderHeaderServiceDto>(true, HttpStatusCode.OK, ResponseMessages.SuccessfullOperation, getAllOrderHeaderDto);
        }

        public async Task<IResponse<GetOrderHeaderServiceDto>> Get(GetOrderHeaderServiceDto dto)
        {
            var selectResponse = await _orderHeaderRepository.Select(new OrderHeader { Id = dto.Id });

            if (selectResponse is null || !selectResponse.IsSuccessful)
            {
                return new Response<GetOrderHeaderServiceDto>(false, HttpStatusCode.UnprocessableContent, ResponseMessages.Error, null);
            }

            var orderHeader = selectResponse.Value;

            var orderDetailsResponse = await _orderDetailRepository.SelectAll();
            var orderDetails = orderDetailsResponse.Value?.Where(d => d.OrderHeaderId == orderHeader.Id).ToList();

            var orderHeaderDto = new GetOrderHeaderServiceDto
            {
                Id = orderHeader.Id,
                SellerId = orderHeader.SellerId,
                BuyerId = orderHeader.BuyerId,
                OrderDetails = orderDetails.Select(d => new GetOrderDetailServiceDto
                {
                    Id = d.Id,
                    OrderHeaderId = d.OrderHeaderId,
                    ProductId = d.ProductId,
                    UnitPrice = d.UnitPrice,
                    Amount = d.Amount
                }).ToList()
            };

            return new Response<GetOrderHeaderServiceDto>(true, HttpStatusCode.OK, ResponseMessages.SuccessfullOperation, orderHeaderDto);
        }

        public async Task<IResponse<PostOrderHeaderServiceDto>> Post(PostOrderHeaderServiceDto dto)
        {
            if (dto is null)
            {
                return new Response<PostOrderHeaderServiceDto>(false, HttpStatusCode.UnprocessableContent, ResponseMessages.NullInput, null);
            }

            var orderHeader = new OrderHeader
            {
                Id = Guid.NewGuid(),
                SellerId = dto.SellerId,
                BuyerId = dto.BuyerId
            };

            var insertResponse = await _orderHeaderRepository.Insert(orderHeader);
            if (!insertResponse.IsSuccessful)
            {
                return new Response<PostOrderHeaderServiceDto>(false, HttpStatusCode.UnprocessableContent, ResponseMessages.Error, dto);
            }

            foreach (var orderDetailDto in dto.OrderDetails)
            {
                var productResponse = await _productRepository.Select(new Product { Id = orderDetailDto.ProductId });
                if (productResponse?.Value == null)
                {
                    return new Response<PostOrderHeaderServiceDto>(false, HttpStatusCode.UnprocessableContent, "Product not found", null);
                }

                var orderDetail = new OrderDetail
                {
                    Id = Guid.NewGuid(),
                    OrderHeaderId = orderHeader.Id,
                    ProductId = orderDetailDto.ProductId,
                    UnitPrice = productResponse.Value.UnitPrice, 
                    Amount = orderDetailDto.Amount
                };

                await _orderDetailRepository.Insert(orderDetail);
            }

            return new Response<PostOrderHeaderServiceDto>(true, HttpStatusCode.OK, ResponseMessages.SuccessfullOperation, dto);
        }

        public async Task<IResponse<PutOrderHeaderServiceDto>> Put(PutOrderHeaderServiceDto dto)
        {
            if (dto is null)
            {
                return new Response<PutOrderHeaderServiceDto>(false, HttpStatusCode.UnprocessableContent, ResponseMessages.NullInput, null);
            }

            var orderHeader = new OrderHeader
            {
                Id = dto.Id,
                SellerId = dto.SellerId,
                BuyerId = dto.BuyerId
            };

            var updateResponse = await _orderHeaderRepository.Update(orderHeader);
            if (!updateResponse.IsSuccessful)
            {
                return new Response<PutOrderHeaderServiceDto>(false, HttpStatusCode.UnprocessableContent, ResponseMessages.Error, dto);
            }

            foreach (var orderDetailDto in dto.OrderDetails)
            {
                var productResponse = await _productRepository.Select(new Product { Id = orderDetailDto.ProductId });
                if (productResponse?.Value == null)
                {
                    return new Response<PutOrderHeaderServiceDto>(false, HttpStatusCode.UnprocessableContent, "Product not found", null);
                }

                var orderDetail = new OrderDetail
                {
                    Id = orderDetailDto.Id,
                    OrderHeaderId = orderHeader.Id,
                    ProductId = orderDetailDto.ProductId,
                    UnitPrice = productResponse.Value.UnitPrice, // Gereftan gheymat az Product
                    Amount = orderDetailDto.Amount
                };

                await _orderDetailRepository.Update(orderDetail);
            }

            return new Response<PutOrderHeaderServiceDto>(true, HttpStatusCode.OK, ResponseMessages.SuccessfullOperation, dto);
        }

        public async Task<IResponse<DeleteOrderHeaderServiceDto>> Delete(DeleteOrderHeaderServiceDto dto)
        {
            if (dto is null)
            {
                return new Response<DeleteOrderHeaderServiceDto>(false, HttpStatusCode.UnprocessableContent, ResponseMessages.NullInput, null);
            }

            var orderHeader = new OrderHeader { Id = dto.Id };
            var deleteResponse = await _orderHeaderRepository.Delete(orderHeader);
            if (!deleteResponse.IsSuccessful)
            {
                return new Response<DeleteOrderHeaderServiceDto>(false, HttpStatusCode.UnprocessableContent, ResponseMessages.Error, dto);
            }

            var orderDetailsResponse = await _orderDetailRepository.SelectAll();
            var orderDetails = orderDetailsResponse.Value?.Where(d => d.OrderHeaderId == dto.Id).ToList();

            foreach (var orderDetail in orderDetails)
            {
                await _orderDetailRepository.Delete(orderDetail);
            }

            return new Response<DeleteOrderHeaderServiceDto>(true, HttpStatusCode.OK, ResponseMessages.SuccessfullOperation, dto);
        }
    }
}


