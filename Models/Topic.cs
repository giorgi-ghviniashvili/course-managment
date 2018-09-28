using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace regents_new.Models
{
    public class Topic
    {
        public Int32 Id { get; set; }

        public string Description { get; set; }

        public Int32 UnitId { get; set; }

        public Topic () { }

        public Topic (DataAccess.Entities.Topic topic)
        {
            this.Id = topic.Id;
            this.Description = topic.Description;
            this.UnitId = topic.UnitId;
        }
    }
}