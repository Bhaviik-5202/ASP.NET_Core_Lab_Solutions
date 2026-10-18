using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab_11_LINQ_Part_3_Join
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public string? DepartmentCode { get; set; }
    }

    public class Student
    {
        public int StudentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string EnrollmentNumber { get; set; } = string.Empty;
        public int? DepartmentId { get; set; }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=================================================");
            Console.WriteLine("     LINQ Part 3 - Joins Practical Execution     ");
            Console.WriteLine("=================================================\n");

            var departments = new List<Department>
            {
                new Department { DepartmentId = 1, DepartmentName = "Computer Engineering", DepartmentCode = "CE" },
                new Department { DepartmentId = 2, DepartmentName = "Information Technology", DepartmentCode = "IT" },
                new Department { DepartmentId = 3, DepartmentName = "Mechanical Engineering", DepartmentCode = "ME" },
                new Department { DepartmentId = 4, DepartmentName = "Civil Engineering", DepartmentCode = "CL" }
            };

            var students = new List<Student>
            {
                new Student { StudentId = 101, Name = "Bhavik Parmar", EnrollmentNumber = "21001", DepartmentId = 1 },
                new Student { StudentId = 102, Name = "Rahul Sharma", EnrollmentNumber = "21002", DepartmentId = 2 },
                new Student { StudentId = 103, Name = "Amit Patel", EnrollmentNumber = "21003", DepartmentId = 1 },
                new Student { StudentId = 104, Name = "Neha Mehta", EnrollmentNumber = "21004", DepartmentId = 3 },
                new Student { StudentId = 105, Name = "Pooja Shah", EnrollmentNumber = "21005", DepartmentId = null }
            };

            Console.WriteLine("--- 1. INNER JOIN (Join) ---");
            Console.WriteLine("Only students with matching departments are shown:\n");

            var innerJoin = students.Join(
                departments,
                s => s.DepartmentId,
                d => d.DepartmentId,
                (s, d) => new
                {
                    s.StudentId,
                    StudentName = s.Name,
                    s.EnrollmentNumber,
                    DepartmentName = d.DepartmentName,
                    d.DepartmentCode
                });

            foreach (var item in innerJoin)
            {
                Console.WriteLine($"[Student #{item.StudentId}] {item.StudentName} ({item.EnrollmentNumber}) -> {item.DepartmentName} [{item.DepartmentCode}]");
            }
            Console.WriteLine();

            Console.WriteLine("--- 2. LEFT OUTER JOIN (All Students + Matching Department) ---");
            Console.WriteLine("All students are returned; missing departments show 'No Department':\n");

            var leftOuterJoin = students.GroupJoin(
                departments,
                s => s.DepartmentId,
                d => d.DepartmentId,
                (s, deptGroup) => new { Student = s, DeptGroup = deptGroup })
                .SelectMany(
                    x => x.DeptGroup.DefaultIfEmpty(),
                    (x, dept) => new
                    {
                        x.Student.StudentId,
                        StudentName = x.Student.Name,
                        x.Student.EnrollmentNumber,
                        DepartmentName = dept != null ? dept.DepartmentName : "No Department Assigned",
                        DepartmentCode = dept != null ? dept.DepartmentCode : "N/A"
                    });

            foreach (var item in leftOuterJoin)
            {
                Console.WriteLine($"[Student #{item.StudentId}] {item.StudentName} -> {item.DepartmentName} ({item.DepartmentCode})");
            }
            Console.WriteLine();

            Console.WriteLine("--- 3. RIGHT OUTER JOIN (All Departments + Matching Students) ---");
            Console.WriteLine("All departments are returned; missing students show 'No Student':\n");

            var rightOuterJoin = departments.GroupJoin(
                students,
                d => d.DepartmentId,
                s => s.DepartmentId,
                (d, studentGroup) => new { Department = d, StudentGroup = studentGroup })
                .SelectMany(
                    x => x.StudentGroup.DefaultIfEmpty(),
                    (x, student) => new
                    {
                        DepartmentName = x.Department.DepartmentName,
                        DepartmentCode = x.Department.DepartmentCode,
                        StudentName = student != null ? student.Name : "No Student Enrolled",
                        EnrollmentNumber = student != null ? student.EnrollmentNumber : "N/A"
                    });

            foreach (var item in rightOuterJoin)
            {
                Console.WriteLine($"[{item.DepartmentCode}] {item.DepartmentName} -> {item.StudentName} ({item.EnrollmentNumber})");
            }
            Console.WriteLine();

            Console.WriteLine("--- 4. GROUP JOIN (Hierarchical Grouping) ---");
            Console.WriteLine("Groups each department with its corresponding enrolled students:\n");

            var groupJoin = departments.GroupJoin(
                students,
                d => d.DepartmentId,
                s => s.DepartmentId,
                (dept, enrolledStudents) => new
                {
                    dept.DepartmentName,
                    dept.DepartmentCode,
                    Students = enrolledStudents.ToList()
                });

            foreach (var group in groupJoin)
            {
                Console.WriteLine($"Department: {group.DepartmentName} ({group.DepartmentCode}) - Total Students: {group.Students.Count}");
                foreach (var s in group.Students)
                {
                    Console.WriteLine($"   * [{s.StudentId}] {s.Name} ({s.EnrollmentNumber})");
                }
                if (group.Students.Count == 0)
                {
                    Console.WriteLine("   * (No students registered)");
                }
            }
            Console.WriteLine();

            Console.WriteLine("--- 5. CROSS JOIN (Cartesian Product) ---");
            var crossJoin = students.Take(2).SelectMany(
                s => departments.Take(2),
                (s, d) => new
                {
                    Student = s.Name,
                    Department = d.DepartmentName
                });

            Console.WriteLine("Sample combinations (Student x Department):");
            foreach (var item in crossJoin)
            {
                Console.WriteLine($"   * {item.Student} paired with {item.Department}");
            }

            Console.WriteLine("\n=================================================");
            Console.WriteLine("     LINQ Joins Completed Successfully!          ");
            Console.WriteLine("=================================================");
        }
    }
}
