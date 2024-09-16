using Marten.Schema;

namespace Basket.Api.Models
{
    public class ShoppingCard
    {
        public string Username { get; set; }
        public List<ShoppingCardItem> Items { get; set; } = [];
        public decimal TotalPrice => Items.Sum(s => s.Price * s.Quantity);

        public ShoppingCard(string username) => this.Username = username;

        public ShoppingCard() { }
    }
}
