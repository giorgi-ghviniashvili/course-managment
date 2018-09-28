using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace regents_new.Models
{
    public class Unit
    {
        public Int32 Id { get; set; }

        public Int32 Sequence { get; set; }

        public String Description { get; set; }

        public Int32 CourseId { get; set; }

        public Unit () { }

        public Unit(DataAccess.Entities.Unit unit)
        {
            this.Id = unit.Id;
            this.Sequence = unit.Sequence;
            this.Description = unit.Description;
            this.CourseId = unit.CourseId;
        }
    }
}
