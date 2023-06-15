namespace App.Shared.Entities.Abstract
{
	public interface IAuiditEntity
	{
       
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
