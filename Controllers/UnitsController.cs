using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DataAccess.Interfaces;
using DataAccess.Repositories;
using regents_new.Models;
using AutoMapper;

namespace regents_new.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitsController : ControllerBase
    {
        public readonly IUnitOfWork unitOfWork;
        public readonly IMapper mapper;

        public UnitsController(IMapper mapper)
        {
            this.unitOfWork = new UnitOfWork(new DataAccess.AppContext());
            this.mapper = mapper;
        }

        // GET: api/Units
        [HttpGet]
        public IActionResult Get()
        {
            var units = this.unitOfWork.Units.GetAll();

            var total = units.Count();

            List<Unit> model = mapper.Map<IEnumerable<DataAccess.Entities.Unit>, List<Unit>>(units);

            return Ok(model);
        }

        // GET: api/Units/5
        [HttpGet("{id}", Name = "GetUnits")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Units
        [HttpPost]
        public IActionResult Post(Unit unit)
        {
            var unitEntity = new DataAccess.Entities.Unit()
            {
                Description = unit.Description,
                Sequence = unit.Sequence,
                CourseId = unit.CourseId
            };

            this.unitOfWork.Units.Add(unitEntity);
            this.unitOfWork.Complete();

            if (unitEntity.Course == null)
            {
                unitEntity.Course = this.unitOfWork.Courses.Get(unitEntity.CourseId);
            }

            return Ok(new Unit(unitEntity));
        }

        // PUT: api/Units/5
        [HttpPut]
        public IActionResult Put(Unit unit)
        {
            var result = 0;
            var unitDb = this.unitOfWork.Units.SingleOrDefault(x => x.Id == unit.Id);
            if (unitDb != null)
            {
                if (unitDb.Description != unit.Description)
                {
                    unitDb.Description = unit.Description;
                    unitDb.Sequence = unit.Sequence;
                    result = this.unitOfWork.Complete();
                }
            }
            return Ok(result > 0);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var unit = this.unitOfWork.Units.Get(id);
            var objectAffected = 0;
            if (unit != null)
            {
                foreach (var topic in unit.Topics)
                {
                    this.unitOfWork.Topics.Remove(topic);
                }
                this.unitOfWork.Units.Remove(unit);
                objectAffected = this.unitOfWork.Complete();
            }
            return Ok(objectAffected > 0);
        }
    }
}
