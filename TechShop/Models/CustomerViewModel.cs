using System.ComponentModel.DataAnnotations;

namespace TechShop.Models
{
	public class CustomerViewModel
	{
		[Required]
        public string FirstName { get; set; }

		[Required]
		public string LastName { get; set; }
		[Required]
		public string Email { get; set; }
		[Required]
		public int PhoneNumber { get; set; }
		[Required]
		public string Address { get; set; }
	}
}
