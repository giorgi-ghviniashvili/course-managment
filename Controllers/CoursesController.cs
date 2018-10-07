using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DataAccess.Repositories;
using DataAccess.Interfaces;
using regents_new.Models;
using AutoMapper;

namespace regents_new.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        public readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public CoursesController(IMapper mapper)
        {
            this.unitOfWork = new UnitOfWork(new DataAccess.AppContext());
            this.mapper = mapper;
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

            var courseOnPage = this.unitOfWork.Courses.GetCoursePage(from, quantity);

            List<Course> model = mapper.Map<IEnumerable<DataAccess.Entities.Course>, List<Course>>(courseOnPage.PageItems);

            return Ok(new {
                courseOnPage.Total,
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

        // PUT: api/Courses
        [HttpPut]
        public IActionResult Put(Course course)
        {
            var result = 0;
            var courseDb = this.unitOfWork.Courses.SingleOrDefault(x => x.Id == course.Id);
            if (courseDb != null)
            {
                if (courseDb.Description != course.Description)
                {
                    courseDb.Description = course.Description;
                    result = this.unitOfWork.Complete();
                }
            }
            return Ok(result > 0);
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var course = this.unitOfWork.Courses.Get(id);
            var objectAffected = 0;
            if (course != null)
            {
                var units = this.unitOfWork.Units.GetByCourseId(course.Id);
                foreach (var unit in units)
                {
                    var topics = this.unitOfWork.Topics.GetByUnitId(unit.Id);
                    foreach (var topic in topics)
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
