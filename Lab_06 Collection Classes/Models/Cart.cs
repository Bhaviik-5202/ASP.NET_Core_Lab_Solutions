using System;
using System.Collections.Generic;
using System.Text;

namespace Lab_06_Collection_Classes.Models
{
    internal class Cart
    {
        public int Id;
        public string ItemName;
        public double Price;
        public int Quantity;
        public int Rating;

        public Cart(int id, string itemName, double price, int quantity, int rating)
        {
            Id = id;
            ItemName = itemName;
            Price = price;
            Quantity = quantity;
            Rating = rating;
        }
    }
}
