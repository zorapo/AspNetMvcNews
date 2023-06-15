using App.Shared.Entities.Abstract;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace App.Entities.Concrete
{
	public class User: IdentityUser, IAuiditEntity
	{
		[StringLength(100)]
		[Column(TypeName = "NVARCHAR")]
		public string? City { get; set; }
		public string? Picture { get; set; }
        public IEnumerable<News>? News { get; set; }
        public IEnumerable<NewsComment>? NewsComments { get; set; }
        public DateTime? CreatedAt { get; set;}=DateTime.Now;
		public DateTime? UpdatedAt { get; set;}= DateTime.Now;
		public DateTime? DeletedAt { get; set;}
	}
}
