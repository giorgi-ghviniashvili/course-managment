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
    public class TopicsController : ControllerBase
    {
        public readonly IUnitOfWork unitOfWork;

        public TopicsController()
        {
            this.unitOfWork = new UnitOfWork(new DataAccess.AppContext());
        }

        // GET: api/Topics
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

            var topics = this.unitOfWork.Topics.GetAll();

            var topicOnPage = topics.Skip(from).Take(quantity).ToArray();
            var total = topics.Count();

            var model = new List<Topic>();

            foreach (var item in topicOnPage)
            {
                model.Add(new Topic(item));
            }

            return Ok(new
            {
                Total = total,
                Topics = model
            });
        }

        // GET: api/Topics/5
        [HttpGet("{id}", Name = "GetTopics")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Topics
        [HttpPost]
        public IActionResult Post(Topic topic)
        {
            var topicEntity = new DataAccess.Entities.Topic()
            {
                Description = topic.Description,
                UnitId = topic.UnitId
            };

            this.unitOfWork.Topics.Add(topicEntity);
            this.unitOfWork.Complete();

            return Ok(new Topic(topicEntity));
        }

        // PUT: api/Topics/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var topic = this.unitOfWork.Topics.Get(id);
            var objectAffected = 0;
            if (topic != null)
            {
                this.unitOfWork.Topics.Remove(topic);
                    
                objectAffected = this.unitOfWork.Complete();
            }
            return Ok(objectAffected > 0);
        }
    }
}
