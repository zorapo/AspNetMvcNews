using App.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entities.Dtos
{
    public class ContactListDto
    {
        public IList<Contact>? Contacts { get; set; }
    }
}
