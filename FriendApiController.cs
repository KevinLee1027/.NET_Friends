using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain.Friends;
using Sabio.Models.Domain.FriendsV2;
using Sabio.Models.Domain.Users;
using Sabio.Models.Requests.Friends;
using Sabio.Models.Requests.Users;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;
using System.Web;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/friends")]
    [ApiController]
    public class FriendApiController : BaseApiController
    {
        private IFriendService _service = null;
        private IAuthenticationService<int> _authService = null;
        public FriendApiController(IFriendService service
            , ILogger<FriendApiController> logger
            , IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;
        }
        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<Friend>> GetById(int id)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                Friend friend = _service.Get(id);

                if (friend== null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Friend not found.");
                }
                else
                {
                    response = new ItemResponse<Friend>() { Item = friend };
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
        public ActionResult<ItemsResponse<Friend>> GetAll()
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                List<Friend> list = _service.GetAll();

                if (list == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Friends not found.");
                }
                else
                {
                    response = new ItemsResponse<Friend> { Items = list };
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

        [HttpPost]
        public ActionResult<ItemResponse<int>> Create(FriendAddRequest model)
        {           
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
                ErrorResponse response = new ErrorResponse(ex.Message);
                result = StatusCode(500, response);
            }
            return result;
        }

        [HttpPut("{id:int}")]
        public ActionResult<SuccessResponse> Update(FriendUpdateRequest model)
        {
            int iCode = 200;
            BaseResponse response = null;
            IUserAuthData user = _authService.GetCurrentUser();

            try
            {
                _service.Update(model, user.Id);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse(ex.Message);
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

        [HttpGet("paginate")]
        public ActionResult<ItemResponse<Paged<Friend>>> Pagination(int pageIndex, int pageSize) 
        {           

            int iCode = 200;
            BaseResponse response = null;

            try
            {
                Paged<Friend> paged = _service.Pagination(pageIndex, pageSize);

                if (paged == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Friends not found.");
                }
                else
                {
                    iCode = 200;
                    response = new ItemResponse<Paged<Friend>> { Item = paged };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse(ex.Message.ToString());
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(iCode, response);
        }
        [HttpGet("search")]
        public ActionResult<ItemResponse<Paged<Friend>>> SearchPaginated(int pageIndex, int pageSize, string query)
        {

            int iCode = 200;
            BaseResponse response = null;

            try
            {
                Paged<Friend> paged = _service.SearchPaginated(pageIndex, pageSize, query);

                if (paged == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Friends not found.");
                }
                else
                {
                    iCode = 200;
                    response = new ItemResponse<Paged<Friend>> { Item = paged };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse(ex.Message.ToString());
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(iCode, response);
        }
    }
}
