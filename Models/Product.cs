using System;

namespace webapi2.Models
{
    public class Product 
    {
        public Guid Id {get;set;} 
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public Guid CategoryId { get; set;}

        /*
        public decimal SellingPrice { get; set; }

        public Guid BrandId { get; set; }
        public Guid CategoryId  { get; set;}
        public string Model { get; set;}

        public string Icon { get; set; }

        public int UnitsInStock { get; set; }
        public bool IsDiscontinued { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        */
    }    
}
