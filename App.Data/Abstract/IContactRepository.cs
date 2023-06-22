using App.Entities.Concrete;
using App.Shared.Data.Abstract;

namespace App.Data.Abstract
{
    public interface IContactRepository: IEntityRepository<Contact>
    {
    }
}
