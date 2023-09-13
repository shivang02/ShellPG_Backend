namespace ShellPG_Backend.Data.Model
{
    public class Order
    {
        public int Id { get; set; }
        public int Email { get; set; }
        public int ProductId { get; set;}
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
