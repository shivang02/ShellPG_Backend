namespace ShellPG_Backend.Data.Model
{
    public class OrderItem
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

    public class OrderRequestModel
    {
        public List<OrderItem> Order { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime Timestamp { get; set; }
    }

}
