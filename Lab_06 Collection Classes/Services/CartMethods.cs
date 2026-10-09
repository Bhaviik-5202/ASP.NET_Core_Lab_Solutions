using Lab_06_Collection_Classes.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lab_06_Collection_Classes.Services
{
    internal class CartMethods
    {
        /* 2. Implement a Shopping Cart system using List that supports adding items, removing items, viewing cart details, 
         * and calculating the total amount with a discount.
         */
        static List<Cart> cart = new List<Cart>();
        public static void Menu()
        {
            int choice = 0;
            do
            {
                Console.WriteLine("\nSHOPPING CART");
                Console.WriteLine("1. Add Item");
                Console.WriteLine("2. Remove Item");
                Console.WriteLine("3. View Cart");
                Console.WriteLine("4. Search Item");
                Console.WriteLine("5. Calculate Total");
                Console.WriteLine("6. Exit");
                Console.Write("Enter Your Choice: ");
                choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        AddItem();
                        break;
                    case 2:
                        RemoveItem();
                        break;
                    case 3:
                        DisplayCart();
                        break;
                    case 4:
                        SearchItem();
                        break;
                    case 5:
                        CalculateTotal();
                        break;
                    case 6:
                        Console.WriteLine("Thank You. (Exit)");
                        return;
                    default:
                        Console.WriteLine("Plese, Enter Valid Choise");
                        break;
                }
            } while (choice != 6);
        }

        static void AddItem()
        {
            Console.Write("Enter Id: ");
            int id = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter Item Name: ");
            string itemName = Console.ReadLine() ?? string.Empty;

            Console.Write("Enter Price: ");
            double price = Convert.ToDouble(Console.ReadLine());

            Console.Write("Enter Quantity: ");
            int quantity = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter Rating (1-5): ");
            int rating = Convert.ToInt32(Console.ReadLine());

            cart.Add(new Cart(id, itemName, price, quantity, rating));

            Console.WriteLine("Item Added Successfully.");
        }
        
        static void RemoveItem()
        {
            Console.Write("Enter Item Id to Remove: ");
            int id = Convert.ToInt32(Console.ReadLine());

            foreach (Cart c in cart)
            {
                if (c.Id == id)
                {
                    cart.Remove(c);
                    Console.WriteLine("Item Removed Successfully.");
                    return;
                }
            }
            Console.WriteLine("Item Not Found.");
        }

        static void DisplayCart()
        {
            if (cart.Count == 0)
            {
                Console.WriteLine("Cart is Empty.");
                return;
            }

            Console.WriteLine("ID\tName\tPrice\tQty\tRating\tAmount");
            foreach (Cart c in cart)
            {
                Console.WriteLine($"{c.Id}\t{c.ItemName}\t{c.Price}\t{c.Quantity}\t{c.Rating}\t{c.Price * c.Quantity}");
            }
        }

        static void SearchItem()
        {
            Console.Write("Enter Item Id: ");
            int id = Convert.ToInt32(Console.ReadLine());

            foreach (Cart c in cart)
            {
                if (c.Id == id)
                {
                    Console.WriteLine("\nItem Found");
                    Console.WriteLine("ID       : " + c.Id);
                    Console.WriteLine("Name     : " + c.ItemName);
                    Console.WriteLine("Price    : " + c.Price);
                    Console.WriteLine("Quantity : " + c.Quantity);
                    Console.WriteLine("Rating   : " + c.Rating);
                    return;
                }
            }
            Console.WriteLine("Item Not Found.");
        }

        static void CalculateTotal()
        {
            double total = 0;
            foreach (Cart c in cart)
            {
                total += c.Price * c.Quantity;
            }
            Console.WriteLine("\nTotal Amount : " + total);

            double discount = 0;
            if (total >= 5000)
                discount = total * 0.20;
            else if (total >= 3000)
                discount = total * 0.15;
            else if (total >= 1000)
                discount = total * 0.10;

            Console.WriteLine("Discount : " + discount);
            Console.WriteLine("Final Amount : " + (total - discount));
        }
    }
}
