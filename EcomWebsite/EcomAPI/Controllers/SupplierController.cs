using EcomAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcomAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly EcommerceContext dbcontext;

        public SupplierController(EcommerceContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllSupplier()
        {
            Dictionary<string, object> res = new Dictionary<string, object>();
            var list = await dbcontext.Suppliers.ToListAsync();
            res["status"] = 1;
            res["obj"] = list;
            return Ok(res);
        }
        [HttpGet]
        public async Task<ActionResult> GetSupplierById(int id)
        {
            Dictionary<string, object> res = new Dictionary<string, object>();
            var supplier = await dbcontext.Suppliers.SingleOrDefaultAsync(x => x.Id == id);
            if (supplier == null)
            {
                res["status"] = 0;
                res["message"] = "Supplier Not Found";
            }

            res["status"] = 1;
            res["obj"] = supplier!;
            return Ok(res);
        }

        [HttpPost]
        public async Task<ActionResult> CreateSupplier(Supplier objsupplier)
        {
            Dictionary<string, object> res = new Dictionary<string, object>();
            if (objsupplier.Id == 0)
            {
                dbcontext.Suppliers.Add(objsupplier);
                await dbcontext.SaveChangesAsync();
                res["status"] = 1;
                res["message"] = "Supplier Created Successfully!";
            }
            else
            {
                if (dbcontext.Suppliers.Any(x => x.Id == objsupplier.Id))
                {
                    dbcontext.Entry(objsupplier).State = EntityState.Modified;
                    await dbcontext.SaveChangesAsync();
                    res["status"] = 1;
                    res["message"] = "Supplier Updated Successfully!";
                }
                else
                {
                    res["status"] = 0;
                    res["message"] = "Supplier Not Found!";
                }
            }
            return Ok(res);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteSupplier(int id)
        {
            Dictionary<string, object> res = new Dictionary<string, object>();
            var supplier = await dbcontext.Suppliers.FirstOrDefaultAsync(x => x.Id == id);
            if (supplier == null)
            {
                res["status"] = 0;
                res["message"] = "Supplier not found!";
            }
            else
            {
                dbcontext.Suppliers.Remove(supplier);
                await dbcontext.SaveChangesAsync();
                res["status"] = 1;
                res["message"] = "Supplier Deleted Successfully!";
            }
            return Ok(res);
        }
    }
}
