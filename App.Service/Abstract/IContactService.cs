using App.Entities.Concrete;
using App.Entities.Dtos;
using App.Shared.Utilities.Results.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.Abstract
{
    public interface IContactService
    {
        //ContactDto service
        Task<IDataResult<Contact>> AddContactAsync(Contact contact);
        Task<IDataResult<  Contact >> GetContactAsync(int id);
        Task<IDataResult<ContactListDto>> GetAllContactAsync();
        Task<IDataResult<Contact>> UpdateContactAsync(Contact contact);
    }
}
