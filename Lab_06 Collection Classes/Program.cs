using Lab_06_Collection_Classes.Services;

namespace Lab_06_Collection_Classes
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\n=== Lab 06: Collection Classes ===");
                Console.WriteLine("1. Student Records (List<Student>)");
                Console.WriteLine("2. Shopping Cart (List<Cart>)");
                Console.WriteLine("3. Grade Book (Dictionary<int, StudentGrade>)");
                Console.WriteLine("4. Product Catalog (Dictionary<string, Product>)");
                Console.WriteLine("5. Role Based Permission System (HashSet<string>)");
                Console.WriteLine("6. Social Network (Graph & Friend Connections)");
                Console.WriteLine("0. Exit");
                Console.Write("Enter your choice: ");

                string? choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        StudentMethods.Menu();
                        break;
                    case "2":
                        CartMethods.Menu();
                        break;
                    case "3":
                        GradeBookMethods.Menu();
                        break;
                    case "4":
                        ProductMethods.Menu();
                        break;
                    case "5":
                        PermissionMethods.Menu();
                        break;
                    case "6":
                        SocialNetworkMethods.Menu();
                        break;
                    case "0":
                        Console.WriteLine("Exiting Lab 06.");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }
    }
}
