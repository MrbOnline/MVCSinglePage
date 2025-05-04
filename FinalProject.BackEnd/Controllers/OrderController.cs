using FinalProject.BackEnd.ApplicationServices.Contracts;
using FinalProject.BackEnd.ApplicationServices.Dtos.OrderDtos;
using FinalProject.BackEnd.ApplicationServices.Dtos.OrderDtos.OrderHeaderDtos;
using FinalProject.BackEnd.ApplicationServices.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FinalProject.BackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderController> _logger;

        #region [- ctor -]
        public OrderController(IOrderService orderHeaderService, ILogger<OrderController> logger)
        {
            _orderService = orderHeaderService;
            _logger = logger;
        }
        #endregion

        #region [- GetAll() -]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            Guard_OrderHeaderService();
            var getAllResponse = await _orderService.GetAll();
            var response = getAllResponse.Value.GetOrderHeaderServiceDtos;
            return new JsonResult(response);
        }
        #endregion

        #region [- Get() -]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            Guard_OrderHeaderService();
            var dto = new GetOrderHeaderServiceDto() { Id = id };
            var getResponse = await _orderService.Get(dto);
            var response = getResponse.Value;
            if (response is null)
            {
                return new JsonResult("NotFound");
            }
            return new JsonResult(response);
        }
        #endregion

        #region [- Post() -]
        [HttpPost(Name = "PostOrderHeader")]
        public async Task<IActionResult> Post([FromBody] PostOrderHeaderServiceDto dto)
        {
            Guard_OrderHeaderService();
            var postDto = new GetOrderHeaderServiceDto() { SellerId = dto.SellerId, BuyerId = dto.BuyerId };
            var getResponse = await _orderService.Get(postDto);

            switch (ModelState.IsValid)
            {
                case true when getResponse.Value is null:
                    {
                        var postResponse = await _orderService.Post(dto);
                        return postResponse.IsSuccessful ? Ok() : BadRequest();
                    }
                case true when getResponse.Value is not null:
                    return Conflict(dto);
                default:
                    return BadRequest();
            }
        }
        #endregion

        #region [- Put() -]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put([FromBody] PutOrderHeaderServiceDto dto)
        {
            Guard_OrderHeaderService();

            //Pay attention to duplication issues in the app.

            var putDto = new GetOrderHeaderServiceDto() { Id = dto.Id, SellerId = dto.SellerId, BuyerId = dto.BuyerId };

            #region [- For checking & avoiding duplication -]
            //var getResponse = await _orderHeaderService.Get(putDto);//For checking & avoiding duplication
            //switch (ModelState.IsValid)
            //{
            //    case true when getResponse.Value is null:
            //    {
            //        var putResponse = await _orderHeaderService.Put(dto);
            //        return putResponse.IsSuccessful ? Ok() : BadRequest();
            //    }
            //    case true when getResponse.Value is not null://For checking & avoiding duplication
            //        return Conflict(dto);
            //    default:
            //        return BadRequest();
            //} 
            #endregion

            if (ModelState.IsValid)
            {
                var putResponse = await _orderService.Put(dto);
                return putResponse.IsSuccessful ? Ok() : BadRequest();
            }
            else
            {
                return BadRequest();
            }
        }
        #endregion

        #region [- Delete() -]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromBody] DeleteOrderHeaderServiceDto dto)
        {
            Guard_OrderHeaderService();
            var deleteResponse = await _orderService.Delete(dto);
            return deleteResponse.IsSuccessful ? Ok() : BadRequest();
        }
        #endregion

        #region [- OrderHeaderServiceGuard() -]
        private ObjectResult Guard_OrderHeaderService()
        {
            if (_orderService is null)
            {
                return Problem($" {nameof(_orderService)} is null.");
            }

            return null;
        }
        #endregion
    }
}


