using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Services.Interfaces;
using Sabio.Services;
using Sabio.Web.Controllers;
using Sabio.Models.Domain.FriendsV2;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;
using Sabio.Models.Domain.Friends;
using Sabio.Models;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/v2/friends")]
    [ApiController]
    public class FriendApiControllerV2 : BaseApiController
    {
        private IFriendService _service = null;
        private IAuthenticationService<int> _authService = null;
        public FriendApiControllerV2(IFriendService service
            , ILogger<FriendApiControllerV2> logger
            , IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;
        }
        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<FriendV2>> GetByIdV2(int id)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                FriendV2 friend = _service.GetV2(id);

                if (friend == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Friend not found.");
                }
                else
                {
                    response = new ItemResponse<FriendV2>() { Item = friend };
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
        public ActionResult<ItemsResponse<FriendV2>> GetAllV2()
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                List<FriendV2> list = _service.GetAllV2();

                if (list == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Friends not found.");
                }
                else
                {
                    response = new ItemsResponse<FriendV2> { Items = list };
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
        public ActionResult<ItemResponse<Paged<FriendV2>>> PaginationV2(int pageIndex, int pageSize)
        {

            int iCode = 200;
            BaseResponse response = null;

            try
            {
                Paged<FriendV2> paged = _service.PaginationV2(pageIndex, pageSize);

                if (paged == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Friends not found.");
                }
                else
                {
                    iCode = 200;
                    response = new ItemResponse<Paged<FriendV2>> { Item = paged };
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
        public ActionResult<ItemResponse<Paged<FriendV2>>> SearchPaginatedV2(int pageIndex, int pageSize, string query)
        {

            int iCode = 200;
            BaseResponse response = null;

            try
            {
                Paged<FriendV2> paged = _service.SearchPaginatedV2(pageIndex, pageSize, query);

                if (paged == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Friends not found.");
                }
                else
                {
                    iCode = 200;
                    response = new ItemResponse<Paged<FriendV2>> { Item = paged };
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
