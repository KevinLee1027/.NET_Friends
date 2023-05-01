using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using System.Collections.Generic;
using Sabio.Models.Domain.Addresses;
using Sabio.Web.Models.Responses;
using Sabio.Models.Requests.Addresses;
using System;
using System.Data.SqlClient;
using Sabio.Services;
using Sabio.Models;
using Sabio.Web.Models;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/addresses")]
    [ApiController]
    public class AddressApiController : BaseApiController 
    {
        private IAddressService _service = null;
        private IAuthenticationService<int> _authService = null;
        public AddressApiController(IAddressService service
            , ILogger<AddressApiController> logger
            , IAuthenticationService<int> authService
            ) : base(logger)
        {
            _service= service;
            _authService = authService;
        }
        
        [HttpGet("")]
        public ActionResult<ItemsResponse<Address>> GetAll()
        {
            List<Address> list = _service.GetTop();
            ItemsResponse<Address> response = new ItemsResponse<Address>();
            response.Items = list;

            if(list ==null)
            {
                return NotFound404(response);
            }
            else
            {
                return Ok(response);
            }            
        }

        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<Address>> Get(int id) 
        {
            try
            {
                Address a = _service.Get(id);

                ItemResponse<Address> response = new ItemResponse<Address>();
                response.Item = a;

                if (response.Item == null)
                {
                    return NotFound404(response);
                }
                else
                {
                    return Ok(response);
                }
            }
            catch (SqlException sqlEx) 
            {
                return base.StatusCode(500, new ErrorResponse($"sqlexception error: {sqlEx.Message}"));
            }
            catch (ArgumentException argEx) 
            {
                return base.StatusCode(500, new ErrorResponse($"argumentexception error: {argEx.Message}"));
            }
            catch (Exception ex) 
            {
                base.Logger.LogError(ex.ToString());
                return base.StatusCode(500, new ErrorResponse($"generic error: {ex.Message}"));
            }
        }

        [HttpPost]
        public ActionResult<ItemResponse<int>> Create(AddressAddRequest model)
        {

            int userId = _authService.GetCurrentUserId();
            IUserAuthData user = _authService.GetCurrentUser();
        
            ObjectResult result = null;

            try
            {                
                int id = _service.Add(model, user.Id);  
                ItemResponse<int> response = new ItemResponse<int>() { Item = id };
              
                result = Created201(response);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse(ex.Message);

                result = StatusCode(500, response);
            }
            return result;
        }

        [HttpDelete("{id:int}") ]
        public ActionResult<SuccessResponse> Delete(int id) 
        {
            ObjectResult result = null;

            try
            {
                _service.Delete(id);

                SuccessResponse response = new SuccessResponse();

                result = StatusCode(200, response);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse(ex.Message);

                result = StatusCode(500, response);
            }
            return result;
        }

        [HttpPut("{id:int}")]
        public ActionResult<ItemResponse<int>> Update(AddressUpdateRequest model)
        {
            _service.Update(model);

            SuccessResponse response = new SuccessResponse();

            return Ok(response);
        }

    }
}
