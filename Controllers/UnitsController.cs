using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DataAccess.Interfaces;
using DataAccess.Repositories;
using regents_new.Models;

namespace regents_new.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitsController : ControllerBase
    {
        public readonly IUnitOfWork unitOfWork;

        public UnitsController()
        {
            this.unitOfWork = new UnitOfWork(new DataAccess.AppContext());
        }

        // GET: api/Units
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

            var units = this.unitOfWork.Units.GetAll();

            var unitOnPage = units.Skip(from).Take(quantity).ToArray();
            var total = units.Count();

            var model = new List<Unit>();

            foreach (var item in unitOnPage)
            {
                model.Add(new Unit(item));
            }

            return Ok(new
            {
                Total = total,
                Units = model
            });
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

            return Ok(new Unit(unitEntity));
        }

        // PUT: api/Units/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
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
