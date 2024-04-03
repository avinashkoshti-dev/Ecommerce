using EcomAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcomAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly EcommerceContext dbcontext;

        public CustomerController(EcommerceContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllCustomer()
        {
            Dictionary<string, object> res = new Dictionary<string, object>();
            var customerlist = await dbcontext.Customers.ToListAsync();
            res["status"] = 1;
            res["obj"] = customerlist;
            return Ok(res);
        }
        [HttpGet]
        public async Task<ActionResult> GetCustomerById(int id)
        {
            Dictionary<string, object> res = new Dictionary<string, object>();
            var customer = await dbcontext.Customers.SingleOrDefaultAsync(x => x.Id == id);
            if(customer==null)
            {
                res["status"] = 0;
                res["message"] = "Customer Not Found";               
            }

            res["status"] = 1;
            res["obj"] = customer!;
            return Ok(res);
        }

        [HttpPost]
        public async Task<ActionResult> CreateCustomer(Customer objcustomer)
        {
            Dictionary<string, object> res = new Dictionary<string, object>();
            if(objcustomer.Id==0)
            {
                dbcontext.Customers.Add(objcustomer);               
                await dbcontext.SaveChangesAsync();
                res["status"] = 1;
                res["message"] = "Customer Created Successfully!";
            }
            else
            {
                if (dbcontext.Customers.Any(x => x.Id == objcustomer.Id))
                {
                    dbcontext.Entry(objcustomer).State = EntityState.Modified;
                    await dbcontext.SaveChangesAsync();
                    res["status"] = 1;
                    res["message"] = "Customer Updated Successfully!";
                }
                else
                {
                    res["status"] = 0;
                    res["message"] = "Customer Not Found!";
                }
            }
            return Ok(res);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteCustomer(int id)
        {
            Dictionary<string, object> res = new Dictionary<string, object>();
            var customer = await dbcontext.Customers.FirstOrDefaultAsync(x => x.Id == id);
            if (customer==null)
            {
                res["status"] = 0;
                res["message"] = "Customer not found!";
            }
            else
            {
                dbcontext.Customers.Remove(customer);
                await dbcontext.SaveChangesAsync();
                res["status"] = 1;
                res["message"] = "Customer Deleted Successfully!";
            }
            return Ok(res);
        }       
    }
}
