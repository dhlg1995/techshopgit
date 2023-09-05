using Infrastructure.Commons;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Bills
{
	public interface IBillServices
	{
		  Task<ResponseResult> Add(BillViewModel model,List<BillDetailViewModel> detailmodel);
	}
	public class BillServices : IBillServices
	{
		private TechShopDbContext context;

		public BillServices(TechShopDbContext _context)
        {
			context = _context;
        }
        public async Task<ResponseResult> Add(BillViewModel model, List<BillDetailViewModel> detailmodel)
		{		
			using var transaction =await context.Database.BeginTransactionAsync();
			try
			{
				var bill = new Bill()
				{
					FirstName = model.FirstName,
					LastName = model.LastName,
					Address = model.Address,
					Email = model.Email,
					Telephone = model.PhoneNumber,
					TotalAmount = detailmodel.Sum(s => s.Quantity * s.Price),
					PaymentMethod = model.PaymentMethod,
					Id = Guid.NewGuid(),
					CreatedDate = DateTime.Now,
					Status = Enums.EntityStatus.Active,

				};
				context.Bills.Add(bill);
				await context.SaveChangesAsync();
				foreach (var item in detailmodel)
				{
					var billdetail = new BillDetail()
					{
						ProductName=item.ProductName,
						UnitPrice=item.Price,
						Quantity=item.Quantity,
						Id=new Guid(),
						CreatedDate=bill.CreatedDate,
						BillId=bill.Id,
						Status=bill.Status,
					};
					context.BillDetails.Add(billdetail);
					await context.SaveChangesAsync();

				}
				await transaction.CommitAsync();
				return new ResponseResult(200, "Success");
			}
			catch (Exception)
			{
				await transaction.RollbackAsync();
				return new ResponseResult(400, "Some thing went wrong");
			}

		}

	
	}
}
