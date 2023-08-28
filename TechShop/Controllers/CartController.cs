using Infrastructure.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.IdentityModel.Tokens.Jwt;
using TechShop.Models;

namespace TechShop.Controllers
{
    public class CartController:Controller
    {
        private readonly ILogger<CartController> _logger;
        private readonly IProductService _productService;
        public CartController(ILogger<CartController> logger,IProductService productService)
        {

            _logger = logger;
            _productService = productService;

        }

        public IActionResult AddToCart(ProductViewModel product)
        {
            //var rs = "";
            //CartViewModel cart ;
            //if (product == null) return Json("Product is not found"); 
            //else if (product != null)
            //{
            //    cart = new CartViewModel()
            //    {
            //        ProductId = product.Id,
            //        ImageLink = product.Image,
            //        Price = product.Price,
            //        Quantity= product.Quantity
            //    };
            //}
            //if (cart == null)
            //{

            //}

            return View();
        }

    }


    public static class CartExtension
    {
        public static string _key = "cart";
        public static void setCart(this ISession session,string key,List<CartViewModel> lscart)
        {
            session.SetString(key, JsonExtensions.SerializeToJson(lscart));
        }
        public static List<CartViewModel> getCart(this ISession session,string key)
        {
            var rs = JsonExtensions.DeserializeFromJson<List<CartViewModel>>(key);
            return rs;
        }
    }
}
