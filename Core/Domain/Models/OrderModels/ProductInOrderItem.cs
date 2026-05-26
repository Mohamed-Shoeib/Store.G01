namespace Domain.Models.OrderModels
{
    public class ProductInOrderItem
    {
        public ProductInOrderItem()
        {
            
        }
        public ProductInOrderItem(int productId, string productName, string pictureUrl)
        {
            ProductId = productId;
            ProductName = productName;
            PictureUrl = pictureUrl;
        }

        // Id
        // Product Id
        public int ProductId { get; set; }

        // Product Name
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
    }
}