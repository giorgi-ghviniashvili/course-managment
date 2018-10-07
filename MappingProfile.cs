using AutoMapper;
using regents_new.Models;
using DataAccess.Repositories;

namespace regents_new
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            var unitOfWork = new UnitOfWork(new DataAccess.AppContext());

            CreateMap<DataAccess.Entities.Course, Course>()
                .ConstructUsing(x => new Course(x));
            CreateMap<DataAccess.Entities.Unit, Unit>()
                .ConstructUsing(x => new Unit(x, unitOfWork.Courses.Get(x.CourseId)));
            CreateMap<DataAccess.Entities.Topic, Topic>()
                .ConstructUsing(x => new Topic(x, unitOfWork.Units.Get(x.UnitId)));
        }
    }
}
