using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace regents_new.Models
{
    [Serializable]
    public class Unit
    {
        public Int32 Id { get; set; }

        public Int32 Sequence { get; set; }

        public String Description { get; set; }

        public Int32 CourseId { get; set; }

        public String CourseName { get; set; }

        public List<Topic> Topics { get; set; }

        public Unit () { }

        public Unit(DataAccess.Entities.Unit unit)
        {
            this.Id = unit.Id;
            this.Sequence = unit.Sequence;
            this.Description = unit.Description;
            this.CourseId = unit.CourseId;
            this.CourseName = unit.Course.Description;

            this.Topics = new List<Topic>();
        }
    }
}
