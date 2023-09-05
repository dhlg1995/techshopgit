using System.Drawing;

namespace TechShop.Models
{
    public class CartViewModel
    {
        public Guid ProductId { get; set; }
        public string ImageLink { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string ProductName { get; set; }

    }
}
