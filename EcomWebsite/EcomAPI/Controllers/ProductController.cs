using EcomAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.ObjectPool;

namespace EcomAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly EcommerceContext dbcontext;

        public ProductController(EcommerceContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllProduct()
        {
            Dictionary<string, object> res = new Dictionary<string, object>();
            //var list = await dbcontext.Products.ToListAsync();
            //var list = await (from product in dbcontext.Products
            //            join sup in dbcontext.Suppliers on product.SupplierId equals sup.Id
            //            select new
            //            {
            //                Id=product.Id,
            //                ProductName=product.ProductName,
            //                SupplierId=product.SupplierId,
            //                UnitPrice=product.UnitPrice,
            //                Package=product.Package,
            //                IsDiscontinued=product.IsDiscontinued,
            //                SuplierName = sup.CompanyName
            //            }).ToListAsync();

            List<ProductModal> list = await dbcontext.Products.Select(x => new ProductModal()
            {
                Id = x.Id,
                IsDiscontinued = x.IsDiscontinued,
                Package = x.Package,
                ProductName = x.ProductName,
                SupplierId = x.SupplierId,
                SupplierName = x.Supplier.CompanyName,
                UnitPrice = x.UnitPrice
            }).ToListAsync();

            res["status"] = 1;
            res["obj"] = list;
            return Ok(res);
        }
        [HttpGet]
        public async Task<ActionResult> GetAllSupplier()
        {
            Dictionary<string, object> res = new Dictionary<string, object>();
            var list = await dbcontext.Suppliers.Select(x => new Supplier()
            {
                Id = x.Id,
                CompanyName = x.CompanyName
            }).ToListAsync();
            
            res["status"] = 1;
            res["obj"] = list;
            return Ok(res);
        }

        [HttpGet]
        public async Task<ActionResult> GetProductById(int id)
        {
            Dictionary<string, object> res = new Dictionary<string, object>();
            var Product = await dbcontext.Products.SingleOrDefaultAsync(x => x.Id == id);
            if (Product == null)
            {
                res["status"] = 0;
                res["message"] = "Product Not Found";
            }

            res["status"] = 1;
            res["obj"] = Product!;
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct1([FromBody] Product product)
        {
            Dictionary<string, object> res = new Dictionary<string, object>();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            dbcontext.Products.Add(product);
            await dbcontext.SaveChangesAsync();

            res["status"] = 1;
            res["message"] = "Product Created Successfully!";
            return Ok(res);
        }


        [HttpPost]
        public async Task<ActionResult> CreateProduct(ProductVM objProduct)
        {
            Dictionary<string, object> res = new Dictionary<string, object>();
            if (objProduct.Id == 0)
            {
                try
                {
                    Product obj = new Product();
                    obj.IsDiscontinued = objProduct.IsDiscontinued;
                    obj.SupplierId = objProduct.SupplierId;
                    obj.Package = objProduct.Package;
                    obj.ProductName = objProduct.ProductName;
                    obj.UnitPrice = objProduct.UnitPrice;

                    dbcontext.Products.Add(obj);
                    await dbcontext.SaveChangesAsync();
                }
                catch (Exception ex)
                {

                }
                res["status"] = 1;
                res["message"] = "Product Created Successfully!";
            }
            else
            {
                if (dbcontext.Products.Any(x => x.Id == objProduct.Id))
                {
                    Product obj = new Product();
                    obj.Id = objProduct.Id;
                    obj.IsDiscontinued = objProduct.IsDiscontinued;
                    obj.SupplierId = objProduct.SupplierId;
                    obj.Package = objProduct.Package;
                    obj.ProductName = objProduct.ProductName;
                    obj.UnitPrice = objProduct.UnitPrice;

                    dbcontext.Entry(obj).State = EntityState.Modified;
                    await dbcontext.SaveChangesAsync();
                    res["status"] = 1;
                    res["message"] = "Product Updated Successfully!";
                }
                else
                {
                    res["status"] = 0;
                    res["message"] = "Product Not Found!";
                }
            }
            return Ok(res);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            Dictionary<string, object> res = new Dictionary<string, object>();
            var Product = await dbcontext.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (Product == null)
            {
                res["status"] = 0;
                res["message"] = "Product not found!";
            }
            else
            {
                dbcontext.Products.Remove(Product);
                await dbcontext.SaveChangesAsync();
                res["status"] = 1;
                res["message"] = "Product Deleted Successfully!";
            }
            return Ok(res);
        }
    }
}
