using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models.Domain.Addresses;
using Sabio.Models.Requests.Addresses;
using Sabio.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Net.NetworkInformation;
using System.Reflection;

namespace Sabio.Services
{
    public class AddressService : IAddressService
    {
        IDataProvider _data = null;
        public AddressService(IDataProvider data)
        {
            _data = data;
        }

        public void Update(AddressUpdateRequest model)
        {
            string procname = "[dbo].[Sabio_Addresses_Update]";
            _data.ExecuteNonQuery(procname,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    AddCommonParams(model, col);
                    col.AddWithValue("@Id", model.Id);

                },
                returnParameters: null);
        }

        public int Add(AddressAddRequest model, int userId)
        {
            int id = 0;

            string procname = "[dbo].[Sabio_Addresses_Insert]";
            _data.ExecuteNonQuery(procname,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    AddCommonParams(model, col);

                    SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                    idOut.Direction = ParameterDirection.Output;

                    col.Add(idOut);
                },
                returnParameters: delegate (SqlParameterCollection returnCollection)
                {
                    Object oId = returnCollection["@Id"].Value;

                    int.TryParse(oId.ToString(), out id);




                });

            return id;
        }

        public Address Get(int id)
        {
            string procName = "[dbo].[Sabio_Addresses_SelectById]";

            Address address = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection parameterCollection)
            {

                parameterCollection.AddWithValue("id", id);

            }, delegate (IDataReader reader, short set) 
            {            
                address = MapAddress(reader);
            }
            );

            return address;
        }

        public List<Address> GetTop()
        {
            List<Address> list = null;
            string procName = "[dbo].[Sabio_Addresses_SelectRandom50]";

            _data.ExecuteCmd(procName, inputParamMapper: null
            , singleRecordMapper: delegate (IDataReader reader, short set) 
            {
              
                Address aAddress = MapAddress(reader);

                if (list == null)
                {
                    list = new List<Address>();
                }

                list.Add(aAddress);
            }
            );

            return list;
        }

        public List<Address> GetRandomAddresses()
        {
            List<Address> list = null;
            string procName = "[dbo].[Sabio_Addresses_SelectRandom50]";

            _data.ExecuteCmd(procName, inputParamMapper: null
            , singleRecordMapper: delegate (IDataReader reader, short set) 
            {

                Address aAddress = MapAddress(reader);

                if (list == null)
                {
                    list = new List<Address>();
                }

                list.Add(aAddress);
            }
            );

            return list;
        }

        private static Address MapSingleAddress(IDataReader data)
        {
            return null;
        }

        public void Delete(int id)
        {
            string procName = "[dbo].[Sabio_Addresses_DeleteById]";

            _data.ExecuteNonQuery(procName,
               inputParamMapper: delegate (SqlParameterCollection col)
               {
                   col.AddWithValue("@Id", id);

               },
               returnParameters: null
            );

        }

        private static Address MapAddress(IDataReader reader)
        {
            Address aAddress = new Address();

            int startingIndex = 0;

            aAddress.Id = reader.GetSafeInt32(startingIndex++);
            aAddress.LineOne = reader.GetSafeString(startingIndex++);
            aAddress.SuiteNumber = reader.GetSafeInt32(startingIndex++);
            aAddress.City = reader.GetSafeString(startingIndex++);
            aAddress.State = reader.GetSafeString(startingIndex++);
            aAddress.PostalCode = reader.GetSafeString(startingIndex++);            
            aAddress.IsActive = reader.GetSafeBool(startingIndex++);
            aAddress.Lat = reader.GetSafeDouble(startingIndex++);
            aAddress.Long = reader.GetSafeDouble(startingIndex++);
            return aAddress;
        }

        private static void AddCommonParams(AddressAddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@SuiteNumber", model.SuiteNumber);
            col.AddWithValue("@IsActive", model.IsActive);          
            col.AddWithValue("@City", model.City);
            col.AddWithValue("@State", model.State);
            col.AddWithValue("@PostalCode", model.PostalCode);            
            col.AddWithValue("@LineOne", model.LineOne);
            col.AddWithValue("@Lat", model.Lat);
            col.AddWithValue("@Long", model.Long);
        }
    }
}
