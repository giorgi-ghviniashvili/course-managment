using AutoMapper;
using regents_new.Models;

namespace regents_new
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<DataAccess.Entities.Course, Course>()
                .ConstructUsing(x => new Course(x));
            CreateMap<DataAccess.Entities.Unit, Unit>()
                .ConstructUsing(x => new Unit(x));
            CreateMap<DataAccess.Entities.Topic, Topic>()
                .ConstructUsing(x => new Topic(x));
        }
    }
}
