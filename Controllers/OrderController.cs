using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BangazonAPI.Data;
using BangazonAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BangazonAPI.Controllers
{
    [Route("[controller]")]
    public class OrderController : Controller
    {
        private BangazonAPIContext _context;
        public OrderController(BangazonAPIContext ctx)
        {
            _context = ctx;
        }

        // GET url/Order
        // Gets a list of all orders -- Eliza
        [HttpGet]
        public IActionResult Get()
        {
            IQueryable<object> orders = _context.Order.Include("ProductOrders.Product");

            if (orders == null)
            {
                return NotFound();
            }

            return Ok(orders);

        }

        // GET url/Order/{id}
        // Gets one order based on an id -- Ben
        // Formats it for the purposes of clean JSON
        [HttpGet("{id}", Name = "GetSingleOrder")]
        public IActionResult Get([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Order order = _context.Order.Include("ProductOrders.Product").Single(m => m.OrderID == id);
                List <Product> theseProducts = new List <Product>();

                foreach (ProductOrder productOrder in order.ProductOrders)
                {
                    theseProducts.Add(productOrder.Product);
                }
                List <ProductJSON> jSONProducts = new List <ProductJSON>();
                foreach (Product product in theseProducts) {
                    int index = jSONProducts.FindIndex(x => x.ProductID == product.ProductID);
                    if (index != -1) 
                    {
                        jSONProducts[index].Quantity ++;
                    } 
                    else 
                    {
                        ProductJSON newProduct = new ProductJSON()
                        {
                            ProductID = product.ProductID,
                            Name = product.Title,
                            Price = product.Price,
                            Quantity = 1
                        };
                        jSONProducts.Add(newProduct);
                    }
                }
                OrderJSON orderWithProducts = new OrderJSON()
                {
                    OrderID = order.OrderID,
                    CustomerID = order.CustomerID,
                    PaymentTypeID = order.PaymentTypeID,
                    Products = jSONProducts
                };
                if (order == null)
                {
                    return NotFound();
                }
                
                return Ok(orderWithProducts);
            }
            catch (System.InvalidOperationException ex)
            {
                return NotFound(ex);
            }
        }

        // POST url/Order
        // Posts a new order -- Eliza
        [HttpPost]
        public IActionResult Post([FromBody] Order newOrder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.Order.Add(newOrder); 
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (OrderExists(newOrder.OrderID))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }
            return CreatedAtRoute("GetSingleOrder", new { id = newOrder.OrderID }, newOrder);
        }

        [HttpPost("addproduct")]
        public IActionResult Post([FromBody] ProductOrder newProductOrder )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.ProductOrder.Add(newProductOrder);
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (OrderExists(newProductOrder.ProductOrderID))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }
            return Ok(newProductOrder);
        }

        private bool OrderExists(int orderID)
        {
          return _context.Order.Count(e => e.OrderID == orderID) > 0;
        }

        // PUT url/Order/5
        // Edits something in the database; you must send the ENTIRE object up. -- Eliza
        // Requires an Object:
        // {
        //     "OrderID": 1,
        //     "CustomerId": 1,
        //     "DateCreated": "0001-01-01T00:00:00",
        //     "PaymentTypeID": 1
        // }

        

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Order modifiedOrder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != modifiedOrder.OrderID)
            {
                return BadRequest();
            }

            _context.Entry(modifiedOrder).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
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

            return new StatusCodeResult(StatusCodes.Status204NoContent);
        }

        // DELETE url/Order/5
        // Deletes something based on an id. -- Eliza
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Order singleOrder = _context.Order.Single(m => m.OrderID == id);
            if (singleOrder == null)
            {
                return NotFound();
            }

            _context.Order.Remove(singleOrder);
            _context.SaveChanges();

            return Ok(singleOrder);
        }

    }
}