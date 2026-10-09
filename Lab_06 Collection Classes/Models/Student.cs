using System;
using System.Collections.Generic;
using System.Text;

namespace Lab_06_Collection_Classes.Models
{
    internal class Student
    {
        public int id;
        public string name;
        public int age;

        public Student(int id, string name, int age)
        {
            this.id = id;
            this.name = name;
            this.age = age;
        }
    }
}
