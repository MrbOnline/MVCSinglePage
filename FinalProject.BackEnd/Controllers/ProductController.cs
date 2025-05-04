using FinalProject.BackEnd.ApplicationServices.Contracts;
using Microsoft.AspNetCore.Mvc;
using SimpleWebApiFullCrud.Dtos;

namespace FinalProject.BackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        #region [- ctor -] 
        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }
        #endregion

        #region [- GetAll() -]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            Guard_ProductService();
            var getAllResponse = await _productService.GetAll();
            var response = getAllResponse.Value.GetProductServiceDtos;
            return new JsonResult(response);
        }
        #endregion

        #region [- Get() -]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            Guard_ProductService();
            var dto = new GetProductServiceDto() { Id = id };
            var getResponse = await _productService.Get(dto);
            var response = getResponse.Value;
            if (response is null)
            {
                return new JsonResult("NotFound");
            }
            return new JsonResult(response);
        }
        #endregion

        #region [- Post() -]
        [HttpPost(Name = "PostProduct")]
        public async Task<IActionResult> Post([FromBody] PostProductServiceDto dto)
        {
            var guardResult = await Guard_ProductService(null, dto.Title, dto.UnitPrice);
            if (guardResult != null)
            {
                return guardResult;
            }
            var postDto = new GetProductServiceDto();
            var getResponse = await _productService.Get(postDto);

            switch (ModelState.IsValid)
            {
                case true when getResponse.Value is null:
                    {
                        var postResponse = await _productService.Post(dto);
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
        public async Task<IActionResult> Put(Guid id, [FromBody] PutProductServiceDto dto)
        {
            var guardResult = await Guard_ProductService(id, dto.Title, dto.UnitPrice);
            if (guardResult != null)
            {
                return guardResult;
            }



            var putDto = new GetProductServiceDto() { Id = dto.Id };




            if (ModelState.IsValid)
            {
                var putResponse = await _productService.Put(dto);
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
        public async Task<IActionResult> Delete([FromBody] DeleteProductServiceDto dto)
        {
            Guard_ProductService();
            var deleteResponse = await _productService.Delete(dto);
            return deleteResponse.IsSuccessful ? Ok() : BadRequest();
        }
        #endregion

        #region [- ProductServiceGuard() -]
        private async Task<ObjectResult> Guard_ProductService(Guid? productId = null, string title = null, decimal? unitPrice = null)
        {
            if (_productService is null)
            {
                return Problem($" {nameof(_productService)} is null.");
            }

            // Agar title va unitPrice null bashan, yani validation lazem nist (baraye GetAll, Delete)
            if (title == null || unitPrice == null)
            {
                return null;
            }

            // Gereftan liste tamami az mahauslaat
            var existingProducts = await _productService.GetAll();

            // Baraye POST: check mikonim agar ham Title va ham UnitPrice yeki bood, error bede
            if (productId == null) // Baraye POST (productId vojood nadare)
            {
                if (existingProducts.Value.GetProductServiceDtos.Any(p => p.Title == title && p.UnitPrice == unitPrice))
                {
                    return BadRequest("Mahsuli ba in onvan va gheymat ghablan sabt shode ast.");
                }
            }
            else // Baraye PUT (update)
            {
                var existingProduct = existingProducts.Value.GetProductServiceDtos.FirstOrDefault(p => p.Id == productId);

                if (existingProduct != null)
                {
                    // Agar gheymat ghabli == gheymat jadid bashe, update niaz nist
                    if (existingProduct.UnitPrice == unitPrice)
                    {
                        return null;
                    }

                    // Check mikonim ke ghablan mahsoli ba in Title va UnitPrice vojood nadashte bashe
                    if (existingProducts.Value.GetProductServiceDtos.Any(p => p.Title == title && p.UnitPrice == unitPrice && p.Id != productId))
                    {
                        return BadRequest("Gheymat jadid ba yek mahsul digar tekrari ast.");
                    }
                }
            }

            return null;
        }


        #endregion

    }
}



