using Sabio.Data.Providers;
using Sabio.Models.Domain.Friends;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabio.Data;
using Sabio.Models.Requests.Friends;
using Sabio.Models.Requests.Users;
using Sabio.Models.Domain.Users;
using Sabio.Services.Interfaces;
using Sabio.Models;
using System.IO.MemoryMappedFiles;
using Sabio.Models.Domain.FriendsV2;
using Sabio.Models.Domain.Images;
using Sabio.Models.Domain.FriendsV3;
using Sabio.Models.Domain.Skills;

namespace Sabio.Services
{
    public class FriendService : IFriendService
    {
        IDataProvider _data = null;
        public FriendService(IDataProvider data)
        {
            _data = data;
        }


        public Friend Get(int id)
        {
            string procName = "[dbo].[Friends_SelectById]";

            Friend Friend = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection parameterCollection)
            {

                parameterCollection.AddWithValue("@id", id);

            }, delegate (IDataReader reader, short set) 
            {

                Friend = MapSingleFriend(reader);
            }
            );

            return Friend;
        }
        public FriendV2 GetV2(int id)
        {
            string procName = "[dbo].[Friends_SelectByIdV2]";

            FriendV2 friend = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection parameterCollection)
            {

                parameterCollection.AddWithValue("@id", id);

            }, delegate (IDataReader reader, short set) 
            {
                friend = MapSingleFriendV2(reader);
            }
            );

            return friend;
        }
        public FriendV3 GetV3(int id)
        {
            string procName = "[dbo].[Friends_SelectByIdV3]";

            FriendV3 friend = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection parameterCollection)
            {

                parameterCollection.AddWithValue("@id", id);

            }, delegate (IDataReader reader, short set) 
            {
                friend = MapSingleFriendV3(reader);
            }
            );

            return friend;
        }
        public List<Friend> GetAll()
        {
            List<Friend> list = null;
            string procName = "[dbo].[Friends_SelectAll]";

            _data.ExecuteCmd(procName, inputParamMapper: null
            , singleRecordMapper: delegate (IDataReader reader, short set) 
            {

                Friend aFriend = MapSingleFriend(reader);

                if (list == null)
                {
                    list = new List<Friend>();
                }

                list.Add(aFriend);
            }
            );

            return list;
        }
        public List<FriendV2> GetAllV2()
        {
            List<FriendV2> list = null;
            string procName = "[dbo].[Friends_SelectAllV2]";

            _data.ExecuteCmd(procName, inputParamMapper: null
            , singleRecordMapper: delegate (IDataReader reader, short set) 
            {

                FriendV2 aFriend = MapSingleFriendV2(reader);

                if (list == null)
                {
                    list = new List<FriendV2>();
                }

                list.Add(aFriend);
            }
            );

            return list;
        }
        public List<FriendV3> GetAllV3()
        {
            List<FriendV3> list = null;
            string procName = "[dbo].[Friends_SelectAllV3]";

            _data.ExecuteCmd(procName, inputParamMapper: null
            , singleRecordMapper: delegate (IDataReader reader, short set) 
            {
                
                FriendV3 aFriend = MapSingleFriendV3(reader);

                if (list == null)
                {
                    list = new List<FriendV3>();
                }

                list.Add(aFriend);
            }
            );

            return list;
        }
        public void Delete(int id)
        {
            string procName = "[dbo].[Friends_Delete]";

            _data.ExecuteNonQuery(procName,
               inputParamMapper: delegate (SqlParameterCollection col)
               {
                   col.AddWithValue("@Id", id);

               },
               returnParameters: null
            );

        }
        public int Add(FriendAddRequest model, int userId)
        {
            int id = 0;

            string procname = "[dbo].[Friends_Insert]";
            _data.ExecuteNonQuery(procname,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    AddCommonParams(model, col);
                    col.AddWithValue("@UserId", userId);

                    SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                    idOut.Direction = ParameterDirection.Output;

                    col.Add(idOut);
                },
                returnParameters: delegate (SqlParameterCollection returnCollection)
                {
                    object oId = returnCollection["@Id"].Value;

                    int.TryParse(oId.ToString(), out id);

                });

            return id;
        }
        public void Update(FriendUpdateRequest model, int userId)
        {
            string procname = "[dbo].[Friends_Update]";
            _data.ExecuteNonQuery(procname,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    AddCommonParams(model, col);
                    col.AddWithValue("@UserId", userId);
                    col.AddWithValue("@Id", model.Id);

                },
                returnParameters: null);
        }
        public Paged<Friend> Pagination(int pageIndex, int pageSize)
        {
            Paged<Friend> pagedList = null;
            List<Friend> list = null;
            int totalCount = 0;
            string procname = "[dbo].[Friends_Pagination]";

            _data.ExecuteCmd(procname,
                (param) =>
                {
                    param.AddWithValue("@PageIndex", pageIndex);
                    param.AddWithValue("@PageSize", pageSize);
                },
                (reader, recordSetIndex) =>
                {
                    Friend friend = MapSingleFriend(reader);
                    totalCount = reader.GetSafeInt32(6);

                    if (list == null)
                    {
                        list = new List<Friend>();
                    }
                    list.Add(friend);
                }
                );
            if (list != null)
            {
                pagedList = new Paged<Friend>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }
        public Paged<FriendV2> PaginationV2(int pageIndex, int pageSize)
        {
            Paged<FriendV2> pagedList = null;
            List<FriendV2>list = null;
            int totalCount = 0;
            string procname = "[dbo].[Friends_PaginationV2]";

            _data.ExecuteCmd(procname,
                (param) =>
                {
                    param.AddWithValue("@PageIndex", pageIndex);
                    param.AddWithValue("@PageSize", pageSize);
                },
                (reader, recordSetIndex) =>
                {
                    FriendV2 friend = MapSingleFriendV2(reader);
                    totalCount = reader.GetSafeInt32(6);

                    if (list == null)
                    {
                        list = new List<FriendV2>();
                    }
                    list.Add(friend);
                }
                );
            if (list != null)
            {
                pagedList = new Paged<FriendV2>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }
        public Paged<FriendV3> PaginationV3(int pageIndex, int pageSize)
        {
            Paged<FriendV3> pagedList = null;
            List<FriendV3> list = null;
            int totalCount = 0;
            string procname = "[dbo].[Friends_PaginationV3]";

            _data.ExecuteCmd(procname,
                (param) =>
                {
                    param.AddWithValue("@PageIndex", pageIndex);
                    param.AddWithValue("@PageSize", pageSize);
                },
                (reader, recordSetIndex) =>
                {
                    FriendV3 friend = MapSingleFriendV3(reader);
                    totalCount = reader.GetSafeInt32(6);

                    if (list == null)
                    {
                        list = new List<FriendV3>();
                    }
                    list.Add(friend);
                }
                );
            if (list != null)
            {
                pagedList = new Paged<FriendV3>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        public Paged<Friend> SearchPaginated(int pageIndex, int pageSize, string query)
        {
            Paged<Friend> pagedList = null;
            List<Friend> list = null;
            int totalCount = 0;
            string procname = "[dbo].[Friends_Search_Pagination]";

            _data.ExecuteCmd(procname,
                (param) =>
                {
                    param.AddWithValue("@PageIndex", pageIndex);
                    param.AddWithValue("@PageSize", pageSize);
                    param.AddWithValue("@Query", query);
                },
                (reader, recordSetIndex) =>
                {
                    Friend friend = MapSingleFriend(reader);
                    totalCount = reader.GetSafeInt32(6);

                    if (list == null)
                    {
                        list = new List<Friend>();
                    }
                    list.Add(friend);
                }
                );
            if (list != null)
            {
                pagedList = new Paged<Friend>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }
        public Paged<FriendV2> SearchPaginatedV2(int pageIndex, int pageSize, string query)
        {
            Paged<FriendV2> pagedList = null;
            List<FriendV2> list = null;
            int totalCount = 0;
            string procname = "[dbo].[Friends_Search_PaginationV2]";

            _data.ExecuteCmd(procname,
                (param) =>
                {
                    param.AddWithValue("@PageIndex", pageIndex);
                    param.AddWithValue("@PageSize", pageSize);
                    param.AddWithValue("@Query", query);
                },
                (reader, recordSetIndex) =>
                {
                    FriendV2 friend = MapSingleFriendV2(reader);
                    totalCount = reader.GetSafeInt32(6);

                    if (list == null)
                    {
                        list = new List<FriendV2>();
                    }
                    list.Add(friend);
                }
                );
            if (list != null)
            {
                pagedList = new Paged<FriendV2>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }
        public Paged<FriendV3> SearchPaginatedV3(int pageIndex, int pageSize, string query)
        {
            Paged<FriendV3> pagedList = null;
            List<FriendV3> list = null;
            int totalCount = 0;
            string procname = "[dbo].[Friends_Search_PaginationV3]";

            _data.ExecuteCmd(procname,
                (param) =>
                {
                    param.AddWithValue("@PageIndex", pageIndex);
                    param.AddWithValue("@PageSize", pageSize);
                    param.AddWithValue("@Query", query);
                },
                (reader, recordSetIndex) =>
                {
                    FriendV3 friend = MapSingleFriendV3(reader);
                    totalCount = reader.GetSafeInt32(6);

                    if (list == null)
                    {
                        list = new List<FriendV3>();
                    }
                    list.Add(friend);
                }
                );
            if (list != null)
            {
                pagedList = new Paged<FriendV3>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }
        private static Friend MapSingleFriend(IDataReader reader)
        {
            Friend aFriend = new Friend();

            int startingIndex = 0;

            aFriend.Id = reader.GetSafeInt32(startingIndex++);
            aFriend.Title = reader.GetSafeString(startingIndex++);
            aFriend.Bio = reader.GetSafeString(startingIndex++);
            aFriend.Summary = reader.GetSafeString(startingIndex++);
            aFriend.Headline = reader.GetSafeString(startingIndex++);
            aFriend.Slug = reader.GetSafeString(startingIndex++);
            aFriend.StatusId = reader.GetSafeInt32(startingIndex++);
            aFriend.UserId = reader.GetSafeInt32(startingIndex++);
            aFriend.PrimaryImageUrl = reader.GetSafeString(startingIndex++);
            aFriend.DateCreated = reader.GetSafeDateTime(startingIndex++);
            aFriend.DateModified = reader.GetSafeDateTime(startingIndex++);
            return aFriend;
        }
        private static FriendV2 MapSingleFriendV2(IDataReader reader)
        {
            FriendV2 aFriend = new FriendV2();
            aFriend.PrimaryImage = new Image();

            int startingIndex = 0;

            aFriend.Id = reader.GetSafeInt32(startingIndex++);
            aFriend.Title = reader.GetSafeString(startingIndex++);
            aFriend.Bio = reader.GetSafeString(startingIndex++);
            aFriend.Summary = reader.GetSafeString(startingIndex++);
            aFriend.Headline = reader.GetSafeString(startingIndex++);
            aFriend.Slug = reader.GetSafeString(startingIndex++);
            aFriend.StatusId = reader.GetSafeInt32(startingIndex++);            
            aFriend.PrimaryImage.Id = reader.GetSafeInt32(startingIndex++);
            aFriend.PrimaryImage.TypeId = reader.GetSafeInt32(startingIndex++);
            aFriend.PrimaryImage.Url = reader.GetSafeString(startingIndex++);
            aFriend.UserId = reader.GetSafeInt32(startingIndex++);
            aFriend.DateCreated = reader.GetSafeDateTime(startingIndex++);
            aFriend.DateModified = reader.GetSafeDateTime(startingIndex++);
            return aFriend;
        }
        private static FriendV3 MapSingleFriendV3(IDataReader reader)
        {
            FriendV3 aFriend = new FriendV3();
            aFriend.PrimaryImage = new Image();
            

            int startingIndex = 0;

            aFriend.Id = reader.GetSafeInt32(startingIndex++);
            aFriend.Title = reader.GetSafeString(startingIndex++);
            aFriend.Bio = reader.GetSafeString(startingIndex++);
            aFriend.Summary = reader.GetSafeString(startingIndex++);
            aFriend.Headline = reader.GetSafeString(startingIndex++);
            aFriend.Slug = reader.GetSafeString(startingIndex++);
            aFriend.StatusId = reader.GetSafeInt32(startingIndex++);
            aFriend.PrimaryImage.Id = reader.GetSafeInt32(startingIndex++);
            aFriend.PrimaryImage.TypeId = reader.GetSafeInt32(startingIndex++);
            aFriend.PrimaryImage.Url = reader.GetSafeString(startingIndex++);
            aFriend.Skills = reader.DeserializeObject<List<Skill>>(startingIndex++);
            aFriend.UserId = reader.GetSafeInt32(startingIndex++);
            aFriend.DateCreated = reader.GetSafeDateTime(startingIndex++);
            aFriend.DateModified = reader.GetSafeDateTime(startingIndex++);
            return aFriend;
        }
        private static void AddCommonParams(FriendAddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@Title", model.Title);
            col.AddWithValue("@Bio", model.Bio);
            col.AddWithValue("@Summary", model.Summary);
            col.AddWithValue("@Headline", model.Headline);
            col.AddWithValue("@Slug", model.Slug);
            col.AddWithValue("@StatusId", model.StatusId);
            col.AddWithValue("@PrimaryImageUrl", model.PrimaryImageUrl);
        }
    }
}
