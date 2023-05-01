using Sabio.Models.Domain.Addresses;
using Sabio.Models.Requests.Addresses;
using System.Collections.Generic;

namespace Sabio.Services.Interfaces
{
    public interface IAddressService
    {
        int Add(AddressAddRequest model, int CurrentUserId);
        void Delete(int id);
        Address Get(int id);
        List<Address> GetRandomAddresses();
        List<Address> GetTop();
        void Update(AddressUpdateRequest model);
    }
}