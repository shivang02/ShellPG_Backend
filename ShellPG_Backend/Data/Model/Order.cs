namespace ShellPG_Backend.Data.Model
{
    public class Order
    {
        public Order()
        {
            //Products = new List<Product>(); // Initialize the Products collection in the constructor
            ProductIds = new int[0]; // Initialize the ProductIds array in the constructor
        }

        public int Id { get; set; }

        // Foreign key for the User
        public int UserId { get; set; }

        // Navigation property to the User
        public virtual User User { get; set; }

        // Foreign key for the Product, an integer array
        public int[] ProductIds { get; set; }

        // public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }

        // product array for the Product
        //public virtual List<Product> Products { get; set; }
    }

}
