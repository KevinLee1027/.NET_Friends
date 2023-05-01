using Microsoft.AspNetCore.Mvc;
using Sabio.Services.Interfaces;
using Sabio.Services;
using Sabio.Web.Controllers;
using Microsoft.Extensions.Logging;
using Sabio.Models.Domain.Friends;
using Sabio.Web.Models.Responses;
using System;
using Sabio.Models.Domain.FriendsV3;
using Sabio.Models.Domain.FriendsV2;
using System.Collections.Generic;
using Sabio.Models;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/v3/friends")]
    [ApiController]
    public class FriendApiControllerV3 : BaseApiController
    {
        private IFriendService _service = null;
        private IAuthenticationService<int> _authService = null;
        public FriendApiControllerV3(IFriendService service
            , ILogger<FriendApiControllerV3> logger
            , IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;
        }

        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<FriendV3>> GetByIdV3(int id)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                FriendV3 friend = _service.GetV3(id);

                if (friend == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Friend not found.");
                }
                else
                {
                    response = new ItemResponse<FriendV3>() { Item = friend };
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
        public ActionResult<ItemsResponse<FriendV3>> GetAllV3()
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                List<FriendV3> list = _service.GetAllV3();

                if (list == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Friends not found.");
                }
                else
                {
                    response = new ItemsResponse<FriendV3> { Items = list };
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
        [HttpGet("paginate")]
        public ActionResult<ItemResponse<Paged<FriendV3>>> PaginationV3(int pageIndex, int pageSize)
        {

            int iCode = 200;
            BaseResponse response = null;

            try
            {
                Paged<FriendV3> paged = _service.PaginationV3(pageIndex, pageSize);

                if (paged == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Friends not found.");
                }
                else
                {
                    iCode = 200;
                    response = new ItemResponse<Paged<FriendV3>> { Item = paged };
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
        public ActionResult<ItemResponse<Paged<FriendV3>>> SearchPaginatedV3(int pageIndex, int pageSize, string query)
        {

            int iCode = 200;
            BaseResponse response = null;

            try
            {
                Paged<FriendV3> paged = _service.SearchPaginatedV3(pageIndex, pageSize, query);

                if (paged == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Friends not found.");
                }
                else
                {
                    iCode = 200;
                    response = new ItemResponse<Paged<FriendV3>> { Item = paged };
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
