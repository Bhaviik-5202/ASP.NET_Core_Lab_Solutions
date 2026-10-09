using Lab_06_Collection_Classes.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lab_06_Collection_Classes.Services
{
    internal class GradeBookMethods
    {
        /* 3. Create a Student Grade Book using Dictionary<int, Student> 
         * where each student record contains subject-wise marks. 
         * Implement features to add/update student details, calculate percentages, and display reports.
        */
        static Dictionary<int, StudentGrade> students = new Dictionary<int, StudentGrade>();

        public static void Menu()
        {
            int choice = 0;
            do
            {
                Console.WriteLine("\nSTUDENT GRADE BOOK");
                Console.WriteLine("1. Add Student");
                Console.WriteLine("2. Update Student");
                Console.WriteLine("3. Search Student");
                Console.WriteLine("4. Display All Students");
                Console.WriteLine("5. Display Report");
                Console.WriteLine("6. Exit");
                Console.Write("Enter Choice : ");
                choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        AddStudent();
                        break;
                    case 2:
                        UpdateStudent();
                        break;
                    case 3:
                        SearchStudent();
                        break;
                    case 4:
                        DisplayStudents();
                        break;
                    case 5:
                        DisplayReport();
                        break;
                    case 6:
                        Console.WriteLine("Thank You!");
                        return;
                    default:
                        Console.WriteLine("Please, Enter Valid Choice.");
                        break;
                }
            } while (choice != 6);
        }

        static void AddStudent()
        {
            Console.Write("Enter Student ID : ");
            int id = Convert.ToInt32(Console.ReadLine());
            if (students.ContainsKey(id))
            {
                Console.WriteLine("Student ID already exists!");
                return;
            }
            Console.Write("Enter Student Name : ");
            string Name = Console.ReadLine() ?? string.Empty;
            Console.Write("Enter CPP Marks : ");
            int CPP = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter Applitude Marks : ");
            int Applitude = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter SQL Marks : ");
            int SQL = Convert.ToInt32(Console.ReadLine());

            students.Add(id, new StudentGrade(id, Name, CPP, Applitude, SQL));
            Console.WriteLine("Student Added Successfully.");
        }

        static void UpdateStudent()
        {
            Console.Write("Enter Student ID : ");
            int id = Convert.ToInt32(Console.ReadLine());
            if (students.ContainsKey(id))
            {
                Console.Write("Enter New CPP Marks: ");
                students[id].CPP = Convert.ToInt32(Console.ReadLine());
                Console.Write("Enter New Applitude Marks: ");
                students[id].Applitude = Convert.ToInt32(Console.ReadLine());
                Console.Write("Enter New SQL Marks: ");
                students[id].SQL = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Marks Updated Successfully!");
            }
            else
            {
                Console.WriteLine("Student Not Found!");
            }
        }

        static void SearchStudent()
        {
            Console.Write("Enter Student ID : ");
            int id = Convert.ToInt32(Console.ReadLine());

            if (students.ContainsKey(id))
            {
                StudentGrade s = students[id];

                Console.WriteLine("\nID : " + s.Id);
                Console.WriteLine("Name : " + s.Name);
                Console.WriteLine("CPP : " + s.CPP);
                Console.WriteLine("Applitude : " + s.Applitude);
                Console.WriteLine("SQL : " + s.SQL);
            }
            else
            {
                Console.WriteLine("Student Not Found.");
            }
        }

        static void DisplayStudents()
        {
            if (students.Count == 0)
            {
                Console.WriteLine("No Records Found.");
                return;
            }

            Console.WriteLine("=============================================================");
            Console.WriteLine("ID\tName\tCPP\tApplitude\tSQL");
            foreach (var student in students.Values)
            {
                Console.WriteLine($"{student.Id}\t{student.Name}\t{student.SQL}\t{student.Applitude}\t\t{student.SQL}");
            }
        }

        static void DisplayReport()
        {
            if (students.Count == 0)
            {
                Console.WriteLine("No Records Found.");
                return;
            }

            Console.WriteLine("==============================");
            Console.WriteLine("ID\tName\tTotal\tPercentage");

            foreach (KeyValuePair<int, StudentGrade> item in students)
            {
                StudentGrade s = item.Value;

                int total = s.CPP + s.Applitude + s.SQL;
                double percentage = total / 3.0;

                Console.WriteLine($"{s.Id}\t{s.Name}\t{total}\t{percentage:F2}%");
            }
        }
    }
}
