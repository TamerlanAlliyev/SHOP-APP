using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicShop.Models
{
    public class Product : BaseModel
    {
        public string Name { get; set; } = null!;
        public double Price { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        //public Product()
        //{
        //    CreatedAt = DateTime.UtcNow;
        //    UpdatedAt = DateTime.UtcNow;
        //}
    }

}
