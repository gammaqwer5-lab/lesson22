using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp14
{
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; } = null!;
    }
}
