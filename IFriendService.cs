using Sabio.Models;
using Sabio.Models.Domain.Friends;
using Sabio.Models.Domain.FriendsV2;
using Sabio.Models.Domain.FriendsV3;
using Sabio.Models.Requests.Friends;
using System.Collections.Generic;

namespace Sabio.Services.Interfaces
{
    public interface IFriendService
    {
        int Add(FriendAddRequest model, int userId);
        void Delete(int id);
        Friend Get(int id);
        FriendV2 GetV2(int id);
        FriendV3 GetV3(int id);
        List<Friend> GetAll();
        List<FriendV2> GetAllV2();
        List<FriendV3> GetAllV3();
        void Update(FriendUpdateRequest model, int userId);
        Paged<Friend> Pagination(int pageIndex, int pageSize);
        Paged<FriendV2> PaginationV2(int pageIndex, int pageSize);
        Paged<FriendV3> PaginationV3(int pageIndex, int pageSize);
        Paged<Friend> SearchPaginated(int pageIndex, int pageSize, string query);
        Paged<FriendV2> SearchPaginatedV2(int pageIndex, int pageSize, string query);
        Paged<FriendV3> SearchPaginatedV3(int pageIndex, int pageSize, string query);
    }
}