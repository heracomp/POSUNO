using System;
using System.Collections.Generic;
using System.Text;

namespace POSUNO.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public float stock { get; set; }
        public bool IsActive { get; set; }
        public User User { get; set; }
    }
}
