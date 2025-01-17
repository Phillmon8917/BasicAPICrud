﻿namespace CRUDAPI_Practice.Models
{
    //This will contain data from the database
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string Brand { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public Decimal Price { get; set; }

        public string Description { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}
