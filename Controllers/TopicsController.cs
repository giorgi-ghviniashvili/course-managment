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
    public class TopicsController : ControllerBase
    {
        public readonly IUnitOfWork unitOfWork;
        public readonly IMapper mapper;

        public TopicsController(IMapper mapper)
        {
            this.unitOfWork = new UnitOfWork(new DataAccess.AppContext());
            this.mapper = mapper;
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
            var total = topics.Count();

            var topicOnPage = topics.Skip(from).Take(quantity).ToArray();  

            List<Topic> model = mapper.Map<IEnumerable<DataAccess.Entities.Topic>, List<Topic>>(topicOnPage);

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

            if (topicEntity.Unit == null)
            {
                topicEntity.Unit = this.unitOfWork.Units.Get(topicEntity.UnitId);
            }

            return Ok(new Topic(topicEntity, this.unitOfWork.Units.Get(topicEntity.UnitId)));
        }

        // PUT: api/Topics
        [HttpPut]
        public IActionResult Put(Topic topic)
        {
            var result = 0;
            var topicDb = this.unitOfWork.Topics.SingleOrDefault(x => x.Id == topic.Id);
            if (topicDb != null)
            {
                if (topicDb.Description != topic.Description)
                {
                    topicDb.Description = topic.Description;
                    result = this.unitOfWork.Complete();
                }
            }
            return Ok(result > 0);
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
