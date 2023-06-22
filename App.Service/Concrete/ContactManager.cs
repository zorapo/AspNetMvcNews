using App.Data.Abstract;
using App.Entities.Concrete;
using App.Entities.Dtos;
using App.Service.Abstract;
using App.Shared.Utilities.Results.Abstract;
using App.Shared.Utilities.Results.ComplexTypes;
using App.Shared.Utilities.Results.Concrete;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.Concrete
{
    public class ContactManager :IContactService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ContactManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        //Contact Manager

        public async Task<IDataResult<Contact>> AddContactAsync(Contact contact)
        {


            if (contact is null)
                return new DataResult<Contact>(ResultStatus.Error, "Bir hata oluştu", null);

            contact.IsActive = false;
            await _unitOfWork.Contacts.AddAsync(contact);
            await _unitOfWork.SaveAsync();

            return new DataResult<Contact>(ResultStatus.Success, $"{contact.Name}, mesajınız başarıyla gönderilmiştir.", contact);


        }

        public async Task<IDataResult<Contact>> GetContactAsync(int id)
        {
            var contact= await _unitOfWork.Contacts.GetAsync(a=>a.Id==id);
            if (contact is null)
                return new DataResult<Contact>(ResultStatus.Error, "Bir hata oluştu", null);
            return new DataResult<Contact>(ResultStatus.Success, contact);

        }

        public async Task<IDataResult<ContactListDto>> GetAllContactAsync()
        {
            var model = await _unitOfWork.Contacts.GetAllAsync();
            if (model is null)
                return new DataResult<ContactListDto>(ResultStatus.Error, "Bir hata oluştu", null);
            return new DataResult<ContactListDto>(ResultStatus.Success, new ContactListDto
            {
                Contacts = model,
            });

        }

        public async Task<IDataResult<Contact>> UpdateContactAsync(Contact contact)
        {
           if(contact is null)
                return new DataResult<Contact>(ResultStatus.Error, "Bir hata oluştu", null);

           await _unitOfWork.Contacts.UpdateAsync(contact);
            await _unitOfWork.SaveAsync();
            return new DataResult<Contact>(ResultStatus.Success, contact);
        }

    }
}
