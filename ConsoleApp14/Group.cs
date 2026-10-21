using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp14
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
