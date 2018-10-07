using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace regents_new.Models
{
    public class Course
    {
        public Int32 Id { get; set; }

        public string Description { get; set; }

        public List<Unit> Units { get; set; }

        public Course () { }

        public Course (DataAccess.Entities.Course course)
        {
            this.Id = course.Id;
            this.Description = course.Description;

            this.Units = new List<Unit>();
        }
    }
}
