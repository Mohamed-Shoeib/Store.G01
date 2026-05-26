namespace Domain.Models.OrderModels
{
    public class OrderItem : BaseEntity<Guid>
    {
        public OrderItem()
        {
            
        }
        public OrderItem(ProductInOrderItem productInOrderItem, decimal price, int quantity)
        {
            Product = productInOrderItem;
            Price = price;
            Quantity = quantity;
        }

        public ProductInOrderItem Product  { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}