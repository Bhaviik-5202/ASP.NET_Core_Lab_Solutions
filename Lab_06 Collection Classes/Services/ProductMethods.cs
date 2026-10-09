using Lab_06_Collection_Classes.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lab_06_Collection_Classes.Services
{
    internal class ProductMethods
    {
        /* 4. Develop a Product Catalog and Inventory Management System using 
         * Dictionary<string, Product> with features for 
         * adding products, updating stock, searching, and generating bills.
         */
        static Dictionary<string, Product> products = new Dictionary<string, Product>();
        public static void Menu()
        {
            int choice;
            do
            {
                Console.WriteLine("PRODUCT CATALOG SYSTEM");
                Console.WriteLine("1. Add Product");
                Console.WriteLine("2. Update Stock");
                Console.WriteLine("3. Search Product");
                Console.WriteLine("4. Display Products");
                Console.WriteLine("5. Generate Bill");
                Console.WriteLine("6. Exit");
                Console.Write("Enter Choice : ");
                choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        AddProduct();
                        break;
                    case 2:
                        UpdateStock();
                        break;
                    case 3:
                        SearchProduct();
                        break;
                    case 4:
                        DisplayProducts();
                        break;
                    case 5:
                        GenerateBill();
                        break;
                    case 6:
                        Console.WriteLine("Thank You!");
                        break;
                    default:
                        Console.WriteLine("Invalid Choice. Please Try Again.");
                        break;
                }
            } while (choice != 6);
        }

        static void AddProduct()
        {
            Console.Write("Enter Product ID : ");
            string id = Console.ReadLine() ?? string.Empty;
            Console.Write("Enter Product Name : ");
            string name = Console.ReadLine() ?? string.Empty;
            Console.Write("Enter Product Price : ");
            double price = Convert.ToDouble(Console.ReadLine());
            Console.Write("Enter Stock Quantity : ");
            int stock = Convert.ToInt32(Console.ReadLine());

            products.Add(id, new Product(id, name, price, stock));

            Console.WriteLine("Product Added Successfully.");
        }

        static void UpdateStock()
        {
            Console.Write("Enter Product ID : ");
            string id = Console.ReadLine() ?? string.Empty;

            if (products.ContainsKey(id))
            {
                Console.Write("Enter New Stock : ");
                products[id].Stock = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("Stock Updated Successfully.");
            }
            else
            {
                Console.WriteLine("Product Not Found.");
            }
        }

        static void SearchProduct()
        {
            Console.Write("Enter Product ID : ");
            string id = Console.ReadLine() ?? string.Empty;

            if (products.ContainsKey(id))
            {
                Product p = products[id];

                Console.WriteLine("\nPRODUCT DETAILS");
                Console.WriteLine("ID      : " + p.Id);
                Console.WriteLine("Name    : " + p.Name);
                Console.WriteLine("Price   : " + p.Price);
                Console.WriteLine("Stock   : " + p.Stock);
            }
            else
            {
                Console.WriteLine("Product Not Found.");
            }
        }

        static void DisplayProducts()
        {
            if (products.Count == 0)
            {
                Console.WriteLine("No Products Available.");
                return;
            }

            Console.WriteLine("-------------------------------------------------------");
            Console.WriteLine("ID\tName\t\tPrice\tStock");
            Console.WriteLine("-------------------------------------------------------");

            foreach (Product p in products.Values)
            {
                Console.WriteLine($"{p.Id}\t{p.Name}\t\t{p.Price}\t{p.Stock}");
            }
        }

        static void GenerateBill()
        {
            Console.Write("Enter Product ID : ");
            string id = Console.ReadLine() ?? string.Empty;

            if (!products.ContainsKey(id))
            {
                Console.WriteLine("Product Not Found.");
                return;
            }

            Product p = products[id];

            Console.Write("Enter Quantity : ");
            int qty = Convert.ToInt32(Console.ReadLine());

            if (qty > p.Stock)
            {
                Console.WriteLine("Insufficient Stock.");
                return;
            }

            double amount = qty * p.Price;

            p.Stock -= qty;

            Console.WriteLine("\n========== BILL ==========");
            Console.WriteLine("Product ID   : " + p.Id);
            Console.WriteLine("Product Name : " + p.Name);
            Console.WriteLine("Price        : " + p.Price);
            Console.WriteLine("Quantity     : " + qty);
            Console.WriteLine("--------------------------");
            Console.WriteLine("Total Amount : " + amount);
            Console.WriteLine("==========================");

            Console.WriteLine("Stock Updated Successfully.");
        }
    }
}
