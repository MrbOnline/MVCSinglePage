using FinalProject.BackEnd.ApplicationServices.Contracts;
using FinalProject.BackEnd.ApplicationServices.Dtos.PersonDtos;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.BackEnd.Controllers
{
  
        [ApiController]
        [Route("[controller]")]
        public class PersonController : ControllerBase
        {
            private readonly IPersonService _personService;
            private readonly ILogger<PersonController> _logger;
            #region [- ctor -]
            public PersonController(IPersonService personService, ILogger<PersonController> logger)
            {
                _personService = personService;
                _logger = logger;
            }
            #endregion


            #region [- GetAll() -]
            [HttpGet("all")]
            public async Task<IActionResult> GetAll()
            {
                Guard_PersonService();
                var getAllResponse = await _personService.GetAll();
                var response = getAllResponse.Value.GetPersonServiceDtos;
                return new JsonResult(response);
            }
            #endregion

            #region [- Get() -]
            [HttpGet("{id:guid}")]
            public async Task<IActionResult> Get(Guid id)
            {
                Guard_PersonService();
                var dto = new GetPersonServiceDto() { Id = id };
                var getResponse = await _personService.Get(dto);
                var response = getResponse.Value;
                if (response is null)
                {
                    return new JsonResult("NotFound");
                }
                return new JsonResult(response);
            }
            #endregion

            #region [- Post() -]
            [HttpPost(Name = "PostPerson")]
            public async Task<IActionResult> Post([FromBody] PostPersonServiceDto dto)
            {
                Guard_PersonService();
                var postDto = new GetPersonServiceDto() { Email = dto.Email };
                var getResponse = await _personService.Get(postDto);

                switch (ModelState.IsValid)
                {
                    case true when getResponse.Value is null:
                        {
                            var postResponse = await _personService.Post(dto);
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
            public async Task<IActionResult> Put([FromBody] PutPersonServiceDto dto)
            {
                Guard_PersonService();

                //Pay attention to Email uniqueness problem in the app.

                var putDto = new GetPersonServiceDto() { Email = dto.Email };

                #region [- For checking & avoiding email duplication -]
                //var getResponse = await _personService.Get(putDto);//For checking & avoiding email duplication
                //switch (ModelState.IsValid)
                //{
                //    case true when getResponse.Value is null:
                //    {
                //        var putResponse = await _personService.Put(dto);
                //        return putResponse.IsSuccessful ? Ok() : BadRequest();
                //    }
                //    case true when getResponse.Value is not null://For checking & avoiding email duplication
                //        return Conflict(dto);
                //    default:
                //        return BadRequest();
                //} 
                #endregion

                if (ModelState.IsValid)
                {
                    var putResponse = await _personService.Put(dto);
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
            public async Task<IActionResult> Delete([FromBody] DeletePersonServiceDto dto)
            {
                Guard_PersonService();
                var deleteResponse = await _personService.Delete(dto);
                return deleteResponse.IsSuccessful ? Ok() : BadRequest();
            }
            #endregion

            #region [- PersonServiceGuard() -]
            private ObjectResult Guard_PersonService()
            {
                if (_personService is null)
                {
                    return Problem($" {nameof(_personService)} is null.");
                }

                return null;
            }
        #endregion
      
    }
}

    

