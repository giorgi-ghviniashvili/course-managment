using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DataAccess.Repositories;
using DataAccess.Interfaces;
using regents_new.Models;

namespace regents_new.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        public readonly IUnitOfWork unitOfWork;

        public CoursesController()
        {
            this.unitOfWork = new UnitOfWork(new DataAccess.AppContext());
        }

        // GET: api/Courses
        [HttpGet]
        public IActionResult Get([FromQuery(Name = "from")] int from = 0, [FromQuery(Name = "to")] int to = 4)
        {
            var quantity = to - from;

            // We should also avoid going too far in the list.
            if (quantity <= 0)
            {
                return BadRequest("You cannot have the 'to' parameter higher than 'from' parameter.");
            }

            if (from < 0)
            {
                return BadRequest("You cannot go in the negative with the 'from' parameter");
            }

            var courses = this.unitOfWork.Courses.GetAll();

            var courseOnPage = courses.Skip(from).Take(quantity).ToArray();
            var total = courses.Count();

            var model = new List<Course>();

            foreach (var item in courseOnPage)
            {
                model.Add(new Course(item));
            }

            return Ok(new {
                Total = total,
                Courses = model
            });
        }

        // GET: api/Courses/5
        [HttpGet("{id}", Name = "GetCourses")]
        public Course Get(int id)
        {
            var course = this.unitOfWork.Courses.Get(id);
            return new Course(course);
        }

        // POST: api/Courses
        [HttpPost]
        public IActionResult Post(Course course)
        {
            var courseEntity = new DataAccess.Entities.Course()
            {
                Description = course.Description
            };

            this.unitOfWork.Courses.Add(courseEntity);
            this.unitOfWork.Complete();

            return Ok(new Course(courseEntity));
        }

        // PUT: api/Courses/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var course = this.unitOfWork.Courses.Get(id);
            var objectAffected = 0;
            if (course != null)
            {
                foreach (var unit in course.Units)
                {
                    foreach (var topic in unit.Topics)
                    {
                        this.unitOfWork.Topics.Remove(topic);
                    }
                    this.unitOfWork.Units.Remove(unit);
                }
                this.unitOfWork.Courses.Remove(course);
                objectAffected = this.unitOfWork.Complete();
            }
            return Ok(objectAffected > 0);
        }
    }
}
