using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TRMDataManager.Library.DataAccess;
using TRMDataManager.Library.Models;

namespace TRMDataManager.Controllers
{
    // Allow what roles can access data
    [Authorize(Roles = "Cashier")]
    public class ProductController : ApiController
    {
        public List<ProductModel> Get()
        {
            // Check userId
            ProductData data = new ProductData();
            return data.GetProducts();
        }
    }
}
