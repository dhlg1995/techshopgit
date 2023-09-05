using Infrastructure.Enums;

namespace TechShop.Models
{
	public class CheckoutViewModel
	{
		public List<CartViewModel> cart { get; set; }	

		private Dictionary<int,string> payments;
			
		public Dictionary<int,string> Payments
		{
			get { return payments; }
			set {
				payments = value;
			}
		}	









	}
}
