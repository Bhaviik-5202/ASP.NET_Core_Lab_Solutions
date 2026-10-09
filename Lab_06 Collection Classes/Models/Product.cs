using System;
using System.Collections.Generic;
using System.Text;

namespace Lab_06_Collection_Classes.Models
{
    internal class Product
    {
        public string Id;
        public string Name;
        public double Price;
        public int Stock;

        public Product(string id, string name, double price, int stock)
        {
            Id = id;
            Name = name;
            Price = price;
            Stock = stock;
        }
    }
}
