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
       // public List<CartViewModel> cart_cookies = new List<CartViewModel>();
        public CartController(ILogger<CartController> logger,IProductService productService)
        {

            _logger = logger;
            _productService = productService;
            


        }
        public IActionResult AddToCart(Guid productId)
        {
            var product = _productService.GetProductDetails(productId);
            CartViewModel cartItem = new CartViewModel();
            if (product.Id == null) return Json("Product is not found");
            else if (product.Id != null)
            {
                cartItem.ProductId = product.Id;
                cartItem.ImageLink = product.Images[0].ImageLink;
                cartItem.Price = product.DiscountPrice.Value;
                cartItem.Quantity = 1;
                cartItem.ProductName = product.Name;
            }
            var cart_cookies = HttpContext.Session.getCart(CartExtension._key);
            if (cart_cookies == null)
            {
                HttpContext.Session.setCart(CartExtension._key, new List<CartViewModel>() { cartItem });
            }
            else if (cart_cookies != null)
            {
                cart_cookies.Add(cartItem);
                HttpContext.Session.setCart(CartExtension._key, cart_cookies);
            }
            //cart_cookies.Add(cartItem);
            //ViewCartPartial(cart_cookies);
            return Json(200);
        }
        public IActionResult ViewCartPartial()
        {           
            var tmp = HttpContext.Session.getCart(CartExtension._key);
            return PartialView(tmp);
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
            if (session.GetString(key) == null) return null;
            var rs = JsonExtensions.DeserializeFromJson<List<CartViewModel>>(session.GetString(key));
            return rs;
        }
    }
}
