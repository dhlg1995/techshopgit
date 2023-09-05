using Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Bills
{
	public class BillViewModel
	{
        public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string Address { get; set; }
		public string PhoneNumber { get; set; }
		public PaymentMethod PaymentMethod { get; set; }
        public List<BillDetailViewModel> BillDetail { get; set; }

    }
	public class BillDetailViewModel
	{
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
