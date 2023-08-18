using Infrastructure.Categories;
using Infrastructure.Enums;
using Infrastructure.Products;
using Microsoft.AspNetCore.Mvc;
using TechShop.Models;


namespace TechShop.Controllers
{
	public class ProductController : Controller
	{
		private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
	

        public ProductController(IProductService productService, ICategoryService categoryService)
		{
			_productService = productService;
            _categoryService = categoryService;
        }
		
		public IActionResult Index(string categoryId, string keyWord)
		{
			var model = new ProductIndexModel();
			model.Categories = _categoryService.GetCategories();
			model.SelectPageSize = new List<int> { 9, 18, 27, 36 };
			model.OrderBys = new List<ProductOrderByModel>
			{
			new ProductOrderByModel { Name = SortEnum.Name.ToString(),  Value = (int)SortEnum.Name },
			new ProductOrderByModel { Name = SortEnum.Price.ToString(),  Value = (int)SortEnum.Price }
			};

			model.CategoryId = !string.IsNullOrEmpty(categoryId)? categoryId : string.Empty;
			model.KeyWord = !string.IsNullOrEmpty(keyWord) ? keyWord : string.Empty;
			
			return View(model);
		}

		public IActionResult Detail()
		{
			var pid = "199B24A3-A283-44F8-8618-005CDD4747F2";
			var product = _productService.GetProductDetails(Guid.Parse(pid));	
			
			return View(product);
		}

		public IActionResult ProductListPartial([FromBody] ProductFilterModel model)
		{
			var result = _productService.SearchProduct(model);
			return PartialView(result);
		}
		public IActionResult ReviewListPartial(bool _bool)
		{
			
			var model = new ReviewFilterModel();
			var rs = new List<ReviewModel>();
			if (_bool == false)
			{
				 rs = _productService.GetReviewModels(Guid.Parse("199B24A3-A283-44F8-8618-005CDD4747F2"), model);
			}
			else
			{
				rs = _productService.GetReviewModels(Guid.Parse("199B24A3-A283-44F8-8618-005CDD4747F2"));

			}
			return PartialView(rs);
		}
		public IActionResult InsertReviews([FromBody] ReviewModel model)
		{
			_productService.InsertReview(model);
			return Detail();
		}

	}
}
