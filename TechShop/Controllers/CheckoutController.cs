using Infrastructure.Bills;
using Infrastructure.Enums;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Drawing.Drawing2D;
using TechShop.Models;

namespace TechShop.Controllers
{
	public class CheckoutController : Controller
	{
		private IBillServices _billServices;
        public CheckoutController(IBillServices billService)
        {
			_billServices = billService;
        }
        public IActionResult Index()
		{
			var _cart = HttpContext.Session.getCart(CartExtension._key);
			CheckoutViewModel cvm = new CheckoutViewModel();
			cvm.cart = _cart;
			cvm.Payments = getpayment();
			return View(cvm);
		}
		public async Task<IActionResult> CheckoutCart(CustomerViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}
			BillViewModel bill = new BillViewModel();
			bill.FirstName = model.FirstName;
			bill.LastName	= model.LastName;
			bill.Address = model.Address;
			bill.Email = model.Email;
			bill.PhoneNumber = model.PhoneNumber.ToString();
			List<BillDetailViewModel> dt = new List<BillDetailViewModel>();
			var items = HttpContext.Session.getCart(CartExtension._key);
			foreach (var item in items)
			{
				dt.Add(new BillDetailViewModel
				{
					Price = item.Price,
					ProductName = item.ProductName,
					Quantity = item.Quantity
				});
			}
			var response = await _billServices.Add(bill, dt);
			HttpContext.Session.Remove(CartExtension._key);
			TempData["checkout"] = JsonConvert.SerializeObject(response);
			return RedirectToAction("Index", "Checkout");

		}

		public Dictionary<int, string> getpayment()
		{
			var x = Enum.GetNames(typeof(PaymentMethod));
			Dictionary<int, string> rs = new Dictionary<int, string>();			
			foreach (var item in x){
				rs.Add((int)Enum.Parse(typeof(PaymentMethod),item), item);				
			}			
			return rs;
		}
	}
}
