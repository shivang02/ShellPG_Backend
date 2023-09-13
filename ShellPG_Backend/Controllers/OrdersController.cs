using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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

        //private int GetCurrentUserId()
        //{

        //    //var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IjEiLCJuYmYiOjE2OTQ1OTgwMjMsImV4cCI6MTY5NTIwMjgyMywiaWF0IjoxNjk0NTk4MDIzfQ.Qp5068WwsdXTfcqQGnQnMhcXJzuytIZbaq8zYwD0o0Y";
        //    var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        //    // Then you can use the JwtSecurityTokenHandler class to read the token
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var tokenData = tokenHandler.ReadJwtToken(token);
        //    var userId = tokenData.Claims.First(claim => claim.Type == ClaimTypes.Name).Value;
        //    return int.Parse(userId);

        //var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");



        //// Then you can use the JwtSecurityTokenHandler class to read the token
        //var tokenHandler = new JwtSecurityTokenHandler();
        //    var tokenData = tokenHandler.ReadJwtToken(token);
        //    var userId = tokenData.Claims.First(claim => claim.Type == "UserId").Value;
        //    return int.Parse(userId);
            
        //}

        private int GetCurrentUserId(string jwtToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenData = tokenHandler.ReadJwtToken(jwtToken);
            var userId = tokenData.Claims.First(claim => claim.Type == ClaimTypes.Name).Value;
            return int.Parse(userId);
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

        //// POST: api/Orders
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Order>> PostOrder(Order order)
        //{
        //  if (_context.Orders == null)
        //  {
        //      return Problem("Entity set 'ApplicationDbContext.Orders'  is null.");
        //  }
        //    _context.Orders.Add(order);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetOrder", new { id = order.Id }, order);
        //}

        // POST: api/Orders
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderRequestModel orderRequest)
        {
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
                
            };

            foreach (var productItem in orderRequest.Order)
            {
                var product = new Product
                {
                    Name = productItem.ProductName,
                    Quantity = productItem.Quantity,
                    Price = productItem.Price,
                    // Map other product properties here as needed
                };

                order.Products.Add(product);
            }

            // Add the order to the context and save changes
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

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
