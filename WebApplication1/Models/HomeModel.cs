using System;
using WebApplication1.Context;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class HomeModel
    {
        
        public List<Product> ListProducts { get; set; }
        public List<Category> ListCategory { get; set; }
    }
}
