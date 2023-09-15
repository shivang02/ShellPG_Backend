using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShellPG_Backend.Data;
using ShellPG_Backend.Data.Model;

namespace ShellPG_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            // Identify the currently logged-in user (you need to implement this part)
            var jwtToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var userId = GetCurrentUserId(jwtToken); // Implement a method to get the user ID

            if (userId == null)
            {
                return Unauthorized(); // User is not authenticated
            }

            // Query the database for orders associated with the user
            var userOrders = await _context.Orders
                .Where(o => o.UserId == userId) // Assuming you have a UserId property in your Order model
                .ToListAsync();

            if (!userOrders.Any())
            {
                return NotFound(); // No orders found for the user
            }

            return userOrders;
        }

        private int GetCurrentUserId(string jwtToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenData = tokenHandler.ReadJwtToken(jwtToken);

            // The claim name in your JWT payload is "unique_name"
            var uniqueNameClaim = tokenData.Claims.FirstOrDefault(claim => claim.Type == "unique_name");

            Console.WriteLine("userIDFF"+ uniqueNameClaim.Value);
            
            return int.Parse(uniqueNameClaim.Value);
    
            throw new InvalidOperationException("User ID not found in JWT token.");
        }



        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
          if (_context.Orders == null)
          {
              return NotFound();
          }
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        
        // POST: api/Orders
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderRequestModel orderRequest)
        {
            if (orderRequest == null)
            {
                return BadRequest("Invalid order request"); // Return a meaningful error response
            }

            var jwtToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var userId = GetCurrentUserId(jwtToken); // Implement a method to get the user ID

            if (userId == null)
            {
                return Unauthorized(); // User is not authenticated
            }

            // Create an Order entity from the request
            var order = new Order
            {
                UserId = userId, // Associate the order with the logged-in user
                OrderDate = DateTime.UtcNow, // Use current timestamp
                TotalPrice = orderRequest.TotalPrice,
                ProductIds = orderRequest.ProductIds
            };


            // Add the order to the context and save changes
            _context.Orders.Add(order); 
            await _context.SaveChangesAsync();

            //loop through the order product ids and update the product quantity in product table

            foreach (var productId in order.ProductIds)
            {
                var product = await _context.Products.FindAsync(productId);
                if (product != null)
                {
                    product.Quantity -= 1;
                    _context.Entry(product).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
            }


            // Return a success response
            return Ok("Order created successfully");
        }



        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return (_context.Orders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
