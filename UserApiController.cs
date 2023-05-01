
using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Logging;
using Sabio.Models.Domain.Users;
using Sabio.Services;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System.Collections.Generic;
using Sabio.Services.Interfaces;
using System.Data.SqlClient;
using System;
using Microsoft.OpenApi.Expressions;
using Sabio.Models.Requests.Addresses;
using Sabio.Models;
using Sabio.Models.Requests.Users;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/users")]
    [ApiController]

    public class UserApiController : BaseApiController
    {
        private IUserServiceV1 _service = null;
        private IAuthenticationService<int> _authService = null;

        public UserApiController(IUserServiceV1 service 
            , ILogger<UserApiController> logger
            , IAuthenticationService<int> authService) : base(logger) 
        {
            _service = service;
            _authService = authService;
        }
        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<User>> GetById(int id)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                User a = _service.Get(id);               

                if (a == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("User not found.");
                }
                else
                {
                    response = new ItemResponse<User>() { Item = a };
                }
            }            
            catch (Exception ex)
            {
                iCode = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse(ex.Message);                
            }
            return StatusCode(iCode, response);
        }

        [HttpGet("")]
        public ActionResult<ItemsResponse<User>> GetAll()
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {             
                List<User> list = _service.GetAll();
                
                if (list == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Users not found.");
                }
                else
                {
                    response = new ItemsResponse<User> { Items = list };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse(ex.Message);

                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(iCode, response);


        }

        [HttpDelete("{id:int}")]
        public ActionResult<SuccessResponse> Delete(int id)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                _service.Delete(id);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(iCode, response);
        }

        [HttpPost]
        public ActionResult<ItemResponse<int>> Create(UserAddRequest model)
        {           

            ObjectResult result = null;

            try
            {               
                int id = _service.Add(model); 
                ItemResponse<int> response = new ItemResponse<int>() { Item = id };               
                result = Created201(response);
            }
            catch (Exception ex)
            {                
                ErrorResponse response = new ErrorResponse(ex.Message);
                result = StatusCode(500, response);
            }
            return result;
        }

        [HttpPut("{id:int}")]
        public ActionResult<SuccessResponse> Update(UserUpdateRequest model)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                _service.Update(model);
                response = new SuccessResponse();
            }
            catch (Exception ex) 
            {
                iCode= 500;
                response= new ErrorResponse(ex.Message);
            }
            return StatusCode(iCode, response);
        }
    }
}
