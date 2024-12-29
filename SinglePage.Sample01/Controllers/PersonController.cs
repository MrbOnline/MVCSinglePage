﻿using Microsoft.AspNetCore.Mvc;
using SinglePage.Sample01.ApplicationServices.Contracts;
using SinglePage.Sample01.ApplicationServices.Dtos.PersonDtos;
using SinglePage.Sample01.ApplicationServices.Services;

namespace SinglePage.Sample01.Controllers
{
    public class PersonController : Controller
    {
        private readonly IPersonService _personService ;

        #region [- ctor -]
        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }
        #endregion

        #region [- Index() -]
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region [- GetAll() -]
        public async Task<IActionResult> GetAll()
        {
            Guard_PersonService();
            var getAllResponse = await _personService.GetAll();
            var response = getAllResponse.Value.GetPersonServiceDtos;
            return Json(response);
        }
        #endregion

        #region [- Get() -]
        public async Task<IActionResult> Get(GetPersonServiceDto dto)
        {
            Guard_PersonService();
            var getResponse = await _personService.Get(dto);
            var response = getResponse.Value;
            if (response is null)
            {
                return Json("NotFound");
            }
            return Json(response);
        }
        #endregion

        #region [- Post() -]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PostPersonServiceDto dto)
        {
            Guard_PersonService();
            var postedDto = new GetPersonServiceDto() { Email = dto.Email };
            var getResponse = await _personService.Get(postedDto);

            if (ModelState.IsValid && getResponse.Value is null)
            {
                var postResponse = await _personService.Post(dto);
                return postResponse.IsSuccessful ? Ok() : BadRequest();
            }
            else if (ModelState.IsValid && getResponse.Value is not null)
            {
                return Conflict(dto);
            }
            else
            {
                return BadRequest();
            }
        }
        #endregion

        #region [- Delete() -]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(DeletePersonServiceDto dto)
        {
            // Guard_PersonService();
            var personToDelete = new DeletePersonServiceDto() { Id = dto.Id };
            //var getResponse = await _personService.Get(personToDelete);

            if (ModelState.IsValid )
            {
                var deleteResponse = await _personService.Delete(personToDelete);
                return deleteResponse.IsSuccessful ? Ok() : BadRequest(deleteResponse.Message);
            }
     
            else
            {
                return BadRequest(ModelState);
            }
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
