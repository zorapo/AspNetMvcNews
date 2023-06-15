namespace App.Shared.Entities.Abstract
{
    public abstract class EntityBase
    {
        public virtual bool IsDeleted { get; set; }=false;
        public virtual bool IsActive { get; set; } = true;
        public virtual string CreatedByName { get; set; } = "Admin";
        public virtual string ModifiedByName { get; set; } = "Admin";
    }
}
