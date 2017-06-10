using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Znachor.Models
{
    public class Product
    {
        public string Name { get; set; }

        public int Cathegory { get; set; }

        public decimal Price { get; set; }

        public string Image { get; set; }
    }
}