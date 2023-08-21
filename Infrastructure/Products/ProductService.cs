using Infrastructure.Entities;
using Infrastructure.Enums;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Products
{
	public interface IProductService
	{
		List<ReviewModel> GetReviewModels(Guid productId,ReviewFilterModel _model);
		List<ReviewModel> GetReviewModels(Guid productId);
		ProductDetailViewModel GetProductDetails(Guid productId);
        List<ProductViewModel> SearchProduct(ProductFilterModel filter);
		int InsertReview(ReviewModel model);
	}
	public class ProductService : IProductService
	{
		private readonly TechShopDbContext _context;

		public ProductService(TechShopDbContext context)
		{
			_context = context;
		}

        public ProductDetailViewModel GetProductDetails(Guid productId)
        {
			var rs = new ProductDetailViewModel();

			var product = _context.Products.Find(productId);
			if (product == null)
			{ 
				return null; 
			}
			rs.Id = product.Id; 
			rs.Name=product.Name;
			rs.Description=product.Description;
			rs.Detail = product.Detail;
			rs.Quantity=product.Quantity;
			rs.Price=product.Price; 
			rs.DiscountPrice=product.DiscountPrice;
			rs.CategoryId=product.CategoryId;
			rs.CategoryName = GetCategory(product.CategoryId);
			rs.Reviews = GetReviewModels(productId);
			rs.Images = GetImage(productId);
			return rs;
		}
		private List<ImageViewModel> GetImage(Guid productId)
		{
			var imgs = _context.ProductImages.Where(x => x.ProductId == productId).Select(s => new ImageViewModel
			{
				Id = s.Id,
				ImageLink = s.ImageLink,
				Alt = s.Alt
			});
			return imgs.ToList();
		}
		private string GetCategory(Guid categoryId)
		{			
				var category = _context.Categories.Find(categoryId);
				return category != null ?  category.Name: string.Empty;		
		}
		public List<ReviewModel> GetReviewModels(Guid ProductId)
		{
			var filter = new ReviewFilterModel();
			var rs = _context.Reviews.Where(s => s.ProductId == ProductId)
				.Select(x => new ReviewModel
				{
					Content = x.Content,
					Rating = x.Rating,
					Id = x.Id,
					ReviewerName = x.ReviewerName,
					Email = x.Email,
					CreatedDate=x.CreatedDate
				});
			
			return rs.OrderByDescending(x => x.Rating).ToList();
		}
		public List<ReviewModel> GetReviewModels(Guid ProductId,ReviewFilterModel _model)
		{
			var lsRv = new List<ReviewModel>();
			var filter = new ReviewFilterModel();
			var rs = _context.Reviews.Where(s => s.ProductId == ProductId)
				.Select(x => new ReviewModel
				{
					Content = x.Content,
					Rating = x.Rating,
					Id = x.Id,
					ReviewerName = x.ReviewerName,
					Email = x.Email,
					CreatedDate = x.CreatedDate
				});
			rs = rs.OrderByDescending(x => x.Rating).Skip((_model.PageIndex - 1) * _model.PageSize).Take(_model.PageSize);
			lsRv = rs.ToList();
			return lsRv;
		}

		public List<ProductViewModel> SearchProduct(ProductFilterModel filter)
		{
			var productViewModels = new List<ProductViewModel>();
			var products = _context.Products.AsQueryable();
			var categories = _context.Categories.AsQueryable();
			var images = _context.ProductImages.AsQueryable();
			var reviews = _context.Reviews.AsQueryable();

			var result = (from p in products
						  join c in categories
						  on p.CategoryId equals c.Id
						  select new ProductViewModel
						  {
							  Id = p.Id,
							  Name = p.Name,
							  Price = p.Price,
							  DiscountPrice = p.DiscountPrice,
							  Description = p.Description,
							  Detail = p.Detail,
							  CategoryName = c.Name,
							  CategoryId = c.Id,
						  }).AsEnumerable();
			if (!string.IsNullOrEmpty(filter.CategoryId) && Guid.TryParse(filter.CategoryId, out Guid categoryId))
			{
				result = result.Where(s => s.CategoryId == categoryId);
			}

			if (filter.FromPrice.HasValue && filter.ToPrice.HasValue)
			{
				result = result.Where(s => s.Price >= filter.FromPrice.Value && s.Price <= filter.ToPrice);
			}
			if (filter.ToPrice.HasValue && !filter.FromPrice.HasValue)
			{
				result = result.Where(s => s.Price <= filter.ToPrice.Value);
			}
			if (filter.FromPrice.HasValue && !filter.ToPrice.HasValue)
			{
				result = result.Where(s => s.Price >= filter.FromPrice.Value);
			}

			if (!string.IsNullOrEmpty(filter.KeyWord))
			{
				result = result.Where(s => s.Name.Equals(filter.KeyWord, StringComparison.OrdinalIgnoreCase) || s.CategoryName.Equals(filter.KeyWord, StringComparison.OrdinalIgnoreCase));
			}
			if (filter.SortBy.Equals(SortEnum.Price))
			{
				result = result.OrderBy(s => s.Price);
			}
			else
			{
				result = result.OrderBy(s => s.Name);
			}
			productViewModels = result.Skip((filter.PageIndex - 1) * filter.PageSize).Take(filter.PageSize).ToList();

			foreach (var item in productViewModels)
			{
				var image = images.FirstOrDefault(s => s.ProductId == item.Id)?.ImageLink;
				item.Image = string.IsNullOrEmpty(image) ? string.Empty : image;

				var rating = reviews.Where(s => s.ProductId == item.Id).Max(s => s.Rating);
				item.Rating = rating;
			}
			return productViewModels;
		}

		public int InsertReview(ReviewModel model)
		{
			var review = new Review();
			review.Id = new Guid();
			review.Content = model.Content;			
			review.Email = model.Email;
			review.ReviewerName = model.ReviewerName;
			review.Rating = model.Rating;
			review.ProductId = model.ProductId;
			review.CreatedDate = DateTime.Now;
			_context.Reviews.Add(review);
			_context.SaveChanges();
			return 200;
		}
	}
}
