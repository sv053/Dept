using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dept.Model
{
    class speciality
    {
        public int Id;
        public string Name{set; get;}
        public int GroupAmount { set; get; }
        public List<string> Students { set; get; }
        public List<string> Teachers { set; get; }

        public List<string> Subjects { set; get; }

        public speciality(string name, int id, int groupamount = 0, List<string> students = null, 
             List<string> subject = null, List<string> teachers = null)
        {
            Name = name;
            GroupAmount = groupamount;
            Students = students;
            Teachers = teachers;
            Id = id;
            Subjects = subject;
        }
    }
}
